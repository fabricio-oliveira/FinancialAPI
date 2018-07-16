using System.Text;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public class InterestQueue : GenericQueue<InterestQueue>
    {
        public InterestQueue(QueueContext context) : base(context) { }

        protected override string QueueName() => _context.InterestQueueName;
    }
}
