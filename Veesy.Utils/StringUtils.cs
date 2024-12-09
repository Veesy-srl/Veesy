using Veesy.Domain.Constants;

namespace Veesy.Utils;

public class StringUtils
{
    public static VeesyConstants.DeviceType DetermineDeviceType(string userAgent)
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
    
    
    
    public static string GetBrowserFromUserAgent(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
            return VeesyConstants.BrowserUnknown;

        if (userAgent.Contains("Firefox"))
            return VeesyConstants.Firefox;
        if (userAgent.Contains("Edg/") || userAgent.Contains("Edge"))
            return VeesyConstants.MicrosoftEdge;
        if (userAgent.Contains("Chrome"))
        {
            // Alcuni browser usano "Chrome" nel loro UA, quindi dobbiamo escludere alcuni casi.
            if (userAgent.Contains("OPR") || userAgent.Contains("Opera"))
                return VeesyConstants.Opera;
            if (userAgent.Contains("Chromium"))
                return VeesyConstants.Chromium;
            return VeesyConstants.Chrome;
        }
        if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome"))
            return VeesyConstants.Safari;
        if (userAgent.Contains("OPR") || userAgent.Contains("Opera"))
            return VeesyConstants.Opera;
        if (userAgent.Contains("Trident") || userAgent.Contains("MSIE"))
            return VeesyConstants.InternetExplorer;

        return VeesyConstants.BrowserUnknown;
    }
}