using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public class ReceiptQueue : GenericQueue<Receipt>
    {
        public ReceiptQueue(QueueContext context):base(context, context.ReceiptQueueName) {}

    }
}
