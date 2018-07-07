using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;
using FinancialApi.Services;
using Microsoft.AspNetCore.Mvc;
using FinancialApi.Models.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FinancialApi.Utils;

namespace FinancialApi.Controllers
{
    [Route("api/v1/financial")]
    public class FinancialController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IReceiptService _receiptService;

        public FinancialController(IPaymentService paymentService,
                                   IReceiptService receiptService)
        {
            this._paymentService = paymentService;
            this._receiptService = receiptService;
        }

        // Post receipt
        [HttpPost("receipt")]
        public IActionResult Receipt([FromBody]Receipt receipt)
        {
            if (!ModelState.IsValid)
            {
                var errors = ValidateErrors();
                return new BadRequestObjectResult(errors);
            }

            var result = _receiptService.receive(receipt);

            if (result is Errors)
                return new BadRequestObjectResult(result);

            return new OkObjectResult(result);
        }

        // Post payment
        [HttpPost("payment")]
        public IActionResult Payment([FromBody]Payment payment)
        {
            if (!ModelState.IsValid)
            {
                var errors = ValidateErrors();
                return new BadRequestObjectResult(errors);
            }

            var result = _paymentService.Pay(payment);

            if (result is Errors)
                return new BadRequestObjectResult(result);

            return new OkObjectResult(result);
        }

        // Post payment
        [HttpGet("cash_flow")]
        public IEnumerable<Entry> CashFlow(int id) => throw new NotImplementedException("Need implementation payment");


        private Errors ValidateErrors()
        {
            var errors = new Errors();

            foreach (var key in ModelState.Keys)
                foreach (ModelError error in ModelState[key].Errors)
                    errors.Add(key.ToUnderScore(), error.ErrorMessage);

            return errors;
        }
    }
}
