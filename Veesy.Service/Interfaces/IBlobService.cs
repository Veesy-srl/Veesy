using Veesy.Service.Implementation;

namespace Veesy.Service.Interfaces;

public interface IBlobService
{
    Task<BlobElement> GetBlobAsync(string name);
    Task UploadContentBlobAsync(string content, string fileName, string contentType);
    Task UploadFromStreamBlobAsync(Stream streamContent, string fileName, string contentType);
    Task DeleteBlobAsync(string fileName);
    Task DownloadAndSaveToFile(string localFilePath, string filename);
}