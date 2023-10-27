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
            IsPublic = true,
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
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            ApplicationUrl = _config["ApplicationUrl"]
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

    public async Task<ResultDto> AddMediaToPortfolio(EditPortfolioDto editPortfolioDto, MyUser user)
    {
        if (editPortfolioDto.Code == Guid.Empty)
            return new ResultDto(false, "Select one portfolio.");
        var portfolio = _portfolioService.GetPortfolioById(editPortfolioDto.Code, user.Id);
        foreach (var item in editPortfolioDto.CodeImagesToAdd)
        {
            if(!portfolio.PortfolioMedias.Select(s => s.MediaId).Contains(item))
                portfolio.PortfolioMedias.Add(new PortfolioMedia()
                {
                    MediaId = item,
                    Description = "",
                    IsActive = true,
                    SortOrder = portfolio.PortfolioMedias.Count
                });
        }

        await _portfolioService.UpdatePortfolio(portfolio, user);
        return new ResultDto(true, "");
    }

    /// <summary>
    /// Metodo utilizzato per cambiare i portfolio a cui l'immagine è associata. All'interno dell'oggetto portfolioDto vengono
    /// si trova una lista di Guid che rappresentano gli Id dei portfolios selezionati in pagina a cui collegare l'immagine.
    /// </summary>
    /// <param name="portfolioDto"></param>
    /// <param name="userInfo"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ResultDto> UpdateMediaLinkedPortfolio(UpdateMediaPortfolioDto portfolioDto, MyUser userInfo)
    {
        var oldPortfolioMediae = _portfolioService.GetPortfoliosMediaByMediaId(portfolioDto.MediaCode).ToList();
        var portfoliosMediaToDelete = new List<PortfolioMedia>();
        var portfoliosMediaToAdd = new List<PortfolioMedia>();
        var portfoliosMediaToUpdate = new List<PortfolioMedia>();
        
        foreach (var item in oldPortfolioMediae)
        {
            if (!portfolioDto.PortfolioSelected.Contains(item.PortfolioId))
            {
                portfoliosMediaToDelete.Add(item);
                var portfoliosMedia =
                    _portfolioService.GetPortfoliosMediaByPortfolioIdToReorder(item.PortfolioId, item.SortOrder);
                foreach (var pf in portfoliosMedia)
                {
                    pf.SortOrder--;
                    portfoliosMediaToUpdate.Add(pf);
                }

            }
        }
        
        foreach (var item in portfolioDto.PortfolioSelected)
        {
            if (!oldPortfolioMediae.Any(s => s.PortfolioId == item))
            {
                var newPortfolio = _portfolioService.GetPortfolioById(item, userInfo.Id);
                portfoliosMediaToAdd.Add(new PortfolioMedia()
                {
                    PortfolioId = item,
                    MediaId = portfolioDto.MediaCode,
                    IsActive = true,
                    SortOrder = newPortfolio.PortfolioMedias.Count,
                    Description = ""
                });
            }
        }

        return await _portfolioService.UpdatePortfolioMedias(portfoliosMediaToDelete, portfoliosMediaToAdd, portfoliosMediaToUpdate, userInfo);
    }

    public async Task<ResultDto> UpdatePassword(UpdatePortfolioDto portfolioDto, MyUser userInfo)
    {
        if(string.IsNullOrEmpty(portfolioDto.Password))
            return new ResultDto(false, "Please insert password.");
        if (portfolioDto.Password.Length < 6)
            return new ResultDto(false, "Min characters are 6.");
        var portfolio = _portfolioService.GetPortfolioById(portfolioDto.Id, userInfo.Id);
        portfolio.Password = portfolioDto.Password;
        portfolio.IsPublic = false;
        await _portfolioService.UpdatePortfolio(portfolio, userInfo);
        return new ResultDto(true, "");
    }

    public async Task<ResultDto> UpdateSecurity(UpdatePortfolioDto portfolioDto, MyUser userInfo)
    {
        var portfolio = _portfolioService.GetPortfolioById(portfolioDto.Id, userInfo.Id);
        portfolio.IsPublic = portfolioDto.IsPublic;
        await _portfolioService.UpdatePortfolio(portfolio, userInfo);
        return new ResultDto(true, "");
    }

    public async Task<ResultDto> SetMainPortfolio(UpdatePortfolioDto portfolioDto, MyUser user)
    {
        var mainPortfolio = _portfolioService.GetMainPortfolioByUser(user);
        if (mainPortfolio != null)
            mainPortfolio.IsMain = false;
        var portfolio = _portfolioService.GetPortfolioById(portfolioDto.Id, user.Id);
        if(portfolio.Id == mainPortfolio.Id)
            return new ResultDto(false, "This portfolio is already set as your main");
        portfolio.IsMain = true;
        var portfoliosToUpdate = new List<Portfolio>();
        portfoliosToUpdate.Add(mainPortfolio);
        portfoliosToUpdate.Add(portfolio);
        await _portfolioService.UpdatePortfolios(portfoliosToUpdate, user);
        return new ResultDto(true, "");
    }

    public async Task<ResultDto> DeletePortfolio(Guid portfolioId, MyUser userInfo)
    {
        var portfolio = _portfolioService.GetPortfolioByIdWithPortfoliosMedia(portfolioId, userInfo.Id);
        if (portfolio == null)
            return new ResultDto(false, "Portfolio not found.");
        if (portfolio.IsMain)
            return new ResultDto(false, "You can't delete main portfolio.");
        await _portfolioService.DeletePortfolio(portfolio, userInfo);
       
        return new ResultDto(true, "");
    }

    public async Task<ResultDto> UpdateLayout(UpdatePortfolioDto portfolioDto, MyUser userInfo)
    {
        var portfolio = _portfolioService.GetPortfolioById(portfolioDto.Id, userInfo.Id);
        if (portfolio == null)
            return new ResultDto(false, "Portfolio not found.");
        portfolio.Layout = (VeesyConstants.PortfolioLayout)portfolioDto.LayoutGrid;
        await _portfolioService.UpdatePortfolio(portfolio, userInfo);
        return new ResultDto(true, "");
    }

    public async Task UpdateSortOrder(UpdateMediaSortOrderDto dto, MyUser userInfo)
    {
        var portfolio = _portfolioService.GetPortfolioById(dto.PortfolioId, userInfo.Id);
        var now = DateTime.Now;
        dto.NewMediasSortOrder.ToList().ForEach(fe =>
        {
            var mediaToUpd = portfolio.PortfolioMedias.SingleOrDefault(sd => sd.MediaId == fe.MediaId);
            mediaToUpd!.SortOrder = fe.SortOrder;
            mediaToUpd!.LastEditRecordDate = now;
            mediaToUpd!.LastEditUserId = userInfo.Id;
        });
        
        await _portfolioService.UpdatePortfolio(portfolio, userInfo);
    }

    public (PortfolioViewModel model, ResultDto result) GetPortfolioPreviewViewModel(Guid id, MyUser user)
    {

        var portfolio = _portfolioService.GetPortfolioByIdForPreview(id);
        if (portfolio == null)
            return (null, new ResultDto(false, "Portfolio not found"));

        return (new PortfolioViewModel
        {
            PortfolioDto = MapPortfolioDtos.MapPreviewPortfolioDto(portfolio),
            BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            BasePathAzure = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/"
        }, new ResultDto(true, ""));
    }

    public async Task<ResultDto> RemoveMediaFromPortfolio(PortfolioMediaDto portfolioMediaDto, MyUser userInfo)
    {
        var portfolio = _portfolioService.GetPortfolioByIdWithPortfoliosMedia(portfolioMediaDto.PortfolioId, userInfo.Id);
        var p_media = portfolio.PortfolioMedias.SingleOrDefault(s => s.MediaId == portfolioMediaDto.MediaId);
        if (p_media == null)
            return new ResultDto(false, "Media not found.");
        foreach (var item in portfolio.PortfolioMedias.Where(s => s.SortOrder > p_media.SortOrder))
        {
            item.SortOrder--;
        }

        var listToDelete = new List<PortfolioMedia>() { p_media };
        await _portfolioService.UpdatePortfolioMedias(listToDelete, new List<PortfolioMedia>(), portfolio.PortfolioMedias.Where(s=>s.MediaId != p_media.MediaId).ToList(),
            userInfo);
        return new ResultDto(true, "");
    }

}