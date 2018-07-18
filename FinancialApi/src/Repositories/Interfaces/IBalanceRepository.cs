
using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IBalanceRepository : IRepository<Balance>
    {
        Balance FindOrCreateBy(Account account, DateTime date);
        Balance Find(Account account, DateTime date);
        Balance LastByOrDefault(Account account);
        void UpdateDayPosition(Balance balance);
        List<Balance> ListTodayMore30Ahead(Account account);

        List<Balance> ToProcess(DateTime date, List<Account> account);
    }
}
