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
        Task<IBaseDTO> EnqueueToPay(Entry entry);
        Task<IBaseDTO> Pay(Entry entry);
    }

    public class PaymentService : EntryService, IPaymentService
    {

        private const decimal ESPECIAL_LIMIT = -20000.00m;

        private readonly IPaymentQueue _mainQueue;

        private readonly IBalanceRepository _balanceRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IEntryRepository _entryRepository;

        public PaymentService(IPaymentQueue mainQueue,
                              IBalanceRepository balanceRepository,
                              IAccountRepository accountRepository,
                              IEntryRepository entryRepository)
        {
            this._mainQueue = mainQueue;

            this._balanceRepository = balanceRepository;
            this._accountRepository = accountRepository;
            this._entryRepository = entryRepository;
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
            using (this._entryRepository.BeginTransaction())
            {
                //Entry
                this._entryRepository.Save(entry);

                //Account
                var account = this._accountRepository.FindOrCreate(entry.DestinationAccount,
                                                                   entry.DestinationBank,
                                                                   entry.TypeAccount,
                                                                   entry.DestinationIdentity);

                //Balance
                var currentBalance = this._balanceRepository.FindOrCreateBy(account, entry.DateToPay.GetValueOrDefault());


                currentBalance.Outputs.Add(new ShortEntryDTO(entry.DateEntry.GetValueOrDefault(),
                                                 entry.Value.GetValueOrDefault()));

                currentBalance.Charges.Add(new ShortEntryDTO(entry.DateEntry.GetValueOrDefault(),
                                                 entry.FinancialCharges.GetValueOrDefault()));

                this._balanceRepository.Update(currentBalance);
                this._balanceRepository.UpdateDayPosition(currentBalance);

                //Commit
                this._entryRepository.Commit();
            }
            //SQL Server has auto rollback when exception as throw

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