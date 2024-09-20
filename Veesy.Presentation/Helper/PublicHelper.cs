using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Email;
using Veesy.Presentation.Model.Cloud;
using Veesy.Presentation.Model.Public;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class PublicHelper
{
    private readonly IMediaService _mediaService;
    private readonly IAccountService _accountService;
    private readonly IConfiguration _config;
    private readonly IPortfolioService _portfolioService;
    private readonly IEmailSender _emailSender;
    private readonly IAnalyticService _analyticService;
    private readonly UserManager<MyUser> _userManager;

    public PublicHelper(IMediaService mediaService, IAccountService accountService, IConfiguration config, IPortfolioService portfolioService, IEmailSender emailSender, IAnalyticService analyticService, UserManager<MyUser> userManager)
    {
        _mediaService = mediaService;
        _accountService = accountService;
        _config = config;
        _portfolioService = portfolioService;
        _emailSender = emailSender;
        _analyticService = analyticService;
        _userManager = userManager;
    }

    public async Task<AboutMediaViewModel> GetAboutInfo()
    {
        var vm = new AboutMediaViewModel();
        var BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/";
        
        var imageList = new Dictionary<string, string>
        {
            {"c077aa3a3c5e4d4c85d5003881830b00a.jpg", "Umberto gnocchi"},
            {"9442e5d989d6435ba6ce87d60d7ea3a5a.jpg", "Umberto Gnocchi"},
            {"3e4ada1584b24b1b9573fe9335a229f9a.jpg", "Daniele Gay"},
            {"d17ecc4058924974a037cd56d5755f6aa.png", "Lorenzo Leo"},
            {"86a30aa71c6d4158a2e210415f85a698a.jpg", "Umberto Ponzecchi"},
            {"86f636e294a747949e8e519c3d3f8bcca.png", "Umberto Gnocchi"},
            {"44b0588e631a4a9e9fef856e193dad95a.png", "Umberto Gnocchi"},
            {"0de6ca7daf5f49a09d1b5a16d74adf92a.jpg", "Fabrizio Agostino Arienti"},
            {"7c1662e674004a729e86fd3e98fe7e4fa.png", "Francesco Schito"},
            {"42cd22f18dbb44bc97cf2996ed5376b4a.jpg", "Niccolò Brovelli"},
            {"859316e749784e498546b221cb2bcf9aa.jpg", "Alessio Vittori"},
            {"85f8596d19374f0ba4b68c875086a3b5a.jpg", "Niccolò Brovelli"},
            {"b23e0e8c441645cca4b88f930365fe5ea.jpg", "Elena Fisogni"},
            {"9c9b7f35e0de474da510e3cf3b9b11a8a.jpg", "Daniele Fabbri"},
            {"605b1b92ca184b07aa09160df7f07419a.jpg", "Luigi Gioia"},
            {"7475fe8680544df1991ef1148c220a02a.jpg", "Niccolò Brovelli"},
            {"24ab0b8e429a4135af1ed804b7aad886a.jpg", "Raffaele Pallota"},
            {"236abe662fac4adab52563e233316ed1a.png", "Laca Maldera"},
            {"5d52857c28d5411493c45ce9a579575fa.png", "Lorenzo Leo"},
            {"8b2bd54f6cac433d895447f5a9f524c6a.png", "Francesco Schito"},
            {"36df68c0cac746a09df5d5bcbf5e6229a.jpg", "Alessio Vittori"},
        };

        var random = new Random();
        var randomIndex = random.Next(imageList.Count);
        var randomPair = imageList.ElementAt(randomIndex);

        var imageName = randomPair.Key;
        var author = randomPair.Value;

        var imageUrl = $"{BasePathImages}{imageName}";
        
        vm.InitialImageUrl = imageUrl;
        vm.InitialAuthor = author;
        
        return vm;
    }
    
    public CreatorsViewModel GetCreatorsViewModel()
    {
        List<MyUser> userInfo = _accountService.GetAllVisibleCreators().ToList();
        List<MyUserCategoryWork> categoryWorks = new List<MyUserCategoryWork>();

        foreach (var user in userInfo)
        {
            categoryWorks.AddRange(user.MyUserCategoriesWork);
        }
        
        return new CreatorsViewModel()
        {
            User = userInfo,
            ApplicationUrl = _config["ApplicationUrl"],
            PortfolioId = userInfo.Select(p => p.Portfolios[0].Id).ToList(),
            Portfolios = userInfo.Select(p => p.Portfolios[0]).ToList(),
            CategoryWorks = categoryWorks.DistinctBy(s => s.CategoryWork.Name).Select(category => category.CategoryWork.Name).ToList(),
            BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/"
        };
    }
    
    public List<string> GetCreatorsFiltered(List<string> category)
    {

        var initialResults = _accountService.GetFilteredCreatorsToShow(category);
        return initialResults.Select(info => info.Id).ToList();
    }

    public SubscritionPlanViewModel GetSubscritionPlanViewModel(MyUser userInfo)
    {
        var vm = new SubscritionPlanViewModel()
        {
            Subscription = userInfo == null ? null : _accountService.GetUserSubscriptionPlan(userInfo.Id)
        };
        return vm;
    }

    public GalleryViewModel GetGalleryViewModel()
    {

        var vm = new GalleryViewModel()
        {
            MediaGalleryDtos = MapCloudDtos.MapMediaGalleryList(_mediaService.GetRandomMedia(50)),
            BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            BasePathAzure = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/",
            BasePathCode = _config["ApplicationUrl"]
        };
        return vm;
    }
    
    public SplashViewModel GetSplashViewModel()
    {

        var mediasDto = MapCloudDtos.MapMediaList(_mediaService.GetRandomPhotos(18));
        
        foreach (var media in mediasDto)
        {
            if (media.NestedPortfolioLinks != null && media.NestedPortfolioLinks != Guid.Empty)
            {
                media.NestedPortfolioNameForUrl = _portfolioService
                    .GetPortfolioById(media.NestedPortfolioLinks.Value, media.UserId).Name.ToLower().Replace(" ", "-").Replace("/", "-");
            }
        }
        
        var vm = new SplashViewModel()
        {
            MediaDtos = mediasDto,
            BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            BasePathAzure = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/"
        };
        return vm;
    }

    public async Task<ResultDto> SendCreatorForm(CreatorFormDto dto, MyUser userInfo, string? testEmail)
    {
        if (dto.SenderEmail.IsNullOrEmpty())
            return new ResultDto(false, "Please insert email");
        if (dto.SenderName.IsNullOrEmpty())
            return new ResultDto(false, "Please insert name");
        if (dto.Message.IsNullOrEmpty())
            return new ResultDto(false, "Please insert message");
        if (!dto.Policy)
            return new ResultDto(false, "Please accept the Privacy Policy");
        
        var emailToSend = "";
        var recipient = new MyUser();
        if (string.IsNullOrEmpty(testEmail))
        {
            recipient = _accountService.GetUserById(dto.Recipient);
            if(recipient == null)
                return new ResultDto(false, "User not found");
            emailToSend = recipient.Email;
        }
        else
        {
            recipient = await _userManager.FindByEmailAsync(testEmail);
            if (recipient == null)
            {
                recipient = await _userManager.FindByNameAsync(testEmail);
                if (recipient == null)
                    return new ResultDto(false, "User not found");
            }
            if(recipient == null)
                return new ResultDto(false, "User not found");
        }
        
        var link = "";
        var message = new Message(new (string, string)[] { ("Noreply | Veesy", emailToSend) }, "You receive new message from: " + dto.SenderName, link);
        List<(string, string)> replacer = new List<(string, string)> { ("[sender-email]", dto.SenderEmail),("[message]", dto.Message),("[sender-name]", dto.SenderName) };

        var currentPath = Directory.GetCurrentDirectory();
        await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/mail-creator-form.html", replacer);
        var form = new TrackingForm
        {
            EmailSender = dto.SenderEmail,
            NameSender = dto.SenderName,
            MyUserId = recipient.Id,
            FormType = VeesyConstants.FormType.CreatorType
        };
        await _analyticService.AddForm(form, userInfo);
        
        return new ResultDto(true, "Message sent correctly");
    }
}