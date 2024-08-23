using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Veesy.Domain.Data;

namespace Veesy.Functions.TimeTriggered;

public static class RemoveOldUser
{
    [FunctionName("RemoveOldUser")]
    public static async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ExecutionContext executionContext)
    {
        try
        {
            var configReader = new ConfigReader(executionContext);
            var optionsBuilderMainDb = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilderMainDb.UseSqlServer(configReader.GetMainConnectionString());
            var dbContext = new ApplicationDbContext(optionsBuilderMainDb.Options);
            //
            // _accountService = new AccountService(new VeesyUoW(dbContext));
            //
            // var users = _accountService.GetUserEmailNotConfirmed(30);
            // Console.WriteLine("Prova");
            // await _accountService.DeleteUsers(users);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}