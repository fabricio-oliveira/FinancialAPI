using System.Threading.Tasks;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.Utils;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using System;
using Microsoft.EntityFrameworkCore;

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
        private const int MAX_RETRY = 3;

        private readonly PaymentQueue _mainQueue;
        private readonly ErrorQueue _errorQueue;

        private readonly BalanceRepository _balanceRepository;
        private readonly AccountRepository _accountRepository;
        private readonly EntryRepository _entryRepository;

        public PaymentService(PaymentQueue mainQueue,
                              ErrorQueue errorQueue,
                              BalanceRepository balanceRepository,
                              AccountRepository accountRepository,
                              EntryRepository entryRepository)
        {
            this._mainQueue = mainQueue;
            this._errorQueue = errorQueue;
            this._balanceRepository = balanceRepository;
            this._accountRepository = accountRepository;
            this._entryRepository = entryRepository;
        }

        public async Task<IBaseDTO> EnqueueToPay(Payment payment)
        {
            var error = Validate(payment);
            if (error.HasErrors()) return await Task.FromResult(error);

            _mainQueue.Enqueue(payment);
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
            try
            {
                using (this._entryRepository.BeginTransaction())
                {
                    this._entryRepository.Save(payment);

                    var account = this._accountRepository.FindOrCreate(payment.DestinationAccount,
                                                                   payment.DestinationBank,
                                                                   payment.TypeAccount,
                                                                   payment.DestinationIdentity);

                    var balance = this._balanceRepository.FindOrCreateBy(account, DateTime.Today);

                    balance.Outputs.Add(new EntryDTO(payment.DateEntry.GetValueOrDefault(),
                                                payment.Value.GetValueOrDefault()));

                    balance.Charges.Add(new EntryDTO(payment.DateEntry.GetValueOrDefault(),
                                                 payment.FinancialCharges.GetValueOrDefault()));

                    this._balanceRepository.Update(balance);
                    this._entryRepository.Commit();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                this._mainQueue.Enqueue(payment, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (payment.attempts > MAX_RETRY)
                    this._errorQueue.Enqueue(payment);
                else
                {
                    payment.errors = e.Message;
                    this._mainQueue.Enqueue(payment, 3 * 60000); //3 minutes
                }
            }
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

            var flow = _balanceRepository.LastByOrDefault(account);

            return (flow.Total - entry.Value - entry.FinancialCharges) >= ESPECIAL_LIMIT;
        }
    }

}