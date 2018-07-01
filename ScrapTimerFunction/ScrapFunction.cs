using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using BnrScrapperLogic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ScrapTimerFunction
{
    public static class ScrapFunction
    {
       
        [FunctionName("BnrRoborFunction")]
        public static void Run([TimerTrigger("0 0/6 8-8  * * 1-5")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info("Starting execution");
                var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
                var manager = new RoborManager(connString, new AzureLogger(log));
                var robors = manager.GetRobors(DateTime.Today, DateTime.Today);
                log.Info($"Querying db on current date befor running scrapper, return {robors.Count}");
                var result=manager.DoMagic(DateTime.Today, DateTime.Today).Result;
                var push=new PushNotification()
                {
                    Data = DateTime.Today.ToString("d"),
                    Robor3M = (result.FirstOrDefault() ?? new RoborHistoric()).Robor3M,
                    Robid3M = (result.FirstOrDefault() ?? new RoborHistoric()).Robid3M
                };
                var sentPush = push.Robid3M != 0 && push.Robor3M != 0;
                log.Info($"Push notification {push}");
                var json = JsonConvert.SerializeObject(push,Formatting.None,new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                if (sentPush)
                {
                    if (!robors.Any())
                    {
                        log.Info($"Push notification json {json}");
                        using (var client = new HttpClient())
                        {
                            var jmecherie = client.PostAsync(Environment.GetEnvironmentVariable("PushHttp"), new StringContent(json, Encoding.UTF8, "application/json")).Result;
                        }
                    }
                }

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
