using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Portfolio;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Helper;

public class PortfolioHelper
{
    private readonly IConfiguration _config;

    public PortfolioHelper(IConfiguration config)
    {
        _config = config;
    }
    
    public PortfolioSettingsViewModel GetPortfolioSettingsViewModel(MyUser userInfo)
    {
        var vm = new PortfolioSettingsViewModel();

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