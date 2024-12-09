using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using ReCaptcha.Extensions;
using Veesy.Discord;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Email;
using Veesy.Service.Implementation;
using Veesy.WebApp;
using Veesy.WebApp.Middlewares;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("NLog.config")
    .GetCurrentClassLogger();
try
{
    logger.Info("Init main");
    var builder = WebApplication.CreateBuilder(args);
    
    // builder.WebHost.ConfigureKestrel(serverOptions =>
    // {
    //     serverOptions.Limits.MaxRequestBodySize = long.MaxValue; //We are allowing the uploading of files of any size.
    //     serverOptions.AllowSynchronousIO = true; //To understand better
    // });
    
    builder.Services.Configure<IISServerOptions>(options =>
    {
        options.MaxRequestBodySize = long.MaxValue;
        options.AllowSynchronousIO = true;
    });
    
    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
        .AddEnvironmentVariables();
    
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
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
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30); 
        options.Cookie.HttpOnly = true; 
        options.Cookie.IsEssential = true; 
    });
    
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
#if  DEBUG
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "Veesy Web API", Version = "v1.0.0" });
    });
#endif
    
    builder.Services.AddRecaptcha(recaptchaOptions =>
    {
        recaptchaOptions.SecretKey = Configuration.GetValue<string>("Captcha:SecretKey");
        recaptchaOptions.SiteKey = Configuration.GetValue<string>("Captcha:SiteKey");
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
    
    builder.Services.AddHttpClient<IDiscordService, DiscordService>();
    
    builder.Services.RegisterVeesyServices();
    
    var app = builder.Build();

#if  DEBUG
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Veesy Web API");
    });
#endif


    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseNotyf();
    
    app.UseAuthentication();
    // app.Use(async (context, next) => {
    //     await next.Invoke();
    //     //handle response
    //     //you may also need to check the request path to check whether it requests image
    //     if (context.User.Identity.IsAuthenticated)
    //     {
    //         var userName = context.User.Identity.Name;
    //         //retrieve uer by userName
    //         using (var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>())
    //         {
    //             var user = dbContext.MyUsers.Where(u => u.UserName == userName).FirstOrDefault();
    //             user.LastLoginTime = DateTime.Now;
    //             dbContext.Update(user);
    //             dbContext.SaveChanges();
    //         }
    //     }
    // });
    app.UseAuthorization();
    app.UseSession();
    app.UseMiddleware<SessionLoggerMiddleware>();
    
    app.MapControllerRoute(
        name: "default",
        pattern: "{area=Public}/{controller=Public}/{action=Splash}");

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
