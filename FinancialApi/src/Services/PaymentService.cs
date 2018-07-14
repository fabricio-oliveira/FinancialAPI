using System.Threading.Tasks;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.Utils;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using System;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;

namespace FinancialApi.Services
{
    public interface IPaymentService
    {
        Task<IBaseDTO> EnqueueToPay(Entry entry);
        Task<IBaseDTO> Pay(Entry entry);
    }

    public class PaymentService : EntryService, IPaymentService
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
            //this._mainQueue.SetConsumer = Pay;
        }

        public async Task<IBaseDTO> EnqueueToPay(Entry entry)
        {
            var error = Validate(entry);
            if (error.HasErrors()) return await Task.FromResult(error);

            _mainQueue.Enqueue(entry);
            return new OkDTO(entry.UUID);
        }

        public async Task<IBaseDTO> Pay(Entry entry)
        {
            var error = Validate(entry);
            if (error.HasErrors()) return await Task.FromResult(error);

            UpdateBalance(entry);

            return new OkDTO(entry.UUID);
        }


        //Private or protected methods

        private void UpdateBalance(Entry entry)
        {
            try
            {
                using (this._entryRepository.BeginTransaction())
                {
                    this._entryRepository.Save(entry);

                    var account = this._accountRepository.FindOrCreate(entry.DestinationAccount,
                                                                       entry.DestinationBank,
                                                                       entry.TypeAccount,
                                                                       entry.DestinationIdentity);

                    var balance = this._balanceRepository.FindOrCreateBy(account, DateTime.Today);

                    balance.Outputs.Add(new ShortEntryDTO(entry.DateEntry.GetValueOrDefault(),
                                                     entry.Value.GetValueOrDefault()));

                    balance.Charges.Add(new ShortEntryDTO(entry.DateEntry.GetValueOrDefault(),
                                                     entry.FinancialCharges.GetValueOrDefault()));

                    this._balanceRepository.Update(balance);
                    this._entryRepository.Commit();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                this._mainQueue.Enqueue(entry, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                    this._errorQueue.Enqueue(entry);
                else
                {
                    entry.Errors = e.Message;
                    this._mainQueue.Enqueue(entry, 3 * 60000); //3 minutes
                }
            }
        }


        protected override ErrorsDTO Validate(Entry entry)
        {
            var errors = base.Validate(entry);

            if (!HasLimit(entry))
                errors.Add(entry.GetJSonFieldName("Value"), "Account don't have especial limit");

            return errors;
        }


        private bool HasLimit(Entry entry)
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