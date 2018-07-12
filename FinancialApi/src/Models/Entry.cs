using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.src.Models.Entity
{
    public class Entry
    {

        public Entry(DateTime date, decimal value, CashFlow cashFlow)
        {
            this.Date = date;
            this.Value = value;
            this.CashFlow = cashFlow;
            this.CashFlowId = cashFlow?.ID;
        }

        public Entry(){}

        [JsonIgnore]
        [Key]
        public int Id { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "data")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "valor")]
        [Column("value")]
        public decimal Value { get; set; }

        //RelationShips

        public int? CashFlowId { get; set; }

        [ForeignKey("CashFlowId")]
        public CashFlow CashFlow { get; set; }

    }
}
