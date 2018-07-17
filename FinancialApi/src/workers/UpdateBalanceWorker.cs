using System;
using System.Threading;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.Services;


namespace FinancialApi.workers
{
    public class UpdateBalanceWorker
    {

        readonly IBalanceService _balanceService;
        readonly IAccountRepository _accountReposiotry;

        public UpdateBalanceWorker(IBalanceService balanceService,
                                   IAccountRepository accountReposiotry)
        {
            _balanceService = balanceService;
            _accountReposiotry = accountReposiotry;
        }

        public void WorkManagement()
        {
            for (var accounts = _accountReposiotry.List())
            {
                var balances = _balanceService.GenerateBalanceWithInterest(Account, DateTime.Today());


            }

        }


    }
}
