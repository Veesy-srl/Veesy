using Veesy.Domain.Repositories;
using Veesy.Domain.Repositories.Impl;
using Veesy.Email;
using Veesy.Media.Utils;
using Veesy.Presentation.Helper;
using Veesy.Service.Implementation;
using Veesy.Service.Interfaces;
using Veesy.Validators;

namespace Veesy.WebApp;

public static class RegisterDependencyInjection
{
    public static IServiceCollection RegisterVeesyServices(this IServiceCollection serviceCollection)
    {
        /*Rpository Dependency Injection*/
        serviceCollection.AddScoped<IMyUserRepository, MyUserRepository>();
        serviceCollection.AddScoped<IVeesyUoW, VeesyUoW>();
        
        /*Service dependency Injection*/
        serviceCollection.AddTransient<IAccountService, AccountService>();

        /*Utils Dependency Injection*/
        serviceCollection.AddScoped<IEmailSender, EmailSender>();
    
    
        /*Validator Dependency Injection*/
        serviceCollection.AddTransient<MyUserValidator>();
    
        /*Helper Dependency Injection*/
        serviceCollection.AddTransient<AuthHelper>();
        serviceCollection.AddTransient<MediaHelper>();
        serviceCollection.AddTransient<ProfileHelper>();

        /*Media Utils Dependency Injection*/
        serviceCollection.AddTransient<MediaHandler>();
        
        return serviceCollection;
    }

}