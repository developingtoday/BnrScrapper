using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BnrScrapperLogic;
using Microsoft.AspNetCore.Mvc;

namespace RateApi
{
    [Route("api/[controller]")]
    public class RateController: Controller
    {
        private readonly IRateRepository _rateRepository;

        public RateController(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        [HttpGet]
        public List<RoborHistoric> GetRobors([FromQuery] DateTime? begin, [FromQuery] DateTime? end)
        {
            if (!begin.HasValue && !end.HasValue)
            {
                return new List<RoborHistoric>(1)
                {
                    _rateRepository.GetRoborRecent()
                };
            }
            return _rateRepository.GetRobors(begin ?? DateTime.MinValue, end ?? DateTime.MaxValue);
        }





    }
}
