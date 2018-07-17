using System.Threading.Tasks;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.Utils;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;

namespace FinancialApi.Services
{
    public class PaymentService : EntryService, IPaymentService
    {

        private const decimal ESPECIAL_LIMIT = -20000.00m;

        public PaymentService(IPaymentQueue mainQueue,
                              IBalanceRepository balanceRepository,
                              IAccountRepository accountRepository,
                              IEntryRepository entryRepository)
            : base(mainQueue,
                   accountRepository,
                   balanceRepository,
                   entryRepository)
        { }

        public async Task<IBaseDTO> EnqueueToPay(Entry entry)
        {
            return await EnqueueToEntry(entry);
        }

        public async Task<IBaseDTO> Pay(Entry entry)
        {
            return await ProcessEntry(entry);
        }

        protected override ErrorsDTO Validate(Entry entry)
        {
            var errors = new ErrorsDTO();

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