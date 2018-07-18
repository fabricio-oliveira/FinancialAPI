using System;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using FinancialApi.UnitTests;

namespace FinancialApiUnitTests.Factory
{
    public static class EntryFactory
    {
        static DataBaseContext Context()
        {
            return DatabaseHelper.Connection();
        }

        public static Entry Build(Action<Entry> pred = null)
        {
            var value = new Entry("pagamento", "Op Teste", "1234567-8", "0123-4", "corrente", "012.345.678-90",
                                100.00m, 0.03m, DateTime.Now);

            pred?.Invoke(value);
            return value;
        }


        public static Entry Create(Action<Entry> pred = null)
        {
            var value = Build(pred);
            Context().Entries.Add(value);
            Context().SaveChanges();
            return value;

        }


    }
}