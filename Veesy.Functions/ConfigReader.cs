using Microsoft.Extensions.Configuration;

namespace Veesy.Functions;

public class ConfigReader
{
    private IConfigurationRoot _config;

    public ConfigReader(Microsoft.Azure.WebJobs.ExecutionContext context)
    {
        this._config = new ConfigurationBuilder()
            .SetBasePath(context.FunctionAppDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
    
    public string GetConnectionString()
    {
        return this._config.GetConnectionString("VeesyConnection");
    }

    public T GetSection<T>(string sectionName)
    {
        return this._config.GetSection(sectionName).Get<T>();
    }
}