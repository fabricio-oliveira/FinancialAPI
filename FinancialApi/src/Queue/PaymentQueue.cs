using System;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class PaymentQueue : GenericQueue<Entry>
    {
        public PaymentQueue(QueueContext context) : base(context, context.PaymentQueueName) { }
    }
}
