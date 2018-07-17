
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account FindOrCreate(string number, string bank, string type, string identity);

        List<Account> List();
    }
}
