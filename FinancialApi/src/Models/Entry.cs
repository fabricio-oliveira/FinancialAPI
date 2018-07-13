using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.Models.Entity
{
    public class Entry
    {

        public Entry(DateTime date, decimal value, Balance balance)
        {
            this.Date = date;
            this.Value = value;
            this.Balance = balance;
            this.BalanceId = balance?.Id;
        }

        public Entry(){}

        [JsonIgnore]
        [Key]
        public long? Id { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "data")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "valor")]
        [Column("value")]
        public decimal Value { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //RelationShips

        public long? BalanceId { get; set; }

        [ForeignKey("BalanceId")]
        public Balance Balance { get; set; }

    }
}
