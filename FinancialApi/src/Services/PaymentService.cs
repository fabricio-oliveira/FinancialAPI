using System;
using System.Text;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using FinancialApi.Models.DTO;
using FinancialApi.Queue;

namespace FinancialApi.Services 
{
    public interface IPaymentService
    {
        Task<IBaseDTO> Pay(Payment payment);   
    }

    public class PaymentService : IPaymentService
    {

        private readonly PaymentQueue _queue;

        public PaymentService(PaymentQueue queue) => this._queue = queue;

        public async Task<IBaseDTO> Pay(Payment payment)
        {
            var error = Validate(payment);
            if (error != null) return await Task.FromResult(error);

            _queue.Enqueue(payment);
            return new OkDTO(payment.UUID);
        }

        // Private

        ErrorsDTO Validate(Payment payment){
            return null;
        }

    }

}