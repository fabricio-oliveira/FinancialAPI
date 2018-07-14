using System;
using FinancialApi.Models.DTO.Request;

namespace FinancialApiUnitTests.Factory
{
    public class ReceiptFactory
    {
        public static Receipt Build(Action<Receipt> pred = null)
        {
            var receipt = new Receipt("Op Teste", "1234567-8", "0123-4", "corrente", "012.345.678-90",
                               100.00m, 0.03m, DateTime.Now);

            pred?.Invoke(receipt);
            return receipt;

        }
    }
}
