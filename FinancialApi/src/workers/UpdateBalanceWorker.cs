using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;
using FinancialApi.Services;
using Hangfire;

namespace FinancialApi.workers
{
    public class UpdateBalanceWorker
    {

        readonly IBalanceService _balanceService;
        readonly IAccountRepository _accountReposiotry;
        const int _maxBatch = 100;

        public UpdateBalanceWorker(IBalanceService balanceService,
                                   IAccountRepository accountReposiotry)
        {
            _balanceService = balanceService;
            _accountReposiotry = accountReposiotry;
        }

        public void WorkManagement()
        {
            var day = DateTime.Today.AddDays(-1);
            var balancesToProcess = _balanceService.ToProcess(day);

            for (int i = 0; i < balancesToProcess.Count; i += _maxBatch)
            {
                BackgroundJob.Enqueue(() => BatchToProcess(balancesToProcess.GetRange(i, Math.Min(_maxBatch,
                                                                                                  balancesToProcess.Count - i)),
                                                           day));
            }
        }


        public void BatchToProcess(List<Balance> balances, DateTime date)
        {
            foreach (Balance balance in balances)
            {
                _balanceService.GenerateBalanceWithInterest(balance, date);
            }

        }


    }
}
