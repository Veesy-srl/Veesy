using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Email;
using Veesy.Media.Utils;
using Veesy.Presentation.Helper;
using Veesy.Service.Implementation;
using Veesy.Validators;

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
            opts.SignIn.RequireConfirmedEmail = false;
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
    
    //Register azure blob storage
    var azureBlobCs = Configuration.GetValue<string>("AzureBlobStorage:ConnectionString");

    builder.Services.AddSingleton(x => new BlobServiceClient(azureBlobCs));
    builder.Services.AddSingleton(x=> 
        new VeesyBlobService(new BlobServiceClient(azureBlobCs), 
            Configuration.GetValue<string>("AzureBlobStorage:VeesyContainerName")));
    
    /*Service dependency Injection*/
    
    /*Utils Dependency Injection*/
    builder.Services.AddScoped<IEmailSender, EmailSender>();
    
    
    /*Validator Dependency Injection*/
    builder.Services.AddTransient<MyUserValidator>();
    
    /*Helper Dependency Injection*/
    builder.Services.AddTransient<AuthHelper>();
    builder.Services.AddTransient<MediaHelper>();

    /*Media Utils Dependency Injection*/
    builder.Services.AddTransient<MediaHandler>();
    
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRouting();
    app.UseAuthorization();
    
    app.MapControllerRoute(
        name: "default",
        pattern: "{area=Auth}/{controller=Auth}/{action=Login}");

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
