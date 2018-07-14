using System;
using FinancialApi.Models.Entity;

namespace FinancialApiUnitTests.Factory
{
    public class AccountFactory
    {
        public static Account Build(Action<Account> pred = null)
        {
            var acccount = new Account("00000-1", "000-1", "999.999.9990-90", "corrente");

            pred?.Invoke(acccount);
            return acccount;

        }
    }
}
