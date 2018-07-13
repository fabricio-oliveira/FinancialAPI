using System.Collections.Generic;
using FinancialApi.Config;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class InputRepository: IRepository<Input>
    {
        private DataBaseContext _context;

        public InputRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void Save(Input input)
        {
            _context.Inputs.Add(input);
            _context.SaveChanges();
        }

        public void Update(Input input)
        {
            _context.Inputs.Update(input);
            _context.SaveChanges();
        }

        public Input Find(long id)
        {
            return _context.Inputs.Find(id);
        }

        public void Get(Input t)
        {
            throw new System.NotImplementedException();
        }
    }
}
