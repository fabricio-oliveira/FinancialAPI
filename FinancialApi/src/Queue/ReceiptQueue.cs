using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public class ReceiptQueue : GenericQueue<Entry>, IReceiptQueue
    {
        public ReceiptQueue(QueueContext context) : base(context) { }

        protected override string QueueName() => _context.ReceiptQueueName;

    }
}
