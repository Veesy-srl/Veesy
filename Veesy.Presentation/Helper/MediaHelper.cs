using System.Collections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Data;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Service.Dtos;
using Veesy.Service.Implementation;
using Veesy.Service.Interfaces;
using Veesy.Validators;

namespace Veesy.Presentation.Helper;

public class MediaHelper
{
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _dbContext;
    private readonly VeesyBlobService _veesyBlobService;
    private readonly IMediaService _mediaService;
    private readonly MediaValidators _mediaValidators;
    private readonly ISubscriptionPlanService _subscriptionPlanService;
    private readonly IPortfolioService _portfolioService;
    private readonly UserManager<MyUser> _userManager;
    
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();
    private readonly IEnumerable<string> allowedExtensions = new List<string> { ".zip", ".bin", ".png", ".mp4", ".jpg", ".jpeg" };

    public MediaHelper(IConfiguration config, ApplicationDbContext dbContext, VeesyBlobService veesyBlobService, IMediaService mediaService, MediaValidators mediaValidators, ISubscriptionPlanService subscriptionPlanService, IPortfolioService portfolioService, UserManager<MyUser> userManager)
    {
        _config = config;
        _dbContext = dbContext;
        _veesyBlobService = veesyBlobService;
        _mediaService = mediaService;
        _mediaValidators = mediaValidators;
        _subscriptionPlanService = subscriptionPlanService;
        _portfolioService = portfolioService;
        _userManager = userManager;
    }

    public async Task<(ResultDto resultDto, string originalFilename, string newFileName)> UploadProfileImageOnAzure(Stream fileStream, string contentType)
    {
        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var multipartReader = new MultipartReader(boundary, fileStream);
        var section = await multipartReader.ReadNextSectionAsync();
        var fileName = Guid.NewGuid().ToString();

        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                var extension = Path.GetExtension(fileSection.FileName);
                fileName += extension;
                await _veesyBlobService.UploadFromStreamBlobAsync(fileSection.FileStream, $"{MediaCostants.BlobMediaSections.ProfileMedia}/{fileName}", contentType);
                return (new ResultDto(true, ""), fileSection.FileName, fileName);
            }
            section = await multipartReader.ReadNextSectionAsync();
        }
        return (new ResultDto(false, "Error during upload image. Please retry."), "", "");
    }


    public async Task<List<(bool success, MediaDto? media, string fileName, string message)>> UploadFileAsync(Stream fileStream, string contentType, MyUser user)
    {
        //Il numero di file che trovo nella section dipende dal limite che imposto a dropzone
        var mediaList = _mediaService.GetMediasNameAndSizeByUserId(user.Id);
        var numberMediaOnCloud = mediaList.Count; 
        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var multipartReader = new MultipartReader(boundary, fileStream);
        var section = await multipartReader.ReadNextSectionAsync();
        var filesUploadedStatus = new List<(bool success, MediaDto? media, string fileName, string message)>();
        var subscription = _subscriptionPlanService.GetSubscriptionByUserId(user.Id);
        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                //File extension validation
                var extension = Path.GetExtension(fileSection.FileName);
                var validateExtension = _mediaValidators.ValidateMediaExtension(extension);
                if (!validateExtension.Success)
                {
                    filesUploadedStatus.Add(new (false, null, fileSection.FileName, validateExtension.Message));
                    section = await multipartReader.ReadNextSectionAsync();
                    continue;
                }
                
                //Number media validation
                if (numberMediaOnCloud >= subscription.AllowedMediaNumber)
                {
                    filesUploadedStatus.Add(new(false, null, fileSection.FileName,
                        "Reached maximum subscription number media."));
                    section = await multipartReader.ReadNextSectionAsync();
                    continue;
                }
                else
                    numberMediaOnCloud ++;
                        
                //File size validation
                await using (Stream stream = new MemoryStream())
                {
                    await fileSection.FileStream.CopyToAsync(stream);
                    var size = stream.Length;
                    var tmpSize = _mediaService.GetSizeMediaStorageByUserId(user.Id) + size; //Value in byte
                    var validateSize = _mediaValidators.ValidateSizeUpload(size, tmpSize, subscription.AllowedMegaByte * 1024 * 1024, extension);
                    if (!validateSize.Success)
                    {
                        filesUploadedStatus.Add(new(false, null, fileSection.FileName, validateSize.Message));
                        section = await multipartReader.ReadNextSectionAsync();
                        continue;
                    }

                    if (mediaList.Contains((fileSection.FileName, size)))
                    {
                        filesUploadedStatus.Add(new(false, null, fileSection.FileName, "File is already in cloud."));
                        section = await multipartReader.ReadNextSectionAsync();
                        continue;
                    }

                    var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{extension}";
                    await _veesyBlobService.UploadFromStreamBlobAsync(stream, $"{MediaCostants.BlobMediaSections.OriginalMedia}/{newFileName}", contentType);
                    try
                    {
                        var result = await _mediaService.AddMedia(new Media()
                        {
                            FileName = newFileName,
                            OriginalFileName = fileSection.FileName,
                            Type = extension,
                            MyUserId = user.Id,
                            Size = size,
                        }, user);
                        filesUploadedStatus.Add(new(true, MapCloudDtos.MapMedia(result), fileSection.FileName, $"{fileSection.FileName} upload correctly."));
                    }
                    catch (Exception e)
                    {
                        //TODO: delete file from azure
                        throw e;
                    }
                }
            }
            section = await multipartReader.ReadNextSectionAsync();
        }

        return filesUploadedStatus;
    }

    public async Task<List<(bool success, MediaDto? media, string fileName, string message)>> UploadFileAsyncToPortfolio(Stream fileStream, string? contentType, Guid portfolioId, MyUser user)
    {
        //Il numero di file che trovo nella section dipende dal limite che imposto a dropzone
        var mediaList = _mediaService.GetMediasNameAndSizeByUserId(user.Id);
        var numberMediaOnCloud = mediaList.Count; 
        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var multipartReader = new MultipartReader(boundary, fileStream);
        var section = await multipartReader.ReadNextSectionAsync();
        var filesUploadedStatus = new List<(bool success, MediaDto? media, string fileName, string message)>();
        var subscription = _subscriptionPlanService.GetSubscriptionByUserId(user.Id);
        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                //File extension validation
                var extension = Path.GetExtension(fileSection.FileName);
                var validateExtension = _mediaValidators.ValidateMediaExtension(extension);
                if (!validateExtension.Success)
                {
                    filesUploadedStatus.Add(new (false, null, fileSection.FileName, validateExtension.Message));
                    section = await multipartReader.ReadNextSectionAsync();
                    continue;
                }
                
                //Number media validation
                if (numberMediaOnCloud >= subscription.AllowedMediaNumber)
                    filesUploadedStatus.Add(new(false, null, fileSection.FileName,
                        "Reached maximum subscription number media."));
                else
                    numberMediaOnCloud ++;
                
                //File size validation
                await using (Stream stream = new MemoryStream())
                {
                    await fileSection.FileStream.CopyToAsync(stream);
                    var size = stream.Length;
                    var tmpSize = _mediaService.GetSizeMediaStorageByUserId(user.Id) + size; //Value in byte
                    var validateSize = _mediaValidators.ValidateSizeUpload(size, tmpSize, subscription.AllowedMegaByte * 1024 * 1024, extension);
                    if (!validateSize.Success)
                    {
                        filesUploadedStatus.Add(new(false, null, fileSection.FileName, validateSize.Message));
                        section = await multipartReader.ReadNextSectionAsync();
                        continue;
                    }

                    if (mediaList.Contains((fileSection.FileName, size)))
                    {
                        filesUploadedStatus.Add(new(false, null, fileSection.FileName, "File is already in cloud."));
                        section = await multipartReader.ReadNextSectionAsync();
                        continue;
                    }

                    var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{extension}";
                    await _veesyBlobService.UploadFromStreamBlobAsync(stream, $"{MediaCostants.BlobMediaSections.OriginalMedia}/{newFileName}", contentType);
                    var portfolioToLink = _portfolioService.GetPortfolioByIdWithPortfoliosMedia(portfolioId, user.Id);
                    try
                    {
                        var result = await _mediaService.AddMedia(new Media()
                        {
                            FileName = newFileName,
                            OriginalFileName = fileSection.FileName,
                            Type = extension,
                            MyUserId = user.Id,
                            Size = size,
                            PortfolioMedias = new List<PortfolioMedia>
                            {
                                new(){ PortfolioId = portfolioId, SortOrder = portfolioToLink.PortfolioMedias.Count, Description = "", IsActive = true}
                            }
                        }, user);
                        filesUploadedStatus.Add(new(true, MapCloudDtos.MapMedia(result), fileSection.FileName, $"{fileSection.FileName} upload correctly."));
                    }
                    catch (Exception e)
                    {
                        //TODO: delete file from azure
                        throw e;
                    }
                }
            }
            section = await multipartReader.ReadNextSectionAsync();
        }

        return filesUploadedStatus;
    }

    public async Task<BlobElement> GetMediaFromBlob(string section, string fileName)
    {
        return await _veesyBlobService.GetBlobAsync($"{section}/{fileName}");
    }
    
    public async Task<List<(bool success, string filename, string message, string? code)>> DeleteFiles(List<Guid> imgToDelete, MyUser userInfo)
    {
        var result = new List<(bool, string, string, string?)>();
        
        var medias = _mediaService.GetMediasByIdWithPortfoliosMedia(imgToDelete);
        var portfolios = _portfolioService.GetPortfoliosByMedias(imgToDelete).ToList();
        var mediaToRemove = new List<Media>();
        var usersInRole = await _userManager.GetUsersInRoleAsync(Roles.Admin);
        var admin = usersInRole.Contains(userInfo);
        foreach (var media in medias)
        {
            try
            {
                if (!admin && media.MyUserId != userInfo.Id)
                {
                    result.Add(new(false, media.OriginalFileName, "Access not allowed.", null));
                    continue;
                }
                // riassegno i SortOrder alle immagini, mantenendo l'ordine originale
                foreach (var portfolio in portfolios.Where(s => s.PortfolioMedias.Select(s => s.MediaId).Contains(media.Id)))
                {
                    var mediaPortfolio = media.PortfolioMedias.SingleOrDefault(s => s.PortfolioId == portfolio.Id);
                    foreach (var portflioMedia in portfolio.PortfolioMedias)
                    {
                        if (portflioMedia.SortOrder > mediaPortfolio.SortOrder)
                            portflioMedia.SortOrder--;
                    }

                    // rimuovo i portfolioMedias coinvolti
                    portfolio.PortfolioMedias = portfolio.PortfolioMedias.Where(s => s.MediaId != mediaPortfolio.MediaId).ToList();
                }
                mediaToRemove.Add(media);
            }
            catch (Exception e)
            {
                result.Add(new(false, media.FileName, "Error deleting file. Please retry.", null));
            }
        }
        
        var res = await _mediaService.DeleteMediasAndUpdatePortfolios(mediaToRemove, portfolios, userInfo);
        if (!res.Success)
        {
            throw new Exception("Error deleting files.");
        }
        foreach (var media in mediaToRemove)
        {
            await _veesyBlobService.DeleteBlobAsync($"{MediaCostants.BlobMediaSections.OriginalMedia}/{media.FileName}");
            result.Add(new (true, media.OriginalFileName, "", media.Id.ToString()));
        }

        return result;
    }
    
    public async Task<(bool success, string filename, string message, string? code, Media? previousMedia)> DeleteFile(Guid imgToDelete, MyUser userInfo)
    {

        
        var media = _mediaService.GetMediaByIdWithPortfoliosMedia(imgToDelete);
        if (media == null || media.MyUserId != userInfo.Id)
        {
            return new(false, imgToDelete.ToString(), "File not found.", null, null);
        }
        
        var usersInRole = await _userManager.GetUsersInRoleAsync(Roles.Admin);
        var admin = usersInRole.Contains(userInfo);

        if (!admin && media.MyUserId != userInfo.Id)
        {
            return new(false, media.OriginalFileName, "Access not allowed.", null, null);
        }
        var portfolios = _portfolioService.GetPortfoliosByMedia(imgToDelete).ToList();
        foreach (var portfolio in portfolios)
        {
            var mediaPortfolio = media.PortfolioMedias.SingleOrDefault(s => s.PortfolioId == portfolio.Id);
            foreach (var portfolioMedia in portfolio.PortfolioMedias)
            {
                if (portfolioMedia.SortOrder > mediaPortfolio.SortOrder)
                    portfolioMedia.SortOrder--;
            }

            portfolio.PortfolioMedias = portfolio.PortfolioMedias.Where(s => s.MediaId != mediaPortfolio.MediaId).ToList();
        }
        var result = await _mediaService.DeleteMediaAndUpdatePortfolios(media, portfolios, userInfo);
        if (!result.Success)
            return new(false, media.OriginalFileName, "Error during deleting media.", null, null);
        await _veesyBlobService.DeleteBlobAsync($"{MediaCostants.BlobMediaSections.OriginalMedia}/{media.FileName}");
        var previousMedia = _mediaService.GetPreviousMediaByDate(media.CreateRecordDate, userInfo);
        
        return new (true, media.OriginalFileName, "", imgToDelete.ToString(), previousMedia);
    }

    #region Media Utils

    private static string GetBoundary(MediaTypeHeaderValue contentType)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        return boundary;
    }
    
    private static string ConvertSizeToString(long bytes)
    {
        var fileSize = new decimal(bytes);
        var kilobyte = new decimal(1024);
        var megabyte = new decimal(1024 * 1024);
        var gigabyte = new decimal(1024 * 1024 * 1024);

        return fileSize switch
        {
            _ when fileSize < kilobyte => "Less then 1KB",
            _ when fileSize < megabyte =>
                $"{Math.Round(fileSize / kilobyte, fileSize < 10 * kilobyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}KB",
            _ when fileSize < gigabyte =>
                $"{Math.Round(fileSize / megabyte, fileSize < 10 * megabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}MB",
            _ when fileSize >= gigabyte =>
                $"{Math.Round(fileSize / gigabyte, fileSize < 10 * gigabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}GB",
            _ => "n/a"
        };
    }
    
    private static string GetContentType(string fileName)
    {
        if (!Provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return contentType;
    }

    #endregion

}

public static class MediaUtils
{
    private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    public static string SizeSuffix(long value, int decimalPlaces = 1)
    {
        if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
        if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); } 
        if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

        // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
        int mag = (int)Math.Log(value, 1024);

        // 1L << (mag * 10) == 2 ^ (10 * mag) 
        // [i.e. the number of bytes in the unit corresponding to mag]
        decimal adjustedSize = (decimal)value / (1L << (mag * 10));

        // make adjustment when the value is large enough that
        // it would round up to 1000 or more
        if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}", 
            adjustedSize, 
            SizeSuffixes[mag]);
    } 
}