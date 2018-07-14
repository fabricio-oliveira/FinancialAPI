using System;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;

namespace FinancialApiUnitTests.Factory
{
    public static class EntryFactory
    {
        public static Entry Build(Action<Entry> pred = null)
        {
            var value = new Entry("pagamento", "Op Teste", "1234567-8", "0123-4", "corrente", "012.345.678-90",
                                100.00m, 0.03m, DateTime.Now);

            pred?.Invoke(value);
            return value;

        }
    }
}
