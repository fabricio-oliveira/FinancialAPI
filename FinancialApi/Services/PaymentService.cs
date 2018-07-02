using System;
using System.Text;
using FinancialApi.Models;
using RabbitMQ.Client;

namespace FinancialApi.Services 
{
    public interface IPaymentService
    {
        Error Pay(Payment payment);   
    }

    class PaymentService : IPaymentService
    {

        private QueueContext _context;
        public PaymentService(QueueContext context) => this._context = context;

        public Error Pay(Payment payment)
        {
            var error = Validate(payment);
            if (error != null) return error;

            Enqueue(payment);
            return null;
        }

        // Private

        Error Validate(Payment payment){
            return null;
        }

        void Enqueue(Payment payment)
        {
            var body = Encoding.UTF8.GetBytes(payment.ToJson());
            _context.channel.BasicPublish(exchange: "",
                                          routingKey: _context.PaymentQueueName,
                                          basicProperties: null,
                                          body: body);
        }

    }

}