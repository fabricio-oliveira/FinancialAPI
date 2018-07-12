using System;
using FinancialApi.Models.DTO.Request;

namespace FinancialApiUnitTests.Factory
{
    public class PaymentFactory
    {
        public static PaymentDTO Build(Action<PaymentDTO> pred = null)
        {
            var payment = new PaymentDTO("Op Teste", "1234567-8", "0123-4", "corrente", "012.345.678-90",
                               100.00m, 0.03m, DateTime.Now);

            pred?.Invoke(payment);
            return payment;

        }
    }
}
