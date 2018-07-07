using System;
using System.Text;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using FinancialApi.Models.Response;
using RabbitMQ.Client;

namespace FinancialApi.Services 
{
    public interface IPaymentService
    {
        Task<Base> Pay(Payment payment);   
    }

    public class PaymentService : IPaymentService
    {

        private readonly QueueContext _context;

        public PaymentService(QueueContext context) => this._context = context;

        public async Task<Base> Pay(Payment payment)
        {
            var error = Validate(payment);
            if (error != null) return await Task.FromResult(error);

            Enqueue(payment);
            return new OK(payment.UUID);
        }

        // Private

        Errors Validate(Payment payment){
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