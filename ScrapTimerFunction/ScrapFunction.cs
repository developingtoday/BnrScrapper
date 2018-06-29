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
                var result=manager.DoMagic(DateTime.Today, DateTime.Today).Result;
                var push=new PushNotification()
                {
                    Data = DateTime.Today.ToString("d"),
                    EuroRonRate = (result.Item2.FirstOrDefault()??new EuroRonRate()).Valoare,
                    Robor3M = (result.Item1.FirstOrDefault() ?? new RoborHistoric()).Robor3M,
                    Robid3M = (result.Item1.FirstOrDefault() ?? new RoborHistoric()).Robid3M
                };
                log.Info($"Push notification {push}");
                var json = JsonConvert.SerializeObject(push,Formatting.None,new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                log.Info($"Push notification {json}");
                using (var client = new HttpClient())
                {
                    //var request =
                    //    new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("PushHttp"));
                    
                    //request.Content = new StringContent(json);
                    //client.SendAsync(request).Wait();
                    
                    var jmecherie=client.PostAsync(Environment.GetEnvironmentVariable("PushHttp"), new StringContent(json,Encoding.UTF8,"application/json")).Result;
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
