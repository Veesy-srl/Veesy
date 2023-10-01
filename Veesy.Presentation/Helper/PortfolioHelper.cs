using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Portfolio;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class PortfolioHelper
{
    private readonly IConfiguration _config;
    private readonly IPortfolioService _portfolioService;

    public PortfolioHelper(IConfiguration config, IPortfolioService portfolioService)
    {
        _config = config;
        _portfolioService = portfolioService;
    }
    
    public PortfolioSettingsViewModel GetPortfolioSettingsViewModel(MyUser userInfo)
    {
        var vm = new PortfolioSettingsViewModel();
        // var portfolio = _portfolioService.GetPortfolioById(new Guid(), userInfo.Id);
        // vm.Portfolio = MapPortfolioDtos.MapPortfolio(portfolio);
        vm.Portfolio = new PortfolioDto
        {
            Name = "NamePlaceholder",
            Description = "DescriptionPlaceholder",
            IsPublic = true,
            IsMain = true,
            LastEditRecordDate = DateTime.Now.AddDays(-3),
            Layout = VeesyConstants.PortfolioLayout.TwoColumns
        };
        vm.BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/";

        return vm;
    }
}