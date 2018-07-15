using FinancialApi.Models.Entity;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public interface IQueue
    {
        void Enqueue(Entry t, int? delay = null);
        Entry Dequeue();
        void SetConsumer(AsyncEventHandler<BasicDeliverEventArgs> consumer);
    }
}
