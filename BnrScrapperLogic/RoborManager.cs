using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BnrScrapperLogic
{
    public class RoborManager
    {
        private readonly BnrRateService _rateService;
        
        private readonly RateRepository _rateRepository;
        private readonly ILog _log;
        public RoborManager(string connString,ILog log)
        {
            _log = log;
            _rateService = new BnrRateService();
            _rateRepository = new RateRepository(connString);
        }

        public async Task<Tuple<List<RoborHistoric>, List<EuroRonRate>>> DoMagic(DateTime startDate, DateTime endDate)
        {
            _log.Log($"Starting Date {startDate.ToShortDateString()} - End Date {endDate.ToShortDateString()}");
            var rates = await _rateService.GetRates(startDate, endDate);
            var euroRates = await _rateService.GetEuroRate(startDate, endDate);
            _log.Log($"Rates found {rates.Count}");
            _log.Log($"Euro Ron Rates found {euroRates.Count}");
            _log.Log("Updating Robor");
            _rateRepository.InsertBatch(rates);
            _log.Log("End update Robor");
            _log.Log("Updating Euro Ron Rate");
            _rateRepository.InsertBatchEuroRonRate(euroRates);
            _log.Log("End update Euro Ron Rate");
            return new Tuple<List<RoborHistoric>, List<EuroRonRate>>(rates, euroRates);
        }
    }
}
