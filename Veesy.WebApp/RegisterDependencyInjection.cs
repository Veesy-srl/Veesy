using Veesy.Domain.Repositories;
using Veesy.Domain.Repositories.Impl;
using Veesy.Email;
using Veesy.Presentation.Helper;
using Veesy.Service.Implementation;
using Veesy.Service.Interfaces;
using Veesy.Validators;

namespace Veesy.WebApp;

public static class RegisterDependencyInjection
{
    public static IServiceCollection RegisterVeesyServices(this IServiceCollection serviceCollection)
    {
        /*Repository Dependency Injection*/
        serviceCollection.AddScoped<IMyUserRepository, MyUserRepository>();
        serviceCollection.AddScoped<IUsedSoftwareRepository, UsedSoftwareRepository>();
        serviceCollection.AddScoped<IMediaRepository, MediaRepository>();
        serviceCollection.AddScoped<IVeesyUoW, VeesyUoW>();
        
        /*Service Dependency Injection*/
        serviceCollection.AddTransient<IAccountService, AccountService>();
        serviceCollection.AddTransient<IMediaService, MediaService>();
        serviceCollection.AddTransient<ISubscriptionPlanService, SubscriptionPlanService>();

        /*Utils Dependency Injection*/
        serviceCollection.AddScoped<IEmailSender, EmailSender>();
    
    
        /*Validator Dependency Injection*/
        serviceCollection.AddTransient<MyUserValidator>();
        serviceCollection.AddTransient<MediaValidators>();
    
        /*Helper Dependency Injection*/
        serviceCollection.AddTransient<AuthHelper>();
        serviceCollection.AddTransient<MediaHelper>();
        serviceCollection.AddTransient<ProfileHelper>();
        serviceCollection.AddTransient<CloudHelper>();
        serviceCollection.AddTransient<PortfolioHelper>();
        
        return serviceCollection;
    }

}