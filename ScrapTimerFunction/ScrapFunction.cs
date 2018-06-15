using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace ScrapTimerFunction
{
    public static class ScrapFunction
    {
        [FunctionName("BnrRoborFunction")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
