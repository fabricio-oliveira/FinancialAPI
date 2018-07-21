using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;
using FinancialApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Utils;
using System.Linq;

namespace FinancialApi.Controllers
{
    [Route("api/v1/financial")]
    public class FinancialController : Controller
    {
        readonly IPaymentService _paymentService;
        readonly IReceiptService _receiptService;
        readonly IBalanceService _balanceService;

        public FinancialController(IPaymentService paymentService,
                                   IReceiptService receiptService,
                                   IBalanceService balanceService)
        {
            _paymentService = paymentService;
            _receiptService = receiptService;
            _balanceService = balanceService;
        }

        // Post entry
        [HttpPost("entry")]
        public async Task<IActionResult> Entry([FromBody]Entry entry)
        {
            if (!ModelState.IsValid)
            {
                var errors = ValidateErrors(entry);
                return new BadRequestObjectResult(errors);
            }

            IBaseDTO result;


            if (entry.IsReceipt())
                result = await _receiptService.EnqueueToReceive(entry);
            else
                result = await _paymentService.EnqueueToPay(entry);


            if (result is ErrorsDTO)
                return new BadRequestObjectResult(result);

            return new OkObjectResult(result);
        }


        [HttpGet("cash_flow")]
        public IActionResult CashFlow([FromQuery] Account account)
        {

            var result = _balanceService.CashFlow(account);

            if (result == null)
            {
                var error = new ErrorsDTO();
                error.Add("account", "not found");
                return new NotFoundObjectResult(error);
            }

            return new OkObjectResult(result);
        }


        private ErrorsDTO ValidateErrors(Object obj)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => obj.GetJSonFieldName(kvp.Key),
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

            return new ErrorsDTO(errors);
        }
    }
}
