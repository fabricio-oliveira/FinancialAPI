using System;
using System.Collections.Generic;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;


namespace FinancialApi.Services
{
    public class BalanceService : IBalanceService
    {
        const decimal TX_INTEREST = 0.083m;

        readonly IBalanceRepository _balanceRepository;
        readonly IInterestRepository _interestRepository;

        public BalanceService(IBalanceRepository balanceRepository,
                              IInterestRepository interestRepository)
        {
            _balanceRepository = balanceRepository;
            _interestRepository = interestRepository;
        }

        public List<Balance> CashFlow(Account account)
        {
            return _balanceRepository.ListTodayMore30Ahead(account);
        }

        public void GenerateBalanceWithInterest(Account account, DateTime date)
        {
            using (_balanceRepository.BeginTransaction())
            {
                // previously Balance
                var balance = _balanceRepository.Find(account, date.AddDays(-1));

                if (balance == null)
                    throw new Exception("Problem find last Balance");

                // new balance 
                var newBalance = _balanceRepository.FindOrCreateBy(account, date);

                //Save Interes
                if (balance.Total < 0)
                {
                    var interest = balance.Total * TX_INTEREST;
                    _interestRepository.Save(new Interest(interest, date, account));
                    newBalance.Charges.Add(new ShortEntryDTO(DateTime.Today, interest));
                    newBalance.UpdateDayPostion(balance.Total);
                }

                _balanceRepository.Update(newBalance);
                _balanceRepository.Commit();
            }

        }

    }
}
