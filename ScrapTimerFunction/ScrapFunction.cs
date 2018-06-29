using System;
using System.Linq;
using System.Net.Http;
using BnrScrapperLogic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
namespace ScrapTimerFunction
{
    public static class ScrapFunction
    {
        private static HttpClient _client = new HttpClient();
        [FunctionName("BnrRoborFunction")]
        public static void Run([TimerTrigger("0 0/6 8-8  * * 1-5")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info("Starting execution");
                var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
                var manager = new RoborManager(connString, new AzureLogger(log));
                var result=manager.DoMagic(DateTime.Today, DateTime.Today).Result;
                var push=new PushNotification()
                {
                    Data = DateTime.Today,
                    EuroRonRate = (result.Item2.FirstOrDefault()??new EuroRonRate()).Valoare,
                    Robor3M = (result.Item1.FirstOrDefault() ?? new RoborHistoric()).Robor3M,
                    Robid3M = (result.Item1.FirstOrDefault() ?? new RoborHistoric()).Robid3M
                };
                var response = _client.PostAsJsonAsync(Environment.GetEnvironmentVariable("PushHttp"), push).Result;
                log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                log.Error("An error occurred",ex);
                throw;
            }
        }
    }
}
