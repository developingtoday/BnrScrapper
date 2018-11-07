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
        public static void Run([TimerTrigger("0 0/6 8-9  * * 1-5")]TimerInfo myTimer, TraceWriter log)
        {
            try
            {
                log.Info("Starting execution");
                var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
                var manager = new RoborManager(connString, new AzureLogger(log));
                var recentRobor = manager.GetRecentRobor();
                log.Info($"Querying db on current date befor running scrapper, return {recentRobor.ToString()}");
                var result=manager.DoMagic(DateTime.Today, DateTime.Today).Result.FirstOrDefault()??new RoborHistoric();
                var push = CreateRatePushNotificationModel(result, recentRobor);
                SendRoborPushNotification(log, push, recentRobor);
                log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                log.Error("An error occurred",ex);
                throw;
            }
        }

        private static void SendRoborPushNotification(TraceWriter log, RatePushNotification ratePush, RoborHistoric recentRobor)
        {
            log.Info($"Push notification {ratePush}");
            var json = JsonConvert.SerializeObject(ratePush, Formatting.None, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            if (!ratePush.SendRoborPush) return;
            if (recentRobor.Data.Date == DateTime.Today.Date) return;
            log.Info($"Push notification json {json}");
            using (var client = new HttpClient())
            {
                var jmecherie = client.PostAsync(Environment.GetEnvironmentVariable("PushHttp"),
                    new StringContent(json, Encoding.UTF8, "application/json")).Result;
            }

        }

        private static RatePushNotification CreateRatePushNotificationModel(RoborHistoric result, RoborHistoric robors)
        {
            var push = new RatePushNotification()
            {
                Data = DateTime.Today.ToString("d"),
                Robor3M = result.Robor3M,
                Robid3M = result.Robid3M,
            };
            push.Delta = push.Robor3M - robors.Robor3M;
            return push;
        }
    }
}
