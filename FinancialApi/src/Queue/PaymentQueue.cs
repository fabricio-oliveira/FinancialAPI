using System.Text;
using FinancialApi.Models.Entity;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class PaymentQueue :GenericQueue<Payment>
    {
        public PaymentQueue(QueueContext context):base(context, context.PaymentQueueName) {}
    }
}
