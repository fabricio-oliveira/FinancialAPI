using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public class ErrorQueue : GenericQueue<Entry>, IErrorQueue
    {
        public ErrorQueue(QueueContext context) : base(context, context.ErrorQueueName) { }
    }
}
