using FinancialApi.Models.DTO.Request;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Queue;
using System.Threading.Tasks;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<IBaseDTO> Receive(ReceiptDTO receipt);
    }

    public class ReceiptService : GenericService<PaymentDTO>, IReceiptService
    {

        private readonly ReceiptQueue _queue;

        public ReceiptService(ReceiptQueue queue) => this._queue = queue;

        public async Task<IBaseDTO> Receive(ReceiptDTO receipt)
        {
            var error = Validate(receipt);
            if (error != null) return await Task.FromResult(error);

            _queue.Enqueue(receipt);
            return new OkDTO(receipt.UUID);
        }

        // Private

        ErrorsDTO Validate(ReceiptDTO receipt)
        {
            return null;
        }

    }
}