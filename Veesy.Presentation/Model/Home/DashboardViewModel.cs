using Veesy.Domain.Models;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Home;

public class DashboardViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string BaseProfileImage { get; set; }
    public string FileName { get; set; }
    public string UserCategory { get; set; }
    public string BasePath { get; set; }
    public string? ApplicationUrl { get; set; }
    public PortfolioThumbnailDto PortfolioThumbnailDto { get; set; }
    public int Percent { get; set; }
    public SubscriptionPlan Subscription { get; set; }
    public int MediaPercent { get; set; }
    public int MediaNumber { get; set; }
    public int PortfolioNumber { get; set; }
    public string UserDescription { get; set; }
    public bool DiscordConnected { get; set; }
    public string? DiscordUsername { get; set; }
}