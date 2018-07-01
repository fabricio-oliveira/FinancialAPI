using System;
using System.Collections.Generic;
using FinancialApi.Model;
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

        // Post payment
        [HttpGet("cash_flow")]
        public IEnumerable<Entry> CashFlow(int id)
        {
            throw new NotImplementedException("Need implementation payment");
        }
    }
}
