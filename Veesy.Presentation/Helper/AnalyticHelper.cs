using System;
using System.Threading.Tasks;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Service.Interfaces;
using Veesy.Utils;

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
        var browser = StringUtils.GetBrowserFromUserAgent(ua);
        if (browser != VeesyConstants.BrowserUnknown)
        {
            await _analyticService.SaveReferralTrackingLink(new ReferralLinkTracking
            {
                Ip = ip,
                ReferralLinkId = res?.Id,
                Referer = referer,
                LastAccess = DateTime.Now,
                Latitude = latitudine,
                Longitude = longitudine,
                Browser = browser,
                DeviceType = StringUtils.DetermineDeviceType(ua),
                UserAgent = ua
            });
        }

        if (res == null || !res.Enable)
            return null;

        return res.RedirectUrl;
    }
    
}