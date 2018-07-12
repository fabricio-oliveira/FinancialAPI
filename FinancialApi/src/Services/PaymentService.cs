using System;
using System.Text;
using System.Threading.Tasks;
using FinancialApi.Models.DTO.Request;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.src.Utils;
using FinancialApi.Models.DTO.Response;

namespace FinancialApi.Services 
{
    public interface IPaymentService
    {
        Task<IBaseDTO> Pay(PaymentDTO payment);   
    }

    public class PaymentService : GenericService<PaymentDTO>, IPaymentService 
    {

        private const decimal ESPECIAL_LIMIT = -20.000m; 

        private readonly PaymentQueue _queue;
        private readonly CashFlowRepository _cashFlowRepository;
        private readonly InputRepository _inputRepository;
        private readonly OutputRepository _outputRepository;
        private readonly ChargeRepository _chargesRepository;
        private readonly AccountRepository _accountRepository;

        public PaymentService(PaymentQueue queue, 
                              CashFlowRepository cashFlowRepository,
                              InputRepository inputRepository,
                              OutputRepository outputRepository,
                              ChargeRepository chargeRepository,
                              AccountRepository accountRepository)
        {
            this._queue = queue;
            this._cashFlowRepository = cashFlowRepository;
            this._inputRepository = inputRepository;
            this._outputRepository = outputRepository;
            this._chargesRepository = chargeRepository;
            this._accountRepository = accountRepository;
        }

        public async Task<IBaseDTO> Pay(PaymentDTO payment)
        {
            var error = Validate(payment);
            if (error.HasErrors()) 
                return await Task.FromResult(error);

            _queue.Enqueue(payment);
            return new OkDTO(payment.UUID);
        }


        protected override ErrorsDTO Validate(PaymentDTO entry)
        {
            var errors = base.Validate(entry);

            if (!HasEspecialLimit(entry))
                errors.Add(entry.GetJSonFieldName("Value"), "Account don't have especial limit");

            return errors;
        }


        private bool HasEspecialLimit(PaymentDTO entry)
        {
            var account = _accountRepository.FindOrCreate(number: entry.DestinationAccount, 
                                                          bank: entry.DestinationBank,
                                                          type: entry.TypeAccount,
                                                          identity: entry.DestinationIdentity);

            var flow =  _cashFlowRepository.LastCashFlow(account);

            return (flow.Total - entry.Value - entry.FinancialCharges) >= ESPECIAL_LIMIT;
        }
    }

}