using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;


namespace FinancialApi.Services
{
    public class BalanceService : IBalanceService
    {
        const decimal TX_INTEREST = 0.083m;

        readonly IBalanceRepository _balanceRepository;
        readonly IAccountRepository _accountRepository;
        readonly IInterestRepository _interestRepository;

        public BalanceService(IBalanceRepository balanceRepository,
                              IInterestRepository interestRepository,
                              IAccountRepository accountRepository)
        {
            _balanceRepository = balanceRepository;
            _interestRepository = interestRepository;
            _accountRepository = accountRepository;
        }

        public List<Balance> CashFlow(Account account)
        {
            return _balanceRepository.ListTodayMore30Ahead(account);
        }

        public List<Balance> ToProcess(DateTime date)
        {
            var accounts = _accountRepository.List().ToList();
            return _balanceRepository.ToProcess(date, accounts);
        }

        public void GenerateBalanceWithInterest(Balance balance, DateTime date)
        {
            using (_balanceRepository.BeginTransaction())
            {

                //Save Interes
                if (balance.Total < 0)
                {
                    var interest = balance.Total * TX_INTEREST;
                    _interestRepository.Save(new Interest(interest, date, balance.Account));
                    balance.Charges.Add(new ShortEntryDTO(DateTime.Today, interest));
                    balance.UpdateDayPostion(balance.Total);
                }

                balance.Closed = true;
                _balanceRepository.Update(balance);
                _balanceRepository.Commit();
            }

        }

    }
}
