using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using FinancialApi.Utils;

namespace FinancialApi.Repositories
{
    public class BalanceRepository : GenericRepository, IBalanceRepository
    {
        private DataBaseContext _context;

        public BalanceRepository(DataBaseContext context) : base(context)
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
            return _context.Balances.Where(x => x.Account == account
                                          && x.Date >= DateTime.Today
                                          && x.Date < DateTime.Today.AddDays(31))
                                    .OrderBy(x => x.Date)
                                    .ToList();
        }

        public Balance FindOrCreateBy(Account account, DateTime date)
        {
            var balance = _context.Balances
                                   .Where(x => x.Account == account
                                          && x.Date == date)
                                    .FirstOrDefault();

            if (balance != null) return balance;

            var lastBalance = LastBy(account);
            if (lastBalance != null)
            {
                balance = new Balance(date, null, null, null, lastBalance.Total, lastBalance.Total, account);
                _context.Balances.Add(balance);
                _context.SaveChanges();
                return balance;
            }

            balance = new Balance(date, null, null, null, 0, 0, account);
            _context.Balances.Add(balance);
            _context.SaveChanges();
            return balance;
        }

        public Balance LastByOrDefault(Account account)
        {
            var balance = LastBy(account);
            return balance ?? new Balance(DateTime.Today.AddDays(-1), null, null, null, 0.0m, 100.0m, account);
        }

        public Balance LastBy(Account account)
        {
            var balance = _context.Balances
                                   .Where(x => x.Account == account)
                                   .OrderByDescending(x => x.Date)
                                   .FirstOrDefault();

            return balance;
        }


        public void UpdateDayPosition(Balance balance)
        {
            var balances = _context.Balances.Where(x => x.Account.Equals(balance.Account)
                                                   && x.Date >= balance.Date.AddDays(-1))
                                   .OrderBy(x => x.Date)
                                   .ToArray();

            balances.Skip(1)
                    .Zip(balances, (c, p) => c.DayPosition = DayPostion(p.Total, c.Total));
            _context.SaveChanges();
        }

        private decimal DayPostion(decimal yestarday, decimal today)
        {
            return yestarday == 0m ? 100.0m : today / yestarday;
        }
    }
}

