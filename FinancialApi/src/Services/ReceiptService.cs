using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using System.Threading.Tasks;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<IBaseDTO> Receive(Receipt receipt);
    }

    public class ReceiptService : IReceiptService
    {
        public ReceiptService(QueueContext queue)
        {
            
        }

        public async Task<IBaseDTO> Receive(Receipt receipt){
            return await Task.FromResult(new ErrorsDTO());
        }
    }

}