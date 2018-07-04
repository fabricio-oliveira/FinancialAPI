using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.Models.Entity
{
    public class Receipt : Entry
     {

        public Receipt() { }

        public Receipt(string Description, string DestinationAccount, string DestinationBank,
                       string TypeAccount, string DestinationIdentity, decimal Value,
                       decimal FinancialCharges, DateTime Date) :
                base(Description, DestinationAccount, DestinationBank, TypeAccount, DestinationIdentity,
                        Value, FinancialCharges, Date){ }
        
     }
}
