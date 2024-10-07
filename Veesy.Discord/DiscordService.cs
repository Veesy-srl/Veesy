using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Veesy.Service.Dtos;

namespace Veesy.Discord;

public class DiscordService : IDiscordService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public DiscordService(IConfiguration config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
    }

    private async Task<string> GetDiscordToken(string code)
    {
        var data = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _config["Discord:ClientId"]),
            new KeyValuePair<string, string>("client_secret", _config["Discord:ClientSecret"]),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", $"{_config["ApplicationUrl"]}/callbackdiscord")
        });

        var response = await _httpClient.PostAsync($"{_config["Discord:ApiEndpoint"]}/oauth2/token", data);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
        
            var tokenResponse = JsonSerializer.Deserialize<DiscordTokenResponseDto>(json);
            return tokenResponse?.AccessToken;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Errore nella richiesta del token: {response.StatusCode}, Dettagli: {error}");
        }
    }

    public async Task<DiscordUserDto> GetDiscordUser(string code)
    {
        var accessToken = await GetDiscordToken(code);

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync($"{_config["Discord:ApiEndpoint"]}/users/@me");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<DiscordUserDto>(json);
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Errore nel recupero dell'utente: {response.StatusCode}, Dettagli: {error}");
        }
    }


}