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
        private readonly IPaymentService _paymentService;

        public FinancialController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
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
            _paymentService.Pay(payment);
            return "";
        }

        // Post payment
        [HttpGet("cash_flow")]
        public IEnumerable<Entry> CashFlow(int id) => throw new NotImplementedException("Need implementation payment");
    }
}
