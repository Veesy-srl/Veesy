using Veesy.Domain.Models.Log;
using Veesy.Presentation.Helper;
using Veesy.Utils;

namespace Veesy.WebApp.Middlewares;

public class SessionLoggerMiddleware
{

    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public SessionLoggerMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        this._next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var authHelper = scope.ServiceProvider.GetRequiredService<AuthHelper>();
        var azureHelper = scope.ServiceProvider.GetRequiredService<GeoHelper>();

        if (context.User.Identity!.IsAuthenticated && !context.Session.TryGetValue("LastLoginLogged", out _))
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                context.Session.SetString("LastLoginLogged", "true");
                
                var userAgent = context.Request.Headers["User-Agent"].ToString();
                var browser = StringUtils.GetBrowserFromUserAgent(userAgent);
                var ipAddress = context.Connection.RemoteIpAddress?.ToString();
                var map = await azureHelper.GetAzurePoint(ipAddress);
                await authHelper.AddLastLogin(new UserSecurity
                {
                    MyUserId = userId,
                    IpAddress = ipAddress,
                    LastAccess = DateTime.Now,
                    UserAgent = userAgent,
                    DeviceType = StringUtils.DetermineDeviceType(userAgent),
                    Browser = browser,
                    Longitude = map.longitude,
                    Country = map.country,
                    Region = map.region,
                    City = map.city,
                    Accuracy = map.accuracy,
                    Timezone = map.timezone,
                    ASN = map.asn,
                    Organization = map.organization,
                    AreaCode = map.area_code,
                    OrganizationName = map.organization_name,
                    CountryCode = map.country_code,
                    CountryCode3 = map.country_code3,
                    ContinentCode = map.continent_code,
                    Latitude = map.latitude,

                });
            }
        }

        await _next(context);
    }
}