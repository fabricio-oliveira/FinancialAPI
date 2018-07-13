using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;


namespace FinancialApi.Services
{
    public interface IBalanceService
    {
        List<Balance> CashFlow(Account account);
      
    }

    public class BalanceService : IBalanceService 
    {
        private readonly BalanceRepository _balanceRepository;

        public BalanceService(BalanceRepository balanceRepository)
        {
            this._balanceRepository = balanceRepository;
        }

        public List<Balance> CashFlow(Account account)
        {
            return _balanceRepository.ListTodayMore30Ahead(account);
        }
    }
}
