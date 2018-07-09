using System.Text;
using FinancialApi.Models.DTO.Request;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class ReceiptQueue : GenericQueue<ReceiptDTO>
    {
        public ReceiptQueue(QueueContext context):base(context, context.ReceiptQueueName) {}

    }
}
