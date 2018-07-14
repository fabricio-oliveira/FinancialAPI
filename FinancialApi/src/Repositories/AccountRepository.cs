using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class AccountRepository : GenericRepository, IRepository<Account>
    {
        private readonly DataBaseContext _context;

        public AccountRepository(DataBaseContext context):base(context)
        {
            this._context = context;
        }

        public void Save(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public void Update(Account account)
        {
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public Account Find(long id)
        {
            return _context.Accounts.Find(id);
        }

        public Account FindOrCreate(string number, string bank, string type, string identity)
        {
            var account = _context.Accounts.Where(x => x.Number == number
                                                  && x.Bank == bank
                                                  && x.Identity == identity
                                                  && x.Type == type)
                                                 .FirstOrDefault();

            if (account == null)
            {
               account = new Account(number, bank, type, identity);
               _context.Accounts.Add(account);
               _context.SaveChanges();
            }

            return account;
        }
    }
}
