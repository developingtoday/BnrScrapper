using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BnrScrapperLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RateApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public ValuesController()
        {
            //throw new Exception("Greseala");
        }

        // GET api/values
        [HttpGet("roborstype")]
        public IEnumerable<string> Get()
        {
            return Enum.GetNames(typeof(BankMargin));
        }

    }
}
