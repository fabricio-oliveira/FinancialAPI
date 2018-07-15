using System;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class ReceiptQueue : GenericQueue<Entry>
    {
        public ReceiptQueue(QueueContext context) : base(context, context.ReceiptQueueName) { }

    }
}
