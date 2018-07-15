using System;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using FinancialApi.UnitTests;

namespace FinancialApiUnitTests.Factory
{
    public static class EntryFactory
    {
        static readonly DataBaseContext _context;
        static EntryFactory()
        {
            _context = DbHelper.Connection();
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
            _context.Entries.Add(value);
            _context.SaveChanges();
            return value;

        }


    }
}