using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class AccountRepository : GenericRepository, IAccountRepository
    {
        private readonly DataBaseContext context;

        public AccountRepository(DataBaseContext context) : base(context)
        {
            this.context = context;
        }

        public void Save(Account account)
        {
            context.Accounts.Add(account);
            context.SaveChanges();
        }

        public void Update(Account account)
        {
            context.Accounts.Update(account);
            context.SaveChanges();
        }

        public Account Find(int id)
        {
            return context.Accounts.Find(id);
        }

        public Account FindOrCreate(string number, string bank, string type, string identity)
        {
            var account = context.Accounts.Where(x => x.Number == number
                                                  && x.Bank == bank
                                                  && x.Identity == identity
                                                  && x.TypeA == type)
                                                 .FirstOrDefault();

            if (account == null)
            {
                account = new Account(number, bank, type, identity);
                context.Accounts.Add(account);
                context.SaveChanges();
            }

            return account;
        }
    }
}
