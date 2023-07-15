using Microsoft.AspNetCore.WebUtilities;

namespace Veesy.Media.Utils;

public class MediaHandler
{
    private const string UploadsSubDirectory = "FilesUploaded";
    private readonly IEnumerable<string> allowedExtensions = new List<string> { ".zip", ".bin", ".png", ".mp4", ".jpg", ".jpeg" };
    
    public async Task<long> SaveFileAsync(FileMultipartSection fileSection, IList<string> filePaths, IList<string> notUploadedFiles)
    {
        var extension = Path.GetExtension(fileSection.FileName);
        if (!allowedExtensions.Contains(extension))
        {
            notUploadedFiles.Add(fileSection.FileName);
            return 0;
        }

        Directory.CreateDirectory(UploadsSubDirectory);

        var filePath = Path.Combine(UploadsSubDirectory, fileSection?.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        await fileSection.FileStream?.CopyToAsync(stream);

        filePaths.Add(MediaInfo.GetFullFilePath(fileSection));

        return fileSection.FileStream.Length;
    }
}