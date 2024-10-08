using System;
using System.Collections.Generic;
using System.Linq;

namespace Veesy.Domain.Constants;

public static class VeesyConstants
{
    public static class SubscriptionPlan
    {
        public const string Free = "Free";
        public const string Pro = "Pro";
        public const string Plus = "Plus";
        public const string Beta = "Beta";
        public const string Fou = "Fou";

        public static Dictionary<string, string> SubscriptionSpan = new Dictionary<string, string>
        {
            { "Free", "btn-light" },
            { "Pro", "btn-success" },
            { "Fou", "btn-success" },
            { "Plus", "btn-primary" },
            { "Beta", "btn-danger" },

        };
    }
    
    public enum DeviceType
    {
        Mobile = 0,
        Tablet,
        Desktop,
        Uknown = 9
    }
    
    public static Dictionary<DeviceType, string> DeviceTypeText = new Dictionary<DeviceType, string>
    {
        { DeviceType.Mobile, "Mobile" },
        { DeviceType.Tablet, "Tablet" },
        { DeviceType.Desktop, "Desktop" },
        { DeviceType.Uknown,  "Sconosciuto"},
    };
    
    public static class InfoToShow
    {
        public const string Software = "Software";
        public const string SoftSkill = "Soft Skills";
        public const string HardSkill = "Hard Skills";
        public const string CV = "Downloadable CV";
        public const string Fields = "Fields";
    }
    
    public enum PortfolioLayout
    {
        Unset = 0,
        OneColumn = 1,
        TwoColumns = 2,
        ThreeColumns = 3,
        FourColumns = 4
    }
    public enum FormType
    {
        CreatorType = 0
    }

    public static List<PortfolioLayout> GetAvailableLayouts(bool excludeDefault = true)
    {
        var layouts = Enum.GetValues(typeof(PortfolioLayout)).Cast<PortfolioLayout>();
        if(excludeDefault)
            return layouts.Where(w=>w!=0).ToList();
        return layouts.ToList();
    }

    #region Browser

    public const string Opera = "Opera";
    public const string Chrome = "Chrome";
    public const string Firefox = "Firefox";
    public const string MicrosoftEdge = "Microsoft Edge";
    public const string Chromium = "Chromium";
    public const string Safari = "Safari";
    public const string InternetExplorer = "Internet Explorer";
    public const string BrowserUnknown = "Unknown";

    #endregion
}