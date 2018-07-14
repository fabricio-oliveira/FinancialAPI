using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using System.Threading.Tasks;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<IBaseDTO> EnqueueToReceive(Receipt receipt);
        Task<IBaseDTO> Receive(Receipt receipt);
    }

    public class ReceiptService : EntryService<Receipt>, IReceiptService
    {

        private readonly ReceiptQueue _queue;

        public ReceiptService(ReceiptQueue queue) => this._queue = queue;

        public async Task<IBaseDTO> EnqueueToReceive(Receipt receipt)
        {
            var error = Validate(receipt);
            if (error != null) return await Task.FromResult(error);

            _queue.Enqueue(receipt);
            return new OkDTO(receipt.UUID);
        }

        public Task<IBaseDTO> Receive(Receipt receipt)
        {
            throw new System.NotImplementedException();
        }

        // Private

        protected override ErrorsDTO Validate(Receipt entry)
        {
            var errors = base.Validate(entry);

            return errors;
        }

    }
}