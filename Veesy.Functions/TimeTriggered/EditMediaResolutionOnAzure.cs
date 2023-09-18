using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Constants;
using Veesy.Domain.Data;
using Veesy.Media.Constants;
using Veesy.Media.Utils;
using Veesy.Service.Implementation;
using MediaFormat = Veesy.Domain.Models.MediaFormat;

namespace Veesy.Functions.TimeTriggered;

public static class EditMediaResolutionOnAzure
{
    [Function("EditMediaResolutionOnAzure")]
    public static async Task Run([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] FunctionContext context)
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
            var medias = mainDbContext.Medias.Where(s => s.Status == 1).ToList();
            var formats = mainDbContext.Formats.ToList();
            //TODO: mettere il media subito in status 2 e se non va bene riportarlo in status 1
            foreach (var media in medias)
            {
                var mediaToCompress = await veesyBlobService.GetBlobAsync($"OriginalMedia/{media.FileName}");
                if (MediaCostants.videoExtensions.Contains(media.Type, StringComparer.OrdinalIgnoreCase))
                {
                    if (media.MediaFormats == null)
                        media.MediaFormats = new List<MediaFormat>();    
                    foreach (var format in formats.Where(s => s.Type == "Video"))
                    {
                        var compressedVideo = MediaCompressor.CompressVideo(mediaToCompress.BlobContent, format, media);
                        var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{media.Type}";
                        await veesyBlobService.UploadFromStreamBlobAsync(compressedVideo,
                            $"{MediaCostants.BlobMediaSections.CompressedMedia}/{newFileName}", mediaToCompress.BlobContentType);
                        media.MediaFormats.Add(new MediaFormat()
                        {
                            Format = format,
                            Media = media,
                            FileName = newFileName,
                            //Size = recuperare la nuova dimensione 
                        });
                        if(formats.Last() != format)
                            mediaToCompress = await veesyBlobService.GetBlobAsync($"OriginalMedia/{media.FileName}");
                    }
                    media.Status = 2;
                    mainDbContext.Medias.Update(media);
                    await mainDbContext.SaveChangesAsync();
                }
                else if (MediaCostants.imageExtensions.Contains(media.Type, StringComparer.OrdinalIgnoreCase))
                {
                    if (media.MediaFormats == null)
                        media.MediaFormats = new List<MediaFormat>();    
                    foreach (var format in formats.Where(s => s.Type == "Image"))
                    {
                        var compressedImage = MediaCompressor.CompressImage(mediaToCompress.BlobContent, format, media);
                        var newFileName = $"{Guid.NewGuid().ToString().Replace("-", String.Empty)}{media.Type}";
                        await veesyBlobService.UploadFromStreamBlobAsync(compressedImage,
                            $"{MediaCostants.BlobMediaSections.CompressedMedia}/{newFileName}", mediaToCompress.BlobContentType);
                        media.MediaFormats.Add(new MediaFormat()
                        {
                            Format = format,
                            Media = media,
                            FileName = newFileName,
                            //Size = recuperare la nuova dimensione 
                        });
                        if(formats.Last() != format)
                            mediaToCompress = await veesyBlobService.GetBlobAsync($"OriginalMedia/{media.FileName}");
                    }
                    media.Status = 2;
                    mainDbContext.Medias.Update(media);
                    await mainDbContext.SaveChangesAsync();
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