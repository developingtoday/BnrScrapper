using System;
using BnrScrapperLogic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
namespace ScrapTimerFunction
{
    public static class ScrapFunction
    {
        [FunctionName("BnrRoborFunction")]
        public static void Run([TimerTrigger("0 0 5-12 * * 1-5")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info("Starting execution");
            var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            var manager = new RoborManager(connString,new AzureLogger(log));
             
             manager.DoMagic(DateTime.Today, DateTime.Today).Wait();
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
