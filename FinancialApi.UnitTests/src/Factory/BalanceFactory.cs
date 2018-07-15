using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinancialApi.Config;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.UnitTests;

namespace FinancialApiUnitTests.Factory
{
    public static class BalanceFactory
    {
        static readonly DataBaseContext _context;

        static BalanceFactory()
        {
            _context = DbHelper.Connection();
        }

        public static Balance Build(Action<Balance> pred = null)
        {
            var balance = new Balance(DateTime.Today,
                                      new List<ShortEntryDTO>(),
                                      new List<ShortEntryDTO>(),
                                      new List<ShortEntryDTO>(),
                                      130.00m, 0.7m, new Account());

            pred?.Invoke(balance);
            return balance;
        }

        public static Balance Create(Action<Balance> pred = null)
        {
            var value = Build(pred);
            _context.Balances.Add(value);
            _context.SaveChanges();
            return value;

        }
    }
}
