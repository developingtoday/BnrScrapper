using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BnrScrapper
{
    public class BnrRateService
    {
        public BnrRateService()
        {
            
        }

        public async Task<List<RoborHistoric>> GetRates(DateTime dateStart, DateTime dateEnd)
        {
            List<RoborHistoric> robor;
            using (var http = new System.Net.Http.HttpClient())
            {
                var dateStr = $"{dateStart.Day}-{dateStart.Month}-{dateStart.Year}";
                var dateSto = $"{dateEnd.Day}-{dateEnd.Month}-{dateEnd.Year}";
                var request =
                    $"http://bnr.ro/StatisticsReportHTML.aspx?icid=801&table=642&column=&startDate={dateStr}&stopDate={dateSto}";
                var result =await http.GetAsync(request);
                var strRes = await result.Content.ReadAsStringAsync();
                var scrapper = new BnrRateScrapper(await result.Content.ReadAsStreamAsync());
                robor = scrapper.GetValues();
            }
            return robor;
        }
    }
}