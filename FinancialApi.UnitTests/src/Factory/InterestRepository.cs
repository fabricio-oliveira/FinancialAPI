using System;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using FinancialApi.UnitTests;

namespace FinancialApiUnitTests.Factory
{
    public static class InterestFactory
    {
        static DataBaseContext Context()
        {
            return DatabaseHelper.Connection();
        }

        public static Interest Build(Action<Interest> pred = null)
        {
            var value = new Interest(100.00m, DateTime.Now, null);

            pred?.Invoke(value);
            return value;
        }

        public static Interest Create(Action<Interest> pred = null)
        {
            var value = Build(pred);
            Context().Interests.Add(value);
            Context().SaveChanges();
            return value;

        }


    }
}