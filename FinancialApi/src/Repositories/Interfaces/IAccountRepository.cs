
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account FindOrCreateBy(string number, string bank, string type, string identity);
        Account FindAs(string number, string bank, string type, string identity);

        List<Account> List();
    }
}
