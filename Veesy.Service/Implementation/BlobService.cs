using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _blobContainerClient;

    public BlobService(BlobServiceClient blobServiceClient, string blobContainerName)
    {
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName ?? "");
    }
    
    public async Task<BlobElement> GetBlobAsync(string name)
    {
        var blobClient = _blobContainerClient.GetBlobClient(name);
        var downloadBlobInfo = await blobClient.DownloadAsync();
        return new BlobElement(downloadBlobInfo.Value.Content, downloadBlobInfo.Value.ContentType);
    }
    
    public async Task UploadContentBlobAsync(string content, string fileName, string contentType)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        var bytes = Encoding.UTF8.GetBytes(content);
        await using var memoryString = new MemoryStream(bytes);
        await blobClient.UploadAsync(memoryString, new BlobHttpHeaders() {ContentType = contentType/*fileName.GetContentType()*/});
    }

    public async Task UploadFromStreamBlobAsync(Stream streamContent, string fileName, string contentType)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(streamContent, new BlobHttpHeaders() {ContentType = contentType});
    }

    public async Task DeleteBlobAsync(string fileName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task DownloadAndSaveToFile(string localFilePath, string fileName)
    {
        FileStream fileStream = File.OpenWrite(localFilePath);
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.DownloadToAsync(fileStream);
        fileStream.Close();
    }
}

public class VeesyBlobService : BlobService
{
    public VeesyBlobService(BlobServiceClient blobServiceClient, string blobContainerName) : base(blobServiceClient, blobContainerName)
    {
    }
}

public class BlobElement
{
    public Stream BlobContent { get; }
    public string BlobContentType { get; }
    public BlobElement(Stream blobContent, string blobContentType)
    {
        BlobContent = blobContent;
        BlobContentType = blobContentType;
    }
}