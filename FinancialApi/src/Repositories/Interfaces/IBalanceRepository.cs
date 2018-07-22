
using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IBalanceRepository : IRepository<Balance>
    {
        Balance FindOrCreateBy(Account account, DateTime date);
        Balance FindBy(Account account, DateTime date);
        Balance LastByOrDefault(Account account, DateTime date);

        List<Balance> ListTodayMore30Ahead(long? accountId);

        void UpdateCurrentAndFutureBalance(DateTime date, long accountId);
        List<Balance> ToProcess(DateTime date, List<Account> account);
    }
}
