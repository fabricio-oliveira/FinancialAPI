using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;
using FinancialApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using FinancialApi.Models.DTO.Response;
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
        public async Task<IActionResult> Receipt([FromBody]Receipt receipt)
        {
            if (!ModelState.IsValid)
            {
                var errors = ValidateErrors(receipt);
                return new BadRequestObjectResult(errors);
            }

            var result = await _receiptService.EnqueueToReceive(receipt);

            if (result is ErrorsDTO)
                return new BadRequestObjectResult(result);

            return new OkObjectResult(result);
        }

        // Post payment
        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody]Payment payment)
        {
            if (!ModelState.IsValid)
            {
                var errors = ValidateErrors(payment);
                return new BadRequestObjectResult(errors);
            }

            var result =await  _paymentService.EnqueueToPay(payment);

            if (result is ErrorsDTO)
                return new BadRequestObjectResult(result);

            return new OkObjectResult(result);
        }

        // Post payment
        [HttpGet("cash_flow")]
        public IEnumerable<Entry> CashFlow([FromBody] Account account){
            throw new NotImplementedException("Need implementation payment");  
        } 


        private ErrorsDTO ValidateErrors(Object obj)
        {
            var errors = new ErrorsDTO();

            foreach (var key in ModelState.Keys)
                foreach (ModelError error in ModelState[key].Errors)
                    errors.Add(obj.GetJSonFieldName(key), error.ErrorMessage);

            return errors;
        }
    }
}
