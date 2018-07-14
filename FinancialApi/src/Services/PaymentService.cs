using System.Threading.Tasks;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.Utils;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using System;

namespace FinancialApi.Services 
{
    public interface IPaymentService
    {
        Task<IBaseDTO> EnqueueToPay(Payment payment);
        Task<IBaseDTO> Pay(Payment payment);
    }

    public class PaymentService : EntryService<Payment>, IPaymentService 
    {

        private const decimal ESPECIAL_LIMIT = -20.000m; 

        private readonly PaymentQueue _queue;

        private readonly BalanceRepository _balanceRepository;
        private readonly AccountRepository _accountRepository;
        private readonly EntryRepository _entryRepository;

        public PaymentService(PaymentQueue queue, 
                              BalanceRepository balanceRepository,
                              AccountRepository accountRepository,
                              EntryRepository entryRepository)
        {
            this._queue = queue;
            this._balanceRepository = balanceRepository;
            this._accountRepository = accountRepository;
            this._entryRepository = entryRepository;
        }

        public async Task<IBaseDTO> EnqueueToPay(Payment payment)
        {
            var error = Validate(payment);
            if (error.HasErrors()) return await Task.FromResult(error);

            _queue.Enqueue(payment);
            return new OkDTO(payment.UUID);
        }


        public async Task<IBaseDTO> Pay(Payment payment)
        {
            var error = Validate(payment);
            if (error.HasErrors()) return await Task.FromResult(error);

            updateBalance(payment);

            return new OkDTO(payment.UUID);
        }




        //Private or protected methods

        private void updateBalance(Payment payment)
        {
            //TODO add transaction context
            this._entryRepository.Save(payment);

            var account = this._accountRepository.FindOrCreate(payment.DestinationAccount,
                                                               payment.DestinationBank,
                                                               payment.TypeAccount,
                                                               payment.DestinationIdentity);

            var balance = this._balanceRepository.GetBy(account, DateTime.Today);

            balance.Outputs.Add(new EntryDTO(payment.DateEntry.GetValueOrDefault(),
                                            payment.Value.GetValueOrDefault()));

            balance.Charges.Add(new EntryDTO(payment.DateEntry.GetValueOrDefault(),
                                             payment.FinancialCharges.GetValueOrDefault()));


            this._balanceRepository.Update(balance);
                                                        
        }


        protected override ErrorsDTO Validate(Payment entry)
        {
            var errors = base.Validate(entry);

            if (!HasLimit(entry))
                errors.Add(entry.GetJSonFieldName("Value"), "Account don't have especial limit");

            return errors;
        }


        private bool HasLimit(Payment entry)
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