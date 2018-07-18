using System;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using FinancialApi.UnitTests;

namespace FinancialApiUnitTests.Factory
{
    public static class AccountFactory
    {
        static DataBaseContext Context()
        {
            return DatabaseHelper.Connection();
        }

        public static Account Build(Action<Account> pred = null)
        {
            var account = new Account("00000-1", "000-1", "999.999.9990-90", "corrente");

            pred?.Invoke(account);
            return account;
        }

        public static Account Create(Action<Account> pred = null)
        {
            var value = Build(pred);
            Context().Accounts.Add(value);
            Context().SaveChanges();
            return value;

        }
    }
}
