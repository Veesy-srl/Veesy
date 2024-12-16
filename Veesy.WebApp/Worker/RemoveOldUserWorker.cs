using NLog;
using Veesy.Presentation.Helper;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Veesy.WebApp.Worker;

public class RemoveOldUserWorker(ILogger<RemoveOldUserWorker> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var profileHelper = scope.ServiceProvider.GetRequiredService<ProfileHelper>();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                await profileHelper.RemoveOldUser();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}