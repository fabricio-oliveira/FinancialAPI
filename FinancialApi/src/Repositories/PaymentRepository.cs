using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Repositories
{
    public interface IPaymentRepository
    {
        void Save(Payment payment);
        IEnumerable<Payment> List();
    }


    public class PaymentRepository: IPaymentRepository
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
