using System.Text;
using FinancialApi.Models.Entity;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class ReceiptQueue : GenericQueue<Receipt>
    {
        public ReceiptQueue(QueueContext context):base(context, context.ReceiptQueueName) {}

    }
}
