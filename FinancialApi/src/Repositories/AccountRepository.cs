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


        public Account Find(long? id) => _context.Accounts.Find(id);

        public Account FindOrCreateBy(string number, string bank, string type, string identity)
        {
            var account = FindBy(number, bank, type, identity);

            if (account == null)
            {
                account = new Account(number, bank, identity, type);
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }

            return account;
        }


        public Account FindBy(string number, string bank, string type, string identity)
        {
            return _context.Accounts.Where(x => x.Number.Equals(number)
                                             && x.Bank.Equals(bank)
                                             && x.Identity.Equals(identity)
                                             && x.Type.Equals(type))
                                            .SingleOrDefault();



        }
        public long Count()
        {
            return _context.Accounts.Count();
        }
    }
}
