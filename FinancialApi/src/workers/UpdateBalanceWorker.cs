using System;
using System.Collections.Generic;
using System.Threading;
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
            var yesterday = DateTime.Today.AddDays(-1);
            var balancesToProcess = _balanceService.ToProcess(yesterday);

            foreach (Balance balance in balancesToProcess)
                _balanceService.GenerateBalanceWithInterest(balance, yesterday);

        }

    }
}
