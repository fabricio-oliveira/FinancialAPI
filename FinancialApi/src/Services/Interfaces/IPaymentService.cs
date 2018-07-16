using System.Threading.Tasks;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;

namespace FinancialApi.Services
{
    public interface IPaymentService
    {
        Task<IBaseDTO> EnqueueToPay(Entry entry);
        Task<IBaseDTO> Pay(Entry entry);
    }
}
