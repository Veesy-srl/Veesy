using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
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
    
    public PortfolioSettingsViewModel GetPortfolioSettingsViewModel(Guid id, MyUser userInfo)
    {
        var vm = new PortfolioSettingsViewModel();
        var portfolio = _portfolioService.GetPortfolioById(id, userInfo.Id);
        vm.Portfolio = MapPortfolioDtos.MapPortfolio(portfolio);
        vm.BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/";

        return vm;
    }

    public void UpdatePortfolioSettings()
    {
        
    }

    public async Task<(ResultDto result, Guid code)> CreateNewPortfolio(NewPortfolioDto newPortfolioDto, MyUser userInfo)
    {
        if(string.IsNullOrEmpty(newPortfolioDto.Name))
            return (new ResultDto(false, "Please insert portfolio's name."), Guid.Empty);
        if(newPortfolioDto.Name.Length > 50)
            return (new ResultDto(false, "Max name characters are 50."), Guid.Empty);
        if (newPortfolioDto.CodeImagesToAdd == null || newPortfolioDto.CodeImagesToAdd.Count == 0)
            return (new ResultDto(false, "Select at least one image."), Guid.Empty);
        
        var portfoliosNumber = _portfolioService.GetPortfoliosByUser(userInfo).Count();
        var potfoliosMedia = new List<PortfolioMedia>();
        var index = 0;
        newPortfolioDto.CodeImagesToAdd.ForEach(code => potfoliosMedia.Add(new PortfolioMedia()
        {
            IsActive = true,
            MediaId = code,
            SortOrder = index++,
            Description = ""
        }));
        
        var portfolio = new Portfolio()
        {
            IsMain = portfoliosNumber == 0,
            Layout = VeesyConstants.PortfolioLayout.FourColumns,
            Name = newPortfolioDto.Name,
            Description = "",
            IsPublic = false,
            PortfolioMedias = potfoliosMedia,
            Note = "",
            Status = PortfolioContants.STATUS_DRAFT,
            Password = "",
            Link = "",
            MyUserId = userInfo.Id
        };
        var res = await _portfolioService.AddPortfolio(portfolio, userInfo);
        return (new ResultDto(true, ""), res.Id);
    }

    public PortfolioListViewModel GetListViewModel(MyUser userInfo)
    {
        var portfolios = _portfolioService.GetPortfoliosByUserWithMedia(userInfo).ToList();
        var vm = new PortfolioListViewModel()
        {
            PortfolioThumbnailDtos = MapPortfolioDtos.MapListPortfolioThumbnailDto(portfolios),
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/"
        };
        return vm;
    }

    public async Task<ResultDto> UpdateNamePortfolio(UpdatePortfolioDto portfolioDto, MyUser userInfo)
    {
        if(string.IsNullOrEmpty(portfolioDto.Name))
            return new ResultDto(false, "Please insert portfolio's name.");
        if (portfolioDto.Name.Length > 50)
            return new ResultDto(false, "Max name characters are 50.");
        var portfolio = _portfolioService.GetPortfolioById(portfolioDto.Id, userInfo.Id);
        portfolio.Name = portfolioDto.Name;
        await _portfolioService.UpdatePortfolio(portfolio, userInfo);
        return new ResultDto(true, "");
    }
}