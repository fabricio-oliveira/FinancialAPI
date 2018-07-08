using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using System.Threading.Tasks;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<IBaseDTO> Receive(Receipt receipt);
    }

    public class ReceiptService : IReceiptService
    {

        private readonly ReceiptQueue _queue;

        public ReceiptService(ReceiptQueue queue) => this._queue = queue;

        public async Task<IBaseDTO> Receive(Receipt receipt)
        {
            var error = Validate(receipt);
            if (error != null) return await Task.FromResult(error);

            _queue.Enqueue(receipt);
            return new OkDTO(receipt.UUID);
        }

        // Private

        ErrorsDTO Validate(Receipt receipt)
        {
            return null;
        }

    }
}