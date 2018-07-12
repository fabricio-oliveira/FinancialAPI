using System;
using System.Collections.Generic;
using FinancialApi.Config;
using FinancialApi.Models.DTO.Request;
using FinancialApi.src.Models.Entity;

namespace FinancialApi.Repositories
{
    public class ChargeRepository: IRepository<Charge>
    {
        private DataBaseContext _context;

        public ChargeRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void Save(Charge charge)
        {
            _context.Charges.Add(charge);
            _context.SaveChanges();
        }

        public IEnumerable<Charge> List()
        {
            return _context.Charges;
        }
    }
}
