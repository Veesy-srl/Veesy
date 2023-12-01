using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Admin;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class AdminHelper
{

    private readonly IAccountService _accountService;
    private readonly IMediaService _mediaService;
    private readonly IPortfolioService _portfolioService;
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;


    public AdminHelper(IAccountService accountService, IMediaService mediaService, IPortfolioService portfolioService, UserManager<MyUser> userManager, IConfiguration config)
    {
        _accountService = accountService;
        _mediaService = mediaService;
        _portfolioService = portfolioService;
        _userManager = userManager;
        _config = config;
    }

    public CreatorsListViewModel GetCreatorsListViewModel()
    {
        
        var users = _accountService.GetCreators();
        var vm = new CreatorsListViewModel()
        {
            FreelancerDtos = MapAdminDto.MapCreatorDtos(users),
        };
        return vm;
    }

    public CreatorViewModel GetCreatorViewModel(string id)
    {
        var user = _accountService.GetUserById(id);
        var portfolios = _portfolioService.GetPortfoliosByUserWithMedia(user);
        var medias = _mediaService.GetAllByUserId(user);
        var elementProfileCompiled = _accountService.NumberRecordCompiled(user);
        var vm = new CreatorViewModel()
        {
            ElemProfileCompiled = elementProfileCompiled,
            ProfilePercentCompiled = (elementProfileCompiled * 100 / 17),
            User = user,
            Medias = medias,
            Portfolios = portfolios.ToList()
        };
        return vm;
    }
    
    public CreatorsListViewModel GetCreatorsPlusListViewModel()
    {
        var users = _accountService.GetCreatorsPlus();
        var vm = new CreatorsListViewModel()
        {
            FreelancerDtos = MapAdminDto.MapCreatorDtos(users),
            ApplicationUrl = _config["ApplicationUrl"]
        };
        return vm;
    }

    public DashboardViewModel GetDashboardViewModel()
    {
        var res = _mediaService.GetMediaNumberByMonthGroupByDay(DateTime.Now.AddMonths(-1));
        var resCreator = _accountService.GetCreatorNumberByMonthGroupByDay(DateTime.Now.AddMonths(-1));
        var lastMediaUploaded = _mediaService.GetLastFourMediaUploaded();
        var numberPayingUsers = _accountService.GetNumberPayingUsers(); 
        var mediaOverview = new List<MediaOverviewDto>();
        var creatorOverview = new List<CreatorOverviewDto>();
        var creators = _accountService.GetCreators();
        foreach (var item in res)
        {
            mediaOverview.Add(new MediaOverviewDto()
            {
                Day = item.Day,
                MediaSize = item.MediaSize / (1024*1024),
                Month = item.Date.Month,
                NumberMedia = item.NumberMedia,
                Year = item.Date.Year
            });
        }
        foreach (var item in resCreator)
        {
            creatorOverview.Add(new CreatorOverviewDto()
            {
                Day = item.Day,
                NumberCreator = item.NumberCreator,
                Month = item.Date.Month,
                Year = item.Date.Year
            });
        }
        var vm = new DashboardViewModel()
        {
            MediaNumber = _mediaService.GetMediaNumber(),
            CreatorNumber = creators.Count,
            MediaOverviewDtos = mediaOverview,
            CreatorOverviewDtos = creatorOverview,
            NumberPayingUsers = numberPayingUsers,
            LastMediaUploadDtos = MapAdminDto.MapLastMediaUploadDtos(lastMediaUploaded)
        };
        return vm;
    }
}