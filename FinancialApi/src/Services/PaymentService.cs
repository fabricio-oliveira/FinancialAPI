using System.Threading.Tasks;
using FinancialApi.Models.DTO.Request;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.Utils;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using System;

namespace FinancialApi.Services 
{
    public interface IPaymentService
    {
        Task<IBaseDTO> EnqueueToPay(PaymentDTO payment);
        Task<IBaseDTO> Pay(PaymentDTO payment);
    }

    public class PaymentService : GenericService<PaymentDTO>, IPaymentService 
    {

        private const decimal ESPECIAL_LIMIT = -20.000m; 

        private readonly PaymentQueue _queue;
        private readonly BalanceRepository _balanceRepository;
        private readonly AccountRepository _accountRepository;

        public PaymentService(PaymentQueue queue, 
                              BalanceRepository balanceRepository,
                              AccountRepository accountRepository)
        {
            this._queue = queue;
            this._balanceRepository = balanceRepository;
            this._accountRepository = accountRepository;
        }

        public async Task<IBaseDTO> EnqueueToPay(PaymentDTO payment)
        {
            var error = Validate(payment);
            if (error.HasErrors()) return await Task.FromResult(error);

            _queue.Enqueue(payment);
            return new OkDTO(payment.UUID);
        }


        public async Task<IBaseDTO> Pay(PaymentDTO payment)
        {
            var error = Validate(payment);
            if (error.HasErrors()) return await Task.FromResult(error);



            return new OkDTO(payment.UUID);
        }


        //Private or protected methods

        private void updateBalance(PaymentDTO payment)
        {
            var account = this._accountRepository.FindOrCreate(payment.DestinationAccount,
                                                               payment.DestinationBank,
                                                               payment.TypeAccount,
                                                               payment.DestinationIdentity);

            var balance = this._balanceRepository.GetBy(account, DateTime.Today);
            balance.Inputs.Add(new Input());
                                                        
        }


        protected override ErrorsDTO Validate(PaymentDTO entry)
        {
            var errors = base.Validate(entry);

            if (!HasLimit(entry))
                errors.Add(entry.GetJSonFieldName("Value"), "Account don't have especial limit");

            return errors;
        }


        private bool HasLimit(PaymentDTO entry)
        {
            var account = _accountRepository.FindOrCreate(number: entry.DestinationAccount, 
                                                          bank: entry.DestinationBank,
                                                          type: entry.TypeAccount,
                                                          identity: entry.DestinationIdentity);

            var flow =  _balanceRepository.LastByOrDefault(account);

            return (flow.Total - entry.Value - entry.FinancialCharges) >= ESPECIAL_LIMIT;
        }
    }

}