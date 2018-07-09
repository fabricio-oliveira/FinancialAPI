using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Models.DTO.Request;
using FinancialApi.src.Models.Entity;

namespace FinancialApi.Repositories
{
    public class CashFlowRepository: IRepository<CashFlow>
    {
        private DataBaseContext _context;

        public CashFlowRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void Save(CashFlow cashFlow)
        {
            _context.CashFlows.Add(cashFlow);
            _context.SaveChanges();
        }

        public IEnumerable<CashFlow> List()
        {
             return _context.CashFlows;
        }

        public CashFlow LastCashFlow(Account account)
        {
            var cashFlow =  _context.CashFlows
                                    .Where(x => x.Account == account)
                                    .OrderByDescending(x => x.Date)?
                                    .Last();

            return cashFlow ?? new CashFlow(DateTime.Today.AddDays(-1), null, null, null, 0.0m, 0.0m,account);
        }
    }
}
