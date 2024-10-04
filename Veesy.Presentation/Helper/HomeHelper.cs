using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Home;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class HomeHelper
{
    private readonly IConfiguration _config;
    private readonly IPortfolioService _portfolioService;
    private readonly IAccountService _accountService;
    private readonly IMediaService _mediaService;
    private readonly ISubscriptionPlanService _subscriptionPlanService;

    public HomeHelper(IConfiguration config, IPortfolioService portfolioService, IAccountService accountService, IMediaService mediaService, ISubscriptionPlanService subscriptionPlanService)
    {
        _config = config;
        _portfolioService = portfolioService;
        _accountService = accountService;
        _mediaService = mediaService;
        _subscriptionPlanService = subscriptionPlanService;
    }

    public DashboardViewModel GetDashboardViewModel(MyUser user)
    {
        var portfolio = _portfolioService.GetMainPortfolioByUserWithMedias(user);
        var portfolioNumber = _portfolioService.GetPortfoliosNumberByUser(user.Id);
        var numberMedia = _mediaService.GetMediaNumberByUser(user.Id);
        var percent = (_accountService.NumberRecordCompiled(user) * 100 / 26);
        var subscription = _subscriptionPlanService.GetSubscriptionByUserId(user.Id);
        return new DashboardViewModel()
        {
            PortfolioThumbnailDto = MapPortfolioDtos.MapPortfolioThumbnailDto(portfolio),
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            ApplicationUrl = _config["ApplicationUrl"],
            Name = user.Name,
            UserDescription = user.Biografy != null && user.Biografy.Length > 340 ? user.Biografy.Substring(0, 340) + "..." : user.Biografy,
            PortfolioNumber = portfolioNumber,
            Surname = user.Surname,
            UserCategory = user.Category,
            DiscordConnected = !string.IsNullOrEmpty(user.DiscordId),
            DiscordUsername = user.DiscordUsername,
            FileName = user.ProfileImageFileName,
            Percent = percent,
            MediaNumber = numberMedia,
            MediaPercent = (numberMedia * 100) / subscription.AllowedMediaNumber,
            Subscription = subscription,
            BaseProfileImage = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/",
        };
    }
}