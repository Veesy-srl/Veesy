using Microsoft.AspNetCore.WebUtilities;
using Veesy.Domain.Constants;
using Veesy.Service.Implementation;

namespace Veesy.Media.Utils;

public class MediaHandler
{
    private const string UploadsSubDirectory = "FilesUploaded";
    private readonly IEnumerable<string> allowedExtensions = new List<string> { ".zip", ".bin", ".png", ".mp4", ".jpg", ".jpeg" };
    private readonly VeesyBlobService _veesyBlobService;

    public MediaHandler(VeesyBlobService veesyBlobService)
    {
        _veesyBlobService = veesyBlobService;
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
    
    
    public async void SaveFileInBackground(FileMultipartSection fileSection, IList<string> notUploadedFiles)
    {
        try
        {
            var files = fileSection;
            var extension = Path.GetExtension(fileSection.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                notUploadedFiles.Add(fileSection.FileName);
                return;
            }
            var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{extension}";
            await _veesyBlobService.UploadFromStreamBlobAsync(files.FileStream,
                $"{VeesyConstants.BlobMediaSections.OriginalMedia}/{newFileName}", newFileName.GetContentType());
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<BlobElement> GetMediaFromBlob(string section, string fileName)
    {
        return await _veesyBlobService.GetBlobAsync($"{section}/{fileName}");
    }
}