using System.Drawing;
using System.Net;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
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

    public PublicHelper(IMediaService mediaService, IAccountService accountService, IConfiguration config)
    {
        _mediaService = mediaService;
        _accountService = accountService;
        _config = config;
    }

    public async Task<AboutMediaViewModel> GetAboutInfo()
    {
        var MediaDtos = MapCloudDtos.MapMediaList(_mediaService.GetRandomPhotos(6));
        
        var vm = new AboutMediaViewModel();
        var BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/";

        string immagineOrizzontale = string.Empty;
        double rapportoMassimo = 0.0;

        foreach (var media in MediaDtos)
        {
            string imageUrl = $"{BasePathImages}{media.FileName}";
            string imageUrlReduced = imageUrl + "?tr=w-50";
            (int larghezza, int altezza) = await GetImageDimensionsAsync(imageUrlReduced);

            double rapporto = (double)larghezza / (double)altezza;

            if (rapporto > rapportoMassimo)
            {
                rapportoMassimo = rapporto;
                immagineOrizzontale = imageUrl;
            }
        }

        vm.InitialImageUrl = immagineOrizzontale;
    
        return vm;
    }
    
    public CreatorsViewModel GetCreatorsViewModel()
    {
        List<MyUser> userInfo = _accountService.GetAllCreators().ToList();
        List<MyUserCategoryWork> categoryWorks = new List<MyUserCategoryWork>();

        foreach (var user in userInfo)
        {
            categoryWorks.AddRange(user.MyUserCategoriesWork);
        }
        
        return new CreatorsViewModel()
        {
            User = userInfo,
            ApplicationUrl = _config["ApplicationUrl"],
            PortfolioId =userInfo.Select(p => p.Portfolios[0].Id).ToList(),
            CategoryWorks = categoryWorks.DistinctBy(s => s.CategoryWork.Name).Select(category => category.CategoryWork.Name).ToList(),
            BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/"
        };
    }
    
    public List<string> GetCreatorsFiltered(List<string> category)
    {

        var initialResults = _accountService.GetFilteredCreators();

        var usersWithAllCategories = initialResults
            .Where(u => category.All(category => u.MyUserCategoriesWork.Any(cw => cw.CategoryWork.Name == category)))
            .ToList();
        
        return usersWithAllCategories.Select(info => info.Id).ToList();
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

        var vm = new SplashViewModel()
        {
            MediaDtos = MapCloudDtos.MapMediaList(_mediaService.GetRandomPhotos(16)),
            BasePathImages = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            BasePathAzure = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/"
        };
        return vm;
    }
    
    public static async Task<(int width, int height)> GetImageDimensionsAsync(string imageUrl)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead))
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (Image image = Image.FromStream(stream, false, false))
            {
                int width = image.Width;
                int height = image.Height;
            
                return (width, height);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero delle dimensioni dell'immagine: {ex.Message}");
            throw;
        }
    }
}