using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class AccountRepository : GenericRepository, IAccountRepository
    {
        readonly DataBaseContext _context;

        public AccountRepository(DataBaseContext context) : base(context) => _context = context;

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

        public List<Account> List() => _context.Accounts.ToList();

<<<<<<< HEAD
        public Account Find(long? id) => _context.Accounts.Find(id);
=======
        public Account Find(long? id)
        {
            return _context.Accounts.Find(id);
        }
>>>>>>> 0c62ff5d5e8d523918c08c5c5617e23bf795b704

        public Account FindOrCreate(string number, string bank, string type, string identity)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Number.Equals(number)
                                                 && x.Bank.Equals(bank)
                                                 && x.Identity.Equals(identity)
                                                 && x.Type.Equals(type));

            if (account == null)
            {
                account = new Account(number, bank, type, identity);
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }

            return account;
        }

        public long Count()
        {
            return _context.Accounts.Count();
        }
    }
}
