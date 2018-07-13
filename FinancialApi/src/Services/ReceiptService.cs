using FinancialApi.Models.DTO.Request;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Queue;
using System.Threading.Tasks;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<IBaseDTO> EnqueueToReceive(ReceiptDTO receipt);
        Task<IBaseDTO> Receive(ReceiptDTO receipt);
    }

    public class ReceiptService : GenericService<ReceiptDTO>, IReceiptService
    {

        private readonly ReceiptQueue _queue;

        public ReceiptService(ReceiptQueue queue) => this._queue = queue;

        public async Task<IBaseDTO> EnqueueToReceive(ReceiptDTO receipt)
        {
            var error = Validate(receipt);
            if (error != null) return await Task.FromResult(error);

            _queue.Enqueue(receipt);
            return new OkDTO(receipt.UUID);
        }

        public Task<IBaseDTO> Receive(ReceiptDTO receipt)
        {
            throw new System.NotImplementedException();
        }

        // Private

        protected override ErrorsDTO Validate(ReceiptDTO entry)
        {
            var errors = base.Validate(entry);

            return errors;
        }

    }
}