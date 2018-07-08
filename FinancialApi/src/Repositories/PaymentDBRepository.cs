using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public class PaymentDBRepository: IRepository<Payment>
    {
        private DataBaseContext _context;

        public PaymentDBRepository(DataBaseContext context)
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
