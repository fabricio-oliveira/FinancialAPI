using FinancialApi.Models.Response;
using FinancialApi.Models.Entity;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Base receive(Receipt receipt);
    }

    class ReceiptService : IReceiptService
    {
        public ReceiptService(QueueContext queue)
        {
            
        }

        public Base receive(Receipt receipt){
            return null;
        }
    }

}