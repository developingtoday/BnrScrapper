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
        public static void Run([TimerTrigger("0 0 10-14 ? * MON,TUE,WED,THU,FRI *")]TimerInfo myTimer, TraceWriter log)
        {

            var connString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            var manager = new RoborManager(connString,new AzureLogger(log));
             
             manager.DoMagic(DateTime.Today, DateTime.Today).Wait();
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }

    public class AzureLogger : ILog
    {
        private readonly TraceWriter writer;

        public AzureLogger(TraceWriter writer)
        {
            this.writer = writer;
        }

        public void Log(string msg)
        {
            writer.Info(msg);
        }
    }
}
