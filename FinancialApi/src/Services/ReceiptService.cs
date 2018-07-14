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
        Task<IBaseDTO> EnqueueToReceive(Receipt receipt);
        Task<IBaseDTO> Receive(Receipt receipt);
    }

    public class ReceiptService : EntryService<Receipt>, IReceiptService
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

        public async Task<IBaseDTO> EnqueueToReceive(Receipt receipt)
        {
            var error = Validate(receipt);
            if (error != null) return await Task.FromResult(error);

            _mainQueue.Enqueue(receipt);
            return new OkDTO(receipt.UUID);
        }

        public async Task<IBaseDTO> Receive(Receipt receipt)
        {
            var error = Validate(receipt);
            if (error.HasErrors()) return await Task.FromResult(error);

            updateBalance(receipt);

            return new OkDTO(receipt.UUID);
        }

        private void updateBalance(Receipt receipt)
        {
            try
            {
                using (this._entryRepository.BeginTransaction())
                {
                    this._entryRepository.Save(receipt);

                    var account = this._accountRepository.FindOrCreate(receipt.DestinationAccount,
                                                                       receipt.DestinationBank,
                                                                       receipt.TypeAccount,
                                                                       receipt.DestinationIdentity);

                    var balance = this._balanceRepository.FindOrCreateBy(account, DateTime.Today);

                    balance.Inputs.Add(new EntryDTO(receipt.DateEntry.GetValueOrDefault(),
                                                     receipt.Value.GetValueOrDefault()));

                    balance.Charges.Add(new EntryDTO(receipt.DateEntry.GetValueOrDefault(),
                                                     receipt.FinancialCharges.GetValueOrDefault()));

                    this._balanceRepository.Update(balance);
                    this._entryRepository.Commit();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                this._mainQueue.Enqueue(receipt, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (receipt.attempts > MAX_RETRY)
                    this._errorQueue.Enqueue(receipt);
                else
                {
                    receipt.errors = e.Message;
                    this._mainQueue.Enqueue(receipt, 3 * 60000); //3 minutes
                }
            }
        }

        // Private

        protected override ErrorsDTO Validate(Receipt entry)
        {
            var errors = base.Validate(entry);

            return errors;
        }

    }
}