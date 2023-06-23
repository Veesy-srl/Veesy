using Microsoft.AspNetCore.Identity;
using NLog;
using Veesy.Domain.Models;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("NLog.config")
    .GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    builder.Services.AddControllersWithViews();

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

    app.UseRouting();

    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{areas=Portfolio}/{controller=Home}/{action=Index}");

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        //var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<MyUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        //context.Database.Migrate();
        //DbInitializer.SeedUsersAndRoles(userManager, roleManager, context, Configuration);
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