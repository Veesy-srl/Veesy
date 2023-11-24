using Microsoft.AspNetCore.Identity;
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

    public AdminHelper(IAccountService accountService, IMediaService mediaService, IPortfolioService portfolioService, UserManager<MyUser> userManager)
    {
        _accountService = accountService;
        _mediaService = mediaService;
        _portfolioService = portfolioService;
        _userManager = userManager;
    }

    public FreelancersListViewModel GetFreelancersListViewModel()
    {
        
        var users = _accountService.GetFreelancer();
        var vm = new FreelancersListViewModel()
        {
            FreelancerDtos = MapAdminDto.MapFrelancerDtos(users),
        };
        return vm;
    }

    public FreelancerViewModel GetFreelancerViewModel(string id)
    {
        var user = _accountService.GetUserById(id);
        var portfolios = _portfolioService.GetPortfoliosByUserWithMedia(user);
        var medias = _mediaService.GetAllByUserId(user);
        var vm = new FreelancerViewModel()
        {
            User = user,
            Medias = medias,
            Portfolios = portfolios.ToList()
        };
        return vm;
    }

    public DashboardViewModel GetDashboardViewModel()
    {
        var res = _mediaService.GetMediaNumberByMonthGroupByDay(DateTime.Now.AddMonths(-1));
        var lastMediaUploaded = _mediaService.GetLastFourMediaUploaded();
        var mediaOverview = new List<MediaOverviewDto>();
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
        var vm = new DashboardViewModel()
        {
            MediaOverviewDtos = mediaOverview,
            LastMediaUploadDtos = MapAdminDto.MapLastMediaUploadDtos(lastMediaUploaded)
        };
        return vm;
    }
}