using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public interface IReceiptQueue
    {
        void Enqueue(Entry t, int? delay = null);
        Entry Dequeue();
    }
}
