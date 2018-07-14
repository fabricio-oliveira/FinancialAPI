using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public class PaymentQueue : GenericQueue<Entry>
    {
        public PaymentQueue(QueueContext context) : base(context, context.PaymentQueueName) { }
    }
}
