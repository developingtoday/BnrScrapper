using System;
using BnrScrapperLogic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace ScrapTimerFunction
{
    public static class EuroScrapFunction
    {
        [FunctionName("EuroScrapFunction")]
        public static void Run([TimerTrigger("0 0/15 10-12  * * 1-5")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info("Starting execution");
                var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
                var manager = new RoborManager(connString, new AzureLogger(log));

                manager.DoMagicEuro(DateTime.Today, DateTime.Today).Wait();
                log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                log.Error("An error occurred", ex);
                throw;
            }
        }
    }
}
