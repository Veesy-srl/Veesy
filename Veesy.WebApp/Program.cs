using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Email;
using Veesy.Media.Utils;
using Veesy.Presentation.Helper;
using Veesy.Service.Implementation;
using Veesy.Validators;
using Veesy.WebApp;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("NLog.config")
    .GetCurrentClassLogger();
try
{
    logger.Info("Init main");
    var builder = WebApplication.CreateBuilder(args);
    
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.Limits.MaxRequestBodySize = long.MaxValue; //We are allowing the uploading of files of any size.
        serverOptions.AllowSynchronousIO = true; //To understand better
    });
    
    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
        .AddEnvironmentVariables();
    
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    builder.Host.UseNLog();
    
    ConfigurationManager Configuration = builder.Configuration;
    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("VeesyConnection"))); // per entity framework
    
    var emailConfig = Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
    builder.Services.AddSingleton(emailConfig);
    
    builder.Services.AddIdentity<MyUser, IdentityRole>(opts =>
        {
            opts.SignIn.RequireConfirmedEmail = true;
            opts.SignIn.RequireConfirmedAccount = false;
            opts.Password.RequireDigit = true;
            opts.Password.RequireLowercase = true;
            opts.Password.RequireUppercase = true;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    
    // Add services to the container.
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
    
    builder.Services.AddNotyf(config =>
    {
        config.DurationInSeconds = 10; 
        config.IsDismissable = true;
        config.Position = NotyfPosition.TopCenter;
    });

    
    //Register azure blob storage
    var azureBlobCs = Configuration.GetValue<string>("AzureBlobStorage:ConnectionString");

    builder.Services.AddSingleton(x => new BlobServiceClient(azureBlobCs));
    builder.Services.AddSingleton(x=> 
        new VeesyBlobService(new BlobServiceClient(azureBlobCs), 
            Configuration.GetValue<string>("AzureBlobStorage:VeesyContainerName")));
    
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "Veesy Web API", Version = "v1.0.0" });
    });
    
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.Redirect(options.LoginPath);
                return Task.FromResult(0);
            }
        };
    });
    
    builder.Services.RegisterVeesyServices();
    
    var app = builder.Build();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Veesy Web API");
    });

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseNotyf();
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllerRoute(
        name: "default",
        pattern: "{area=Portfolio}/{controller=Home}/{action=Index}");

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<MyUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        context.Database.Migrate();
        DbInitializer.SeedUsersAndRoles(userManager, roleManager, context, Configuration);
    }
    logger.Debug("Program Initialized, Running App...");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}