using FinancialApi.Models.DTO;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FinancialApi.Services
{
    public interface IReceiptService
    {
        Task<IBaseDTO> EnqueueToReceive(Entry receipt);
        Task<IBaseDTO> Receive(Entry receipt);
    }

    public class ReceiptService : EntryService, IReceiptService
    {

        private const int MAX_RETRY = 3;

        private readonly ReceiptQueue _mainQueue;
        private readonly ErrorQueue _errorQueue;

        private readonly EntryRepository _entryRepository;
        private readonly AccountRepository _accountRepository;
        private readonly BalanceRepository _balanceRepository;

        public ReceiptService(ReceiptQueue mainQueue,
                              ErrorQueue errorQueue,
                              EntryRepository entryRepository,
                              AccountRepository accountRepository,
                              BalanceRepository balanceRepository)
        {
            this._mainQueue = mainQueue;
            this._errorQueue = errorQueue;
            this._entryRepository = entryRepository;
            this._accountRepository = accountRepository;
            this._balanceRepository = balanceRepository;
        }

        public async Task<IBaseDTO> EnqueueToReceive(Entry entry)
        {
            var error = Validate(entry);
            if (error.HasErrors()) return await Task.FromResult(error);

            _mainQueue.Enqueue(entry);
            return new OkDTO(entry.UUID);
        }

        public async Task<IBaseDTO> Receive(Entry entry)
        {
            var error = Validate(entry);
            if (error.HasErrors()) return await Task.FromResult(error);

            UpdateBalance(entry);

            return new OkDTO(entry.UUID);
        }

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

                    balance.Inputs.Add(new ShortEntryDTO(entry.DateEntry.GetValueOrDefault(),
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

        // Private

        protected override ErrorsDTO Validate(Entry entry)
        {
            var errors = base.Validate(entry);

            return errors;
        }

    }
}