using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public interface IPaymentQueue : IQueue<Entry> { }
}
