using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public class PaymentQueue :GenericQueue<Payment>
    {
        public PaymentQueue(QueueContext context):base(context, context.PaymentQueueName) {}
    }
}
