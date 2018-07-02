using System;
using System.Collections.Generic;
using FinancialApi.Models;

namespace FinancialApi.Repositories
{
    public class PaymentRepository
    {
        private DataBaseContext _context;

        public PaymentRepository(DataBaseContext context)
        {
            _context = context;
        }

        public void Save(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public IEnumerable<Payment> List()
        {
             return _context.Payments;
        }
    }
}
