using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using FinancialApi.Utils;

namespace FinancialApi.Repositories
{
    public class BalanceRepository: IRepository<Balance>
    {
        private DataBaseContext _context;

        public BalanceRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void Save(Balance balance)
        {
            _context.Balances.Add(balance);
            _context.SaveChanges();
        }

        public void Update(Balance balance)
        {
            _context.Balances.Update(balance);
            _context.SaveChanges();
        }

        public Balance Find(long id)
        {
           return _context.Balances.Find(id);
        }

        public List<Balance> ListTodayMore30Ahead(Account account)
        {
            return _context.Balances.Where( x => x.Account == account
                                           &&    x.Date >= DateTime.Today
                                           &&    x.Date < DateTime.Today.AddDays(31))
                                    .OrderBy( x => x.Date)
                                    .ToList();
        }

        public Balance GetBy(Account account, DateTime date)
        {
            var balance = _context.Balances
                                   .Where(x => x.Account == account
                                          && x.Date == date)
                                    .OrderByDescending(x => x.Date.IsSameDay(date))
                                    .FirstOrDefault();

            if (balance != null) return balance;

                
            var lastBalance = LastBy(account);
            if (lastBalance != null)
            {
                balance = new Balance(date, null, null, null, lastBalance.Total, lastBalance.DayPosition, account);
                _context.Balances.Add(balance);
                return balance;
            }

            balance = new Balance(date, null, null, null, 0, 0, account);
            _context.Balances.Add(balance);
            return balance;
        }

        public Balance LastBy(Account account)
        {
            var balance = _context.Balances
                                   .Where(x => x.Account == account)
                                   .OrderByDescending(x => x.Date)
                                   .FirstOrDefault();
            
            return balance;
        }

        public Balance LastByOrDefault(Account account)
        {
            var balance = LastBy(account);
            return  balance ?? new Balance(DateTime.Today.AddDays(-1), null, null, null, 0.0m, 0.0m,account);
        }
    }
}

