using System;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Veesy.Domain.Constants;
using Veesy.Domain.Data;
using Veesy.Media.Constants;
using Veesy.Media.Utils;
using Veesy.Service.Implementation;

namespace Veesy.Functions.TimeTriggered;

public static class EditMediaResolutionOnAzure
{
    [Function("EditMediaResolutionOnAzure")]
    public static async Task Run([TimerTrigger("0 */30 * * * *", RunOnStartup = false)] FunctionContext context)
    {
        try
        {
            var logger = context.GetLogger("EditMediaResolutionOnAzure");
            var optionsBuilderMainDb = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilderMainDb.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=VeesyUploadMedia;Trusted_Connection=True;MultipleActiveResultSets=true;");
            var mainDbContext = new ApplicationDbContext(optionsBuilderMainDb.Options);
                
            var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=swirkeydevelopas;AccountKey=MD8x0U32UR5Njtdct4ytlRuj5r9+ZAZvnP22w3JPmvpW3hRk2U7/PLyGrdLI7yLzTazcF5G+kK4G+AStbcHLrw==;EndpointSuffix=core.windows.net");
            var veesyBlobService = new VeesyBlobService(blobServiceClient, "blobveesy");
            var mediaHandler = new MediaHandler(veesyBlobService, mainDbContext);
            var medias = mainDbContext.TmpMedias.Where(s => s.Status == 1).ToList();
            //TODO: Put media in status 2
            foreach (var tmp in medias)
            {
                var media = await veesyBlobService.GetBlobAsync($"OriginalMedia/{tmp.FileName}");
                var extension = Path.GetExtension(tmp.FileName);
                if (MediaCostants.videoExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    
                }
                else if (MediaCostants.imageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    var compressedImage = MediaCompressor.CompressImage(media.BlobContent);
                    var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{media.BlobContentType}";
                    await veesyBlobService.UploadFromStreamBlobAsync(compressedImage,
                        $"{VeesyConstants.BlobMediaSections.OriginalMedia}/{newFileName}", media.BlobContentType);
                    //TODO: content.Position must be less than content.Length. Please set content.Position to the start of the data to upload.
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}