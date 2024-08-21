using System;
using System.Threading.Tasks;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class AnalyticHelper
{
    private readonly IAnalyticService _analyticService;

    public AnalyticHelper(IAnalyticService analyticService)
    {
        _analyticService = analyticService;
    }

    public async Task<string?> SaveReferral(string startEndpoint, string ip, string referer, string latitudine, string longitudine, string ua)
    {
        var res = _analyticService.GetReferralByStartEndpoint(startEndpoint);
        
        await _analyticService.SaveReferralTrackingLink(new ReferralLinkTracking
        {
            Ip = ip,
            ReferralLinkId = res?.Id,
            Referer = referer,
            LastAccess = DateTime.Now,
            Latitude = latitudine,
            Longitude = longitudine,
            Browser = GetBrowserFromUserAgent(ua),
            DeviceType = DetermineDeviceType(ua),
            UserAgent = ua
        });
        
        if (res == null || !res.Enable)
            return null;

        return res.RedirectUrl;
    }
    
    private string GetBrowserFromUserAgent(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
            return "Unknown";

        if (userAgent.Contains("Firefox"))
            return "Firefox";
        if (userAgent.Contains("Edg/") || userAgent.Contains("Edge"))
            return "Microsoft Edge";
        if (userAgent.Contains("Chrome"))
        {
            // Alcuni browser usano "Chrome" nel loro UA, quindi dobbiamo escludere alcuni casi.
            if (userAgent.Contains("OPR") || userAgent.Contains("Opera"))
                return "Opera";
            if (userAgent.Contains("Chromium"))
                return "Chromium";
            return "Chrome";
        }
        if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome"))
            return "Safari";
        if (userAgent.Contains("OPR") || userAgent.Contains("Opera"))
            return "Opera";
        if (userAgent.Contains("Trident") || userAgent.Contains("MSIE"))
            return "Internet Explorer";

        return "Unknown";
    }
    
    private VeesyConstants.DeviceType DetermineDeviceType(string userAgent)
    {
        if (userAgent.Contains("Mobile"))
        {
            return VeesyConstants.DeviceType.Mobile;
        }
        if (userAgent.Contains("Tablet"))
        {
            return VeesyConstants.DeviceType.Tablet;
        }
        return VeesyConstants.DeviceType.Desktop;
    }
    
}