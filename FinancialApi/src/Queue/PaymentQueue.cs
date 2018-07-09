using System.Text;
using FinancialApi.Models.DTO.Request;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class PaymentQueue :GenericQueue<PaymentDTO>
    {
        public PaymentQueue(QueueContext context):base(context, context.PaymentQueueName) {}
    }
}
