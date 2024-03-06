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
    private readonly ISubscriptionPlanService _subscriptionPlanService;
    private readonly IMediaService _mediaService;
    private readonly IPortfolioService _portfolioService;
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;


    public AdminHelper(IAccountService accountService, IMediaService mediaService, IPortfolioService portfolioService, UserManager<MyUser> userManager, IConfiguration config, ISubscriptionPlanService subscriptionPlanService)
    {
        _accountService = accountService;
        _mediaService = mediaService;
        _portfolioService = portfolioService;
        _userManager = userManager;
        _config = config;
        _subscriptionPlanService = subscriptionPlanService;
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
            ProfilePercentCompiled = (elementProfileCompiled * 100 / 26),
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
        var now = DateTime.Now;
        var res = _mediaService.GetMediaNumberByMonthGroupByDay(now.Month, now.Year);
        var resCreator = _accountService.GetCreatorNumberByMonthGroupByDay(now.Month, now.Year);
        var lastUserCreated = _accountService.GetLastFourCreatedUser(5);
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
            LastUsersCreated = lastUserCreated,
            CreatorOverviewDtos = creatorOverview,
            NumberPayingUsers = numberPayingUsers,
            LastMediaUploadDtos = MapAdminDto.MapLastMediaUploadDtos(lastMediaUploaded)
        };
        return vm;
    }

    public SubscriptionOverviewViewModel GetSusbscriptionsOverviewViewModel()
    {
        var subscriptionPlanGroupedByUserId = _subscriptionPlanService.GetMyUserSubscriptionPlanGoupByUserId();
        var res = GetEarningsThisYear(subscriptionPlanGroupedByUserId);
        var vm = new SubscriptionOverviewViewModel()
        {
            EarningsThisMonth = _subscriptionPlanService.GetEarningsByMonth(DateTime.Now.Month),
            EarningsThisYear = res.Item1,
            EarningGraph = res.Item2,
            NumberPayingUsers = _accountService.GetNumberPayingUsers() 
        };
        return vm;
    }
    
    private (decimal, List<EarningYearGroupedByMonthDto>) GetEarningsThisYear(List<IGrouping<string, MyUserSubscriptionPlan>> myUserSubscriptionPlan)
    {
        decimal total = 0;
        var graph = new List<EarningYearGroupedByMonthDto>()
        {
            new(1, 0),new(2, 0),new(3, 0),new(4, 0),new(5, 0),new(6, 0),new(7, 0),new(8, 0),new(9, 0),new(10, 0),new(11, 0),new(12, 0)
        };
        foreach (var item in myUserSubscriptionPlan)
        {
            var subscriptions = item.ToList();
            var lastSub = new SubscriptionPlan();
            for (int i = 1; i < 13; i++)
            {
                var tmpSub = subscriptions.LastOrDefault(s => s.CreateRecordDate.Month == i);
                lastSub = tmpSub != null ? tmpSub.SubscriptionPlan : lastSub;
                if (lastSub.Id != Guid.Empty)
                {
                    total += lastSub.Price;
                    graph.SingleOrDefault(s => s.Month == i).Total += lastSub.Price;
                }
                else if(subscriptions.LastOrDefault(s => s.CreateRecordDate.Year < DateTime.Now.Year) != null)
                {
                    lastSub = subscriptions.LastOrDefault(s => s.CreateRecordDate.Year < DateTime.Now.Year)
                        .SubscriptionPlan;
                    total += lastSub.Price;
                    graph.SingleOrDefault(s => s.Month == i).Total += lastSub.Price;
                }
            }
        }

        return (total, graph);
    }

    public class EarningYearGroupedByMonthDto
    {
        public EarningYearGroupedByMonthDto(int month, decimal total)
        {
            Month = month;
            Total = total;
        }

        public int Month { get; set; }
        public decimal Total { get; set; }
    }

    public List<MediaOverviewDto> GetMediaUploadedByMonth(int month)
    {
        var res = _mediaService.GetMediaNumberByMonthGroupByDay(month, DateTime.Now.Year);
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

        return mediaOverview;
    }

    public List<CreatorOverviewDto> GetCreatorsSubscribedByMonth(int month)
    {
        var now = DateTime.Now;
        var resCreator = _accountService.GetCreatorNumberByMonthGroupByDay(month ,now.Year);
        var creatorOverview = new List<CreatorOverviewDto>();
        
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

        return creatorOverview;
    }
}