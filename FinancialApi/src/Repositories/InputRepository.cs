using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Models.DTO.Request;
using FinancialApi.src.Models.Entity;

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

        public IEnumerable<Input> List()
        {
            return _context.Inputs;
        }

    }
}
