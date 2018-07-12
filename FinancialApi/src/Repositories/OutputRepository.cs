using System.Collections.Generic;
using FinancialApi.Config;
using FinancialApi.src.Models.Entity;

namespace FinancialApi.Repositories
{
    public class OutputRepository: IRepository<Output>
    {
        private DataBaseContext _context;

        public OutputRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void Save(Output receipt)
        {
            _context.Outputs.Add(receipt);
            _context.SaveChanges();
        }

        public IEnumerable<Output> List()
        {
            return _context.Outputs;
        }
    }
}
