using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApi.Controllers
{
    [Route("api/financial")]
    public class FinancialController : Controller
    {
        // Post receipt
        [HttpPost("receipt")]
        public void Receipt()
        {
            throw new NotImplementedException("Need implementation receipt");
        }

        // Post payment
        [HttpPost("payment")]
        public string Payment(int id)
        {
            throw new NotImplementedException("Need implementation payment");
        }
    }
}
