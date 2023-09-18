using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Veesy.Presentation.Model.Media;
using Microsoft.Net.Http.Headers;
using Veesy.Domain.Data;
using Veesy.Domain.Exception;
using Veesy.Domain.Models;
using Veesy.Media.Constants;
using Veesy.Media.Utils;
using Veesy.Service.Implementation;

namespace Veesy.Presentation.Helper;

public class MediaHelper
{
    private readonly IConfiguration _config;
    private readonly MediaHandler _mediaHandler;
    private readonly ApplicationDbContext _dbContext;

    public MediaHelper(IConfiguration config, MediaHandler mediaHandler, ApplicationDbContext dbContext)
    {
        _config = config;
        _mediaHandler = mediaHandler;
        _dbContext = dbContext;
    }

    public async Task<(ResultDto resultDto, string originalFilename, string newFileName)> UploadProfileImageOnAzure(Stream fileStream, string contentType)
    {
        var boundary = MediaInfo.GetBoundary(MediaTypeHeaderValue.Parse(contentType));
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
                await _mediaHandler.SaveFileAsStreamAsync(fileSection.FileStream, $"{MediaCostants.BlobMediaSections.ProfileMedia}/{fileName}", contentType);
                return (new ResultDto(true, ""), fileSection.FileName, fileName);
            }
            section = await multipartReader.ReadNextSectionAsync();
        }
        return (new ResultDto(false, "Error during upload image. Please retry."), "", "");
    }


    public async Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType, MyUser user)
    {
        var fileCount = 0; 
        long totalSizeInBytes = 0;
        var boundary = MediaInfo.GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var multipartReader = new MultipartReader(boundary, fileStream);
        var section = await multipartReader.ReadNextSectionAsync();
        var filePaths = new List<string>();
        var notUploadedFiles = new List<string>();
    
        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                var length = 12354;
                var extension = Path.GetExtension(fileSection.FileName);
                var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{extension}";
                await _mediaHandler.SaveFileAsStreamAsync(fileSection.FileStream, $"{MediaCostants.BlobMediaSections.OriginalMedia}/{newFileName}", contentType);
                try
                {
                    var now = DateTime.Now;
                    var size = (0, 0);
                    _dbContext.Medias.Add(new Domain.Models.Media()
                    {
                        FileName = newFileName,
                        OriginalFileName = fileSection.FileName,
                        Type = extension,
                        Width = size.Item1,
                        Height = size.Item2,
                        MyUserId = user.Id,
                        Size = length,
                        Status = 2,
                        CreateUserId = user.Id,
                        LastEditRecordDate = now,
                        LastEditUserId = user.Id,
                        CreateRecordDate = now,
                        UploadDate = now
                    });
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
                fileCount++;
            }
            section = await multipartReader.ReadNextSectionAsync();
        }
        return new FileUploadSummary
        {
            TotalFilesUploaded = fileCount,
            TotalSizeUploaded = MediaInfo.ConvertSizeToString(totalSizeInBytes),
            FilePaths = filePaths,
            NotUploadedFiles = notUploadedFiles
        };
    }

    public async Task<BlobElement> GetMediaFromBlob(string section, string fileName)
    {
        return await _mediaHandler.GetMediaFromBlob(section, fileName);
    }
}