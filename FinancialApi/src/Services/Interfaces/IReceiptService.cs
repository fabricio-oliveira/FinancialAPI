using System.Threading.Tasks;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<IBaseDTO> EnqueueToReceive(Entry receipt);
        Task<IBaseDTO> Receive(Entry receipt);
    }
}
