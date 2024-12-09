using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NLog;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Helper;

public class GeoHelper(IConfiguration config, HttpClient httpClient)
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public async Task<MapDto> GetAzurePoint(string ipAddress)
    {
        try
        {
            var url = $"https://get.geojs.io/v1/ip/geo/{ipAddress}.json";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return new MapDto();
            
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions{ AllowTrailingCommas = true };
            return JsonSerializer.Deserialize<MapDto>(responseBody, options);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return new MapDto();
        }
    }
}