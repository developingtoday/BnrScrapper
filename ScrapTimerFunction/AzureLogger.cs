using BnrScrapperLogic;
using Microsoft.Azure.WebJobs.Host;
namespace ScrapTimerFunction
{
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
