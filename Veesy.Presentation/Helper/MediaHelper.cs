using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Veesy.Presentation.Model.Media;
using Microsoft.Net.Http.Headers;
using Veesy.Domain.Exception;
using Veesy.Media.Constants;
using Veesy.Media.Utils;
using Veesy.Service.Implementation;

namespace Veesy.Presentation.Helper;

public class MediaHelper
{
    private readonly IConfiguration _config;
    private readonly MediaHandler _mediaHandler;

    public MediaHelper(IConfiguration config, MediaHandler mediaHandler)
    {
        _config = config;
        _mediaHandler = mediaHandler;
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
                await _mediaHandler.SaveFileAsStreamAsync(fileSection.FileStream, $"{MediaCostants.BlobProfileImageDirectory}/{fileName}", contentType);
                return (new ResultDto(true, ""), fileSection.FileName, fileName);
            }
            section = await multipartReader.ReadNextSectionAsync();
        }
        return (new ResultDto(false, "Error during upload image. Please retry."), "", "");
    }


    public async Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType)
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
                await _mediaHandler.SaveFileAsByteArrayRecordAsync(fileSection);
                //totalSizeInBytes += await _mediaHandler.SaveFileAsync(fileSection, filePaths, notUploadedFiles);
                //await Task.Run(() => _mediaHandler.SaveFileInBackground(fileSection, notUploadedFiles));
                fileCount++;
                //Try to compress image. This method will be called at runtime by a function to compress image.
                /*MediaCompressor.CompressImage(
                    $"C:\\Users\\lore9\\Desktop\\ENIGMA\\SourceCode\\Veesy\\Veesy.WebApp\\FilesUploaded\\Compressed\\{fileSection.FileName}",
                    $"C:\\Users\\lore9\\Desktop\\ENIGMA\\SourceCode\\Veesy\\Veesy.WebApp\\FilesUploaded\\{fileSection.FileName}");*/
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