﻿using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class BalanceRepository : GenericRepository, IBalanceRepository
    {
        readonly DataBaseContext _context;

        public BalanceRepository(DataBaseContext context) : base(context)
        {
            this._context = context;
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

        public Balance Find(int id)
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

        public Balance Find(Account account, DateTime date)
        {
            return _context.Balances
                                   .Where(x => x.Account == account
                                          && x.Date == date)
                                    .FirstOrDefault();
        }

        public Balance FindOrCreateBy(Account account, DateTime date)
        {
            var balance = Find(account, date);

            if (balance != null) return balance;

            var lastBalance = LastBy(account);
            if (lastBalance != null)
            {
                balance = new Balance(date, new List<ShortEntryDTO>(),
                                            new List<ShortEntryDTO>(),
                                            new List<ShortEntryDTO>(), lastBalance.Total, lastBalance.Total, account);
                _context.Balances.Add(balance);
                _context.SaveChanges();
                return balance;
            }

            balance = new Balance(date, new List<ShortEntryDTO>(),
                                        new List<ShortEntryDTO>(),
                                        new List<ShortEntryDTO>(), 0m, 0m, account);
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

            balances = balances.Skip(1)
                               .Zip(balances, (c, p) => { c.UpdateDayPostion(p.Total); return c; })
                               .ToArray();

            _context.UpdateRange(balances);
            _context.SaveChanges();
        }

        //public void CreateBalaceWithInterest(decimal interest)
        //{

        //}

    }
}

