using NLog;
using Veesy.WorkerService.Worker;


var logger = LogManager.Setup()
    .LoadConfigurationFromFile("NLog.config")
    .GetCurrentClassLogger();

try
{
    logger.Info("Init main");
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(configHost =>
        {
            //Todo: capire perchè in prod non funziona;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            configHost.AddJsonFile($"appsettings.{environmentName}.json");
        })
        .ConfigureLogging(logger =>
        {
            logger.ClearProviders();
            logger.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<Worker>();
        })
        .Build();

    host.Run();
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