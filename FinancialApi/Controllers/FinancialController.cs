using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;
using FinancialApi.Services;
using Microsoft.AspNetCore.Mvc;
using FinancialApi.Models.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public IActionResult Receipt()
        {
            throw new NotImplementedException("Need implementation receipt");
        }

        // Post payment
        [HttpPost("payment")]
        public IActionResult Payment([FromBody]Payment payment)
        {
            if (!ModelState.IsValid)
                foreach (var key in ModelState.Keys)
                {
                Console.WriteLine("yyy" + key);
                foreach (ModelError error in ModelState[key].Errors)
                    {

                    Console.WriteLine("xxx" + error.ErrorMessage);
                    }
                }
               
            var result = _paymentService.Pay(payment);

            if (result is Error)
                return new BadRequestObjectResult(result);

            return new OkObjectResult(result);
            //return StatusCode(StatusCodes.Status200OK);
        }

        // Post payment
        [HttpGet("cash_flow")]
        public IEnumerable<Entry> CashFlow(int id) => throw new NotImplementedException("Need implementation payment");
    }
}
