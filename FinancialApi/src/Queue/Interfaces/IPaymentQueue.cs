using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public interface IPaymentQueue
    {
        void Enqueue(Entry t, int? delay = null);
        Entry Dequeue();
    }
}
