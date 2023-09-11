using Microsoft.AspNetCore.WebUtilities;
using Veesy.Domain.Constants;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Service.Implementation;
using System.Linq;
using Veesy.Media.Constants;

namespace Veesy.Media.Utils;

public class MediaHandler
{
    private const string UploadsSubDirectory = "FilesUploaded";
    private readonly IEnumerable<string> allowedExtensions = new List<string> { ".zip", ".bin", ".png", ".mp4", ".jpg", ".jpeg" };
    private readonly VeesyBlobService _veesyBlobService;
    private readonly ApplicationDbContext _dbContext;

    public MediaHandler(VeesyBlobService veesyBlobService, ApplicationDbContext dbContext)
    {
        _veesyBlobService = veesyBlobService;
        _dbContext = dbContext;
    }

    public async Task SaveFileAsStreamAsync(Stream stream, string fileName, string contentType)
    {
        await _veesyBlobService.UploadFromStreamBlobAsync(stream, fileName, contentType);
    }


    public async Task<long> SaveFileAsync(FileMultipartSection fileSection, IList<string> filePaths, IList<string> notUploadedFiles)
    {
        var extension = Path.GetExtension(fileSection.FileName);
        if (!allowedExtensions.Contains(extension))
        {
            notUploadedFiles.Add(fileSection.FileName);
            return 0;
        }
        var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{extension}";
        
        /*Save File on Azure Blob*/
        await _veesyBlobService.UploadFromStreamBlobAsync(fileSection.FileStream,
            $"{VeesyConstants.BlobMediaSections.OriginalMedia}/{newFileName}", newFileName.GetContentType());
        
        /*Save File in Folder*/
        Directory.CreateDirectory(UploadsSubDirectory);

        var filePath = Path.Combine(UploadsSubDirectory, fileSection?.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        await fileSection.FileStream?.CopyToAsync(stream);

        filePaths.Add(MediaInfo.GetFullFilePath(fileSection));

        return fileSection.FileStream.Length;
    }

    public async Task<BlobElement> GetMediaFromBlob(string section, string fileName)
    {
        return await _veesyBlobService.GetBlobAsync($"{section}/{fileName}");
    }

    public async Task SaveFileAsByteArrayRecordAsync(FileMultipartSection fileSection, MyUser user)
    {
        try
        {
            var now = DateTime.Now;
            byte[] bytes;
            var size = (0, 0);
            var extension = Path.GetExtension(fileSection.FileName);
            using (var memoryStream = new MemoryStream())
            {
                await fileSection.FileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
                if (MediaCostants.videoExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    size = MediaInfo.GetVideoSizeFromStream(memoryStream);
                else
                    size = MediaInfo.GetVideoSizeFromStream(memoryStream);
            }
            var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{extension}";
            _dbContext.TmpMedias.Add(new TmpMedia()
            {
                FileName = newFileName,
                MediaBase64 = bytes,
                Type = extension,
                Media = new Domain.Models.Media()
                {
                    CreateRecordDate = now,
                    LastEditUserId = user.Id,
                    LastEditRecordDate = now,
                    CreateUserId = user.Id,
                    MyUserId = user.Id,
                    Type = extension,
                    OriginalFileName = fileSection.FileName,
                    FileName = newFileName,
                    IpAddress = "",
                    Width = size.Item1,
                    Height = size.Item2,
                    Size = bytes.Length,
                    Status = 0
                }
            });
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<bool> SaveFileOnAzureFromByteArray(TmpMedia mediaToUpload)
    {
        MemoryStream stream = new MemoryStream(mediaToUpload.MediaBase64);
        await _veesyBlobService.UploadFromStreamBlobAsync(stream,
            $"{VeesyConstants.BlobMediaSections.OriginalMedia}/{mediaToUpload.FileName}", mediaToUpload.FileName.GetContentType());
        return true;
    }
}