using System.Collections.Generic;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

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

        public void Update(Output receipt)
        {
            _context.Outputs.Update(receipt);
            _context.SaveChanges();
        }

        public Output Find(long id)
        {
            return _context.Outputs.Find(id);
        }
    }
}
