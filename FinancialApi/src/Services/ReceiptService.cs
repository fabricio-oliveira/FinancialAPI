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
    public class ReceiptService : EntryService, IReceiptService
    {

        private const int MAX_RETRY = 3;


        public ReceiptService(IReceiptQueue mainQueue,
                              IAccountRepository accountRepository,
                              IBalanceRepository balanceRepository,
                              IEntryRepository entryRepository)
            : base(mainQueue,
                   accountRepository,
                   balanceRepository,
                   entryRepository)
        { }

        public async Task<IBaseDTO> EnqueueToReceive(Entry entry)
        {
            return await EnqueueToEntry(entry);
        }

        public async Task<IBaseDTO> Receive(Entry entry)
        {
            return await ProcessEntry(entry);
        }

        // Private

        protected override ErrorsDTO Validate(Entry entry)
        {
            return new ErrorsDTO();
        }

    }
}