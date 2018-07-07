using System;
using System.Text;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using FinancialApi.Models.DTO;
using RabbitMQ.Client;
using FinancialApi.Utils;

namespace FinancialApi.Services 
{
    public interface IPaymentService
    {
        Task<IBaseDTO> Pay(Payment payment);   
    }

    public class PaymentService : IPaymentService
    {

        private readonly QueueContext _context;

        public PaymentService(QueueContext context) => this._context = context;

        public async Task<IBaseDTO> Pay(Payment payment)
        {
            var error = Validate(payment);
            if (error != null) return await Task.FromResult(error);

            Enqueue(payment);
            return new OkDTO(payment.UUID);
        }

        // Private

        ErrorsDTO Validate(Payment payment){
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