using Cronos;
using NLog;

namespace Veesy.WorkerService.Worker;

public class Worker : BackgroundService
{
    private const string SCHEDULE_EVERY_FIVE_MINUTES = "*/2 * * * *";
    private readonly TimeSpan _period = TimeSpan.FromSeconds(60);
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly CronExpression _cron;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    
    public Worker(IConfiguration configuration)
    {
        _cron = CronExpression.Parse(SCHEDULE_EVERY_FIVE_MINUTES);
        _configuration = configuration;
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

        // Pass the handler to httpclient(from you are calling api)
        _httpClient = new HttpClient(clientHandler);
        
        _httpClient.DefaultRequestHeaders.Add("xapikey", _configuration["ApiKey"]);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        var counter = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            await WorkerTask(stoppingToken, counter);
            counter += 1;
        }    
    }

    private async Task WorkerTask(CancellationToken stoppingToken, int counter)
    {
        Console.WriteLine($"Counter: {counter}");
        var endpoints = _configuration.GetSection("Endpoints").Get<List<string>>();
        var actionsEveryTwoMinutes = _configuration.GetSection("ActionsEveryTwoMinutes").Get<List<string>>();
        var actionsEveryFiveMinutes = _configuration.GetSection("ActionsEveryFiveMinutes").Get<List<string>>();
        var actionsEveryTenMinutes = _configuration.GetSection("ActionsEveryTenMinutes").Get<List<string>>();
        var actionsEveryHalfHour = _configuration.GetSection("ActionsEveryHalfHour").Get<List<string>>();
        var actionsEveryHour = _configuration.GetSection("ActionsEveryHour").Get<List<string>>();
        var actionsEveryDay = _configuration.GetSection("ActionsEveryDay").Get<List<string>>();
        var now = DateTime.Now;
        Console.WriteLine($"Time execution: {now.ToString("dd/MM/yyyy HH:mm:ss")}");
        foreach (var endpoint in endpoints)
        {
            if (counter % 2 == 0 && actionsEveryTwoMinutes != null)
            {
                Console.WriteLine($"Eseguo funzioni 2 min");
                if (actionsEveryTwoMinutes != null)
                {
                    foreach (var action in actionsEveryTwoMinutes)
                    {
                        try
                        {
                            await _httpClient.GetStringAsync($"{endpoint}{action}");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, ex.Message);
                        }
                    }
                }
            }
            
            if (counter % 5 == 0 && actionsEveryFiveMinutes != null)
            {
                Console.WriteLine($"Eseguo funzioni 5 min");
                if (actionsEveryFiveMinutes != null)
                {
                    foreach (var action in actionsEveryFiveMinutes)
                    {
                        try
                        {
                            await _httpClient.GetStringAsync($"{endpoint}{action}");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, ex.Message);
                        }
                    }
                }
            }
            
            if (counter % 10 == 0 && actionsEveryTenMinutes != null)
            {
                Console.WriteLine($"Eseguo funzioni 10 min");
                if (actionsEveryTenMinutes != null)
                {
                    foreach (var action in actionsEveryTenMinutes)
                    {
                        try
                        {
                            await _httpClient.GetStringAsync($"{endpoint}{action}");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, ex.Message);
                        }
                    }
                }
            }

            if (counter % 30 == 0 && actionsEveryHalfHour != null)
            {
                Console.WriteLine($"Eseguo funzioni 30 min");
                foreach (var action in actionsEveryHalfHour)
                {
                    try
                    {
                        await _httpClient.GetStringAsync($"{endpoint}{action}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, ex.Message);
                    }                }
            }
            
            if (counter % 60 == 0 && actionsEveryHour != null)
            {
                Console.WriteLine($"Eseguo funzioni 60 min");
                foreach (var action in actionsEveryHour)
                {
                    try
                    {
                        await _httpClient.GetStringAsync($"{endpoint}{action}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, ex.Message);
                    }                
                }
            }
            
            if (now.Hour == 3 && now.Minute == 0 && actionsEveryDay != null)
            {
                Console.WriteLine($"Eseguo funzioni 1 day.");
                foreach (var action in actionsEveryDay)
                {
                    try
                    {
                        await _httpClient.GetStringAsync($"{endpoint}{action}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, ex.Message);
                    }                }
            }
        }

        var utcNow = DateTime.UtcNow;
        var nextUtc = _cron.GetNextOccurrence(utcNow);
        await Task.Delay(nextUtc.Value - utcNow, stoppingToken);
        
    }
}