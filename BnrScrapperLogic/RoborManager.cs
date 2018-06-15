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
        public RoborManager()
        {
            _rateService = new BnrRateService();
            
        }

        public async Task DoMagic(DateTime startDate, DateTime endDate)
        {
            var rates = await _rateService.GetRates(startDate, endDate);
            _rateRepository.InsertBatch(rates);
        }
    }
}
