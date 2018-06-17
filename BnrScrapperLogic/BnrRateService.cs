using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BnrScrapperLogic
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
                var scrapper = new BnrRoborScapper(await result.Content.ReadAsStreamAsync());
                robor = scrapper.GetValues();
            }
            return robor;
        }

        public async Task<List<EuroRonRate>> GetEuroRate(DateTime dateStart, DateTime dateEnd)
        {
            List<EuroRonRate> robor;
            using (var http = new System.Net.Http.HttpClient())
            {
                var dateStr = $"{dateStart.Day}-{dateStart.Month}-{dateStart.Year}";
                var dateSto = $"{dateEnd.Day}-{dateEnd.Month}-{dateEnd.Year}";
                //http://www.bnro.ro/StatisticsReportHTML.aspx?icid=800&table=668&column=5462&startDate=01-06-2018&stopDate=07-06-2018
                var request =
                    $"http://bnr.ro/StatisticsReportHTML.aspx?icid=800&table=668&column=5462&startDate={dateStr}&stopDate={dateSto}";
                var result = await http.GetAsync(request);
                var strRes = await result.Content.ReadAsStringAsync();
                var scrapper = new BnrEuroScapper(await result.Content.ReadAsStreamAsync());
                robor = scrapper.GetValues();
            }
            return robor;
        }
    }
}