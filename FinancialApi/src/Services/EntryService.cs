using System;
using System.Threading.Tasks;
using FinancialApi.Models.DTO;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Repositories;

namespace FinancialApi.Services
{
    public abstract class EntryService
    {
        readonly IQueue<Entry> _mainQueue;

        protected readonly IAccountRepository _accountRepository;
        protected readonly IBalanceRepository _balanceRepository;
        protected readonly IEntryRepository _entryRepository;

        protected EntryService(IQueue<Entry> mainQueue,
                               IAccountRepository accountRepository,
                               IBalanceRepository balanceRepository,
                               IEntryRepository entryRepository)
        {
            _mainQueue = mainQueue;
            _entryRepository = entryRepository;
            _accountRepository = accountRepository;
            _balanceRepository = balanceRepository;
        }

        protected abstract ErrorsDTO Validate(Entry entry);

        protected void UpdateBalance(Entry entry)
        {
            using (_entryRepository.BeginTransaction())
            {
                //Entry
                this._entryRepository.Save(entry);

                //Account
                var account = _accountRepository.FindOrCreateBy(entry.DestinationAccount,
                                                                   entry.DestinationBank,
                                                                   entry.TypeAccount,
                                                                   entry.DestinationIdentity);

                //Balance
                var currentBalance = _balanceRepository.FindOrCreateBy(account, entry.DateToPay.GetValueOrDefault());


                var entryDto = new ShortEntryDTO(entry.DateEntry.GetValueOrDefault(),
                                                 entry.Value.GetValueOrDefault());
                if (entry.IsPayment())
                {
                    currentBalance.Outputs.Add(entryDto);
                    currentBalance.Total -= entry.Value.GetValueOrDefault(0);
                }
                else
                {
                    currentBalance.Inputs.Add(entryDto);
                    currentBalance.Total += entry.Value.GetValueOrDefault(0);

                }


                if (entry.FinancialCharges > 0)
                    currentBalance.Charges.Add(new ShortEntryDTO(entry.DateEntry.GetValueOrDefault(),
                                                     entry.FinancialCharges.GetValueOrDefault(0)));

                currentBalance.Total -= entry.FinancialCharges.GetValueOrDefault(0);

                _balanceRepository.UpdateDayPosition(currentBalance);
                _balanceRepository.Update(currentBalance);

                //Commit
                _entryRepository.Commit();
            }

        }


        protected async Task<IBaseDTO> EnqueueToEntry(Entry entry)
        {
            var error = Validate(entry);
            if (error.HasErrors()) return await Task.FromResult(error);

            _mainQueue.Enqueue(entry);
            return new OkDTO(entry.UUID);
        }

        public async Task<IBaseDTO> ProcessEntry(Entry entry)
        {
            var error = Validate(entry);
            if (error.HasErrors()) return await Task.FromResult(error);

            UpdateBalance(entry);

            return new OkDTO(entry.UUID);
        }

    }

}