using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public interface IErrorQueue
    {
        void Enqueue(Entry t, int? delay = null);
        Entry Dequeue();
    }
}
