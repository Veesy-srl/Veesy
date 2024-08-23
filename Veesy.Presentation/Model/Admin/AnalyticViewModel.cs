using System.Collections.Generic;
using Veesy.Domain.Models;

namespace Veesy.Presentation.Model.Admin;

public class AnalyticViewModel
{
    public List<ReferralLink> ReferralLinks { get; set; }
    public List<ReferralLinkTracking> ReferralLinkTrackings { get; set; }
    public string? ApplicationUrl { get; set; }
    public List<string> RedirectUrls { get; set; }
}