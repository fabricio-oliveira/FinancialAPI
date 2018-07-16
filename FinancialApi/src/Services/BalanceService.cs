using System;
using System.Collections.Generic;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;


namespace FinancialApi.Services
{
    public class BalanceService : IBalanceService
    {
        const decimal TX_INTEREST = 0.83m;

        readonly IBalanceRepository balanceRepository;
        readonly IInterestRepository interestRepository;

        public BalanceService(IBalanceRepository balanceRepository,
                              IInterestRepository interestRepository)
        {
            this.balanceRepository = balanceRepository;
            this.interestRepository = interestRepository;
        }

        public List<Balance> CashFlow(Account account)
        {
            return balanceRepository.ListTodayMore30Ahead(account);
        }

        public void GenerateBalanceWithInterest(Account account, DateTime date)
        {
            // Find Balance
            var previuslyBalance = balanceRepository.Find(account, date.AddDays(-1));

            if (previuslyBalance == null)
                throw new Exception("Problem find last Balance");

            //Save Interes
            var interest = previuslyBalance.Total * TX_INTEREST;

            interestRepository.Save(new Interest(interest, date, account));

            // Balance

            var newBalance = balanceRepository.FindOrCreateBy(account, date);

            newBalance.Charges.Add(new ShortEntryDTO(DateTime.Today, interest));


        }

    }
}
