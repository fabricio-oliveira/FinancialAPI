using FinancialApi.Models.Entity;

namespace FinancialApi.Queue
{
    public interface IErrorQueue : IQueue<Entry> { }
}
