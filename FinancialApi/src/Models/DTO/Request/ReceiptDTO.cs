using System;

namespace FinancialApi.Models.DTO.Request
{
    public class ReceiptDTO : EntryDTO
     {

        public ReceiptDTO() 
        {
            this.Type = "receipt";
        }

        public ReceiptDTO(string Description, string DestinationAccount, string DestinationBank,
                       string TypeAccount, string DestinationIdentity, decimal Value,
                       decimal FinancialCharges, DateTime Date) :
                base(Description, DestinationAccount, DestinationBank, TypeAccount, DestinationIdentity,
                        Value, FinancialCharges, Date)
        {
            this.Type = "receipt";
        }
        
     }
}
