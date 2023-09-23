using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Veesy.Presentation.Model.Media;
using Microsoft.Net.Http.Headers;
using Veesy.Domain.Constants;
using Veesy.Domain.Data;
using Veesy.Domain.Exception;
using Veesy.Domain.Models;
using Veesy.Service.Implementation;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class MediaHelper
{
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _dbContext;
    private readonly VeesyBlobService _veesyBlobService;
    private readonly IMediaService _mediaService;
    
    private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();
    private readonly IEnumerable<string> allowedExtensions = new List<string> { ".zip", ".bin", ".png", ".mp4", ".jpg", ".jpeg" };

    public MediaHelper(IConfiguration config, ApplicationDbContext dbContext, VeesyBlobService veesyBlobService, IMediaService mediaService)
    {
        _config = config;
        _dbContext = dbContext;
        _veesyBlobService = veesyBlobService;
        _mediaService = mediaService;
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


    public async Task<ResultDto> UploadFileAsync(Stream fileStream, long? mediaSize, string contentType, MyUser user)
    {
        //Il numero di file che trovo nella section dipende dal limite che imposto a dropzone
        //TODO: fare validazione
        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var multipartReader = new MultipartReader(boundary, fileStream);
        var section = await multipartReader.ReadNextSectionAsync();
        var result = new ResultDto(false, "No files uploaded. Please retry");
        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                var extension = Path.GetExtension(fileSection.FileName);
                var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{extension}";
                await _veesyBlobService.UploadFromStreamBlobAsync(fileSection.FileStream, $"{MediaCostants.BlobMediaSections.OriginalMedia}/{newFileName}", contentType);
                try
                {
                    await _mediaService.AddMedia(new Media()
                    {
                        FileName = newFileName,
                        OriginalFileName = fileSection.FileName,
                        Type = extension,
                        MyUserId = user.Id,
                        Size = mediaSize.Value,
                    }, user);
                    result = new ResultDto(true, $"{fileSection.FileName} upload correctly.");
                }
                catch (Exception e)
                {
                    //TODO: delete file from azure
                    throw e;
                }
            }
            section = await multipartReader.ReadNextSectionAsync();
        }

        return result;
    }

    public async Task<BlobElement> GetMediaFromBlob(string section, string fileName)
    {
        return await _veesyBlobService.GetBlobAsync($"{section}/{fileName}");
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