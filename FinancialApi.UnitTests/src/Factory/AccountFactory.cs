using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public static List<Account> CreateList(int size, Action<Account> pred = null)
        {
            var accounts = new List<Account>();
            for (int i = 0; i < size; i++)
            {
                if (pred == null) pred = (Account a) => { };

                pred += (Account a) => a.Number = Regex.Replace(a.Number, "(\\d{5})-(\\d{1})", i.ToString("00000") + "-($2)");
                var account = Build(pred);
                accounts.Add(account);
                Context().Accounts.Add(account);
            }

            Context().SaveChanges();
            return accounts;

        }
    }

}