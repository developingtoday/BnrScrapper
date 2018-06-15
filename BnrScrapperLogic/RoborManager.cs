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

        public async Task DoMagic(DateTime startDate, DateTime endDate)
        {
            _log.Log($"Starting Date {startDate.ToShortDateString()} - End Date {endDate.ToShortDateString()}");
            var rates = await _rateService.GetRates(startDate, endDate);
            _log.Log($"Rates found {rates.Count}");
            _log.Log("Updating");
            _rateRepository.InsertBatch(rates);
            _log.Log("End update");
        }
    }
}
