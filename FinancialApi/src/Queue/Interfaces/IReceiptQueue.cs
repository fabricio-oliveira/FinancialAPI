using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public interface IReceiptQueue : IQueue<Entry> { }
}
