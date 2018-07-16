using FinancialApi.Models.Entity;
using RabbitMQ.Client.Events;

namespace FinancialApi.Queue
{
    public interface IQueue<T> where T : class
    {
        void Enqueue(Entry t, int? delay = null);
        T Dequeue();
        void SetConsumer(AsyncEventHandler<BasicDeliverEventArgs> consumer);
    }
}
