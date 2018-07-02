using System;
using System.Collections.Generic;
using FinancialApi.Models;
using FinancialApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApi.Controllers
{
    [Route("api/v1/financial")]
    public class FinancialController : Controller
    {
        private readonly IPaymentService _PaymentService;

        public FinancialController(IPaymentService PaymentService)
        {
            this._PaymentService = PaymentService;   
        }

        // Post receipt
        [HttpPost("receipt")]
        public void Receipt()
        {
            throw new NotImplementedException("Need implementation receipt");
        }

        // Post payment
        [HttpPost("payment")]
        public string Payment([FromBody]Payment payment)
        {
            _PaymentService.Pay(payment);
            return "";
        }

        // Post payment
        [HttpGet("cash_flow")]
        public IEnumerable<Entry> CashFlow(int id) => throw new NotImplementedException("Need implementation payment");
    }
}
