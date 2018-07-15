
using System;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IBalanceRepository : IRepository<Balance>
    {
        Balance FindOrCreateBy(Account account, DateTime date);
        Balance LastByOrDefault(Account account);
    }
}
