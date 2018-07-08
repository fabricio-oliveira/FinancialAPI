using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class ReceiptRepository: IRepository<Receipt>
    {
        private DataBaseContext _context;

        public ReceiptRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void Save(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            _context.SaveChanges();
        }

        public IEnumerable<Receipt> List()
        {
            return _context.Receipts;
        }
    }
}
