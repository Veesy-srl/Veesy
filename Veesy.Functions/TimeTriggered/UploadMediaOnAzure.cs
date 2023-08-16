using System;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Veesy.Domain.Data;
using Veesy.Media.Utils;
using Veesy.Service.Implementation;

namespace Veesy.Functions.TimeTriggered;

public static class UploadMediaOnAzure
{
    [Function("UploadMediaOnAzure")]
    public static async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] MyInfo myTimer, Microsoft.Azure.WebJobs.ExecutionContext context)
    {
        try
        {

            var optionsBuilderMainDb = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilderMainDb.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=VeesyUploadMedia;Trusted_Connection=True;MultipleActiveResultSets=true;");
            var mainDbContext = new ApplicationDbContext(optionsBuilderMainDb.Options);
            
            var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=swirkeydevelopas;AccountKey=MD8x0U32UR5Njtdct4ytlRuj5r9+ZAZvnP22w3JPmvpW3hRk2U7/PLyGrdLI7yLzTazcF5G+kK4G+AStbcHLrw==;EndpointSuffix=core.windows.net");
            var veesyBlobService = new VeesyBlobService(blobServiceClient, "blobveesy");
            var mediaHandler = new MediaHandler(veesyBlobService, mainDbContext);

            var tmpMedia = mainDbContext.TmpMedias.Where(s => s.Status == 0).ToList();
            tmpMedia.ForEach(s => s.Status = 1);
            mainDbContext.UpdateRange(tmpMedia);
            mainDbContext.SaveChanges();
            foreach (var fileToUpload in tmpMedia)
            {
                if (await mediaHandler.SaveFileOnAzureFromByteArray(fileToUpload))
                {
                    mainDbContext.Medias.Add(new Domain.Models.Media()
                    {
                        Id = new Guid(),
                        CreateRecordDate = DateTime.Now,
                        CreateUserId = "1",
                        FileName = fileToUpload.FileName,
                        IpAddress = "10.10.10.10",
                        LastEditRecordDate = DateTime.Now,
                        LastEditUserId = "1",
                    });
                    mainDbContext.SaveChanges();
                }
                
            }
        }
        catch (Exception ex)
        {
        }
        
    }
}

public class MyInfo
{
    public MyScheduleStatus ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }
}

public class MyScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}