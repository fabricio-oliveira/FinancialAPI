using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialApi.src.Models.Entity;

namespace FinancialApi.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private DataBaseContext _context;

        public AccountRepository(DataBaseContext context)
        {
            _context = context;
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

            var account = _context.Accounts.First();

            if (account == null)
            {
                account = new Account(number, bank, type, identity);
                _context.Accounts.Add(account);
            }


            return account;
        }
    }
}
