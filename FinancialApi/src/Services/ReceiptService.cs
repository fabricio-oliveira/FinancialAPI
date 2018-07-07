using FinancialApi.Models.Response;
using FinancialApi.Models.Entity;
using System.Threading.Tasks;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<Base> Receive(Receipt receipt);
    }

    public class ReceiptService : IReceiptService
    {
        public ReceiptService(QueueContext queue)
        {
            
        }

        public async Task<Base> Receive(Receipt receipt){
            return await Task.FromResult(new Errors());
        }
    }

}