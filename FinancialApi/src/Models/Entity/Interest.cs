using System.ComponentModel.DataAnnotations;
using System;
using Newtonsoft.Json;
using FinancialApi.validate;

namespace FinancialApi.Models.Entity
{
    public abstract class Interest
    {

        public Interest(string Description, string DestinationAccount, string DestinationBank,
                     string TypeAccount, string DestinationIdentity, decimal Value, decimal FinancialCharges,
                     DateTime Date):base(){
            this.UUID = Guid.NewGuid().ToString();
            this.Value = Value;
            this.Date = Date;
        }

        public Interest(){
            this.UUID = Guid.NewGuid().ToString();
        }

        [Key]
        public long? Id { get; set; }

        public string UUID { get; set; }

        public decimal? Value { get; set; }

        public DateTime? Date { get; set; }

    }
}