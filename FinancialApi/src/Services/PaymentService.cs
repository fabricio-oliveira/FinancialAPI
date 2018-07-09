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

    public class PaymentService : GenericService<Payment>, IPaymentService 
    {

        private readonly PaymentQueue _queue;
        //private readonly ;

        public PaymentService(PaymentQueue queue)
        {
            this._queue = queue;
            this.
        }


        public async Task<IBaseDTO> Pay(Payment payment)
        {
            var error = Validate(payment);
            if (error.HasErrors()) 
                return await Task.FromResult(error);

            _queue.Enqueue(payment);
            return new OkDTO(payment.UUID);
        }


        private override ErrorsDTO Validate(Payment payment)
        {
            var errors = base.Validate(payment);

            if 

            return errors;
        }
    }

}