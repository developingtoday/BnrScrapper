using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using BnrScrapperLogic;
using HtmlAgilityPack;

namespace BnrScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var bnrScrapper = new BnrRateService();
            var robor = bnrScrapper.GetRates(DateTime.Today.AddYears(-25), DateTime.Today).Result;
            var euro = bnrScrapper.GetEuroRate(DateTime.Today.AddYears(-25), DateTime.Today).Result;
            var repo = new RateRepository(Environment.GetEnvironmentVariable("DatabaseConnectionString"));
            //repo.InsertBatch(robor);
            repo.InsertBatchEuroRonRate(euro);
            Console.WriteLine("E gata smecheria");
        }
    }
}
