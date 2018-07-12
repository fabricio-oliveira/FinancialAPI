using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.src.Models.Entity;

namespace FinancialApi.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly DataBaseContext _context;

        public AccountRepository(DataBaseContext context)
        {
            this._context = context;
        }

        public void Save(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public IEnumerable<Account> List()
        {
            return _context.Accounts;
        }

        public Account FindOrCreate(string number, string bank, string type, string identity)
        {
           

            //var account = _context.Accounts.Where(x => x.Number == number
            //        && x.Bank == bank
            //        && x.Identity == identity
            //        && x.TypeAccount == type)?
            //.First();

          

            //if (account == null)
            //{
               var account = new Account(number, bank, type, identity);
                _context.Accounts.Add(account);
            //}


            return account;
        }
    }
}
