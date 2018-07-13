using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.Models.Entity
{
    [Table("Balance")]
    public class Balance
    {
        public Balance()
        {
        }


        public Balance(DateTime date, ICollection<Input> inputs, ICollection<Output> outputs,
                        ICollection<Charge> charges, decimal total, decimal dayPosition,
                        Account account)
        {
            this.Date = date;
            this.Inputs = inputs;
            this.Outpus = outputs;
            this.Charges = charges;
            this.Total = total;
            this.DayPosition = dayPosition;
            this.Account = account;
        }

        [Key]
        [JsonIgnore]
        public long? Id { get; set; }

        [JsonProperty(PropertyName = "data")]
        public DateTime Date { get; set; }

        [Column("total")]
        [JsonProperty(PropertyName = "total")]
        public decimal Total { get; set; }

        [Column("day_position")]
        [JsonProperty(PropertyName = "posicao_do_dia")]
        public decimal DayPosition { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //RelationShips

        [JsonProperty(PropertyName = "entradas")]
        public ICollection<Input> Inputs { get; set;  }
        
        [JsonProperty(PropertyName = "saidas")]
        public ICollection<Output> Outpus { get; set; }

        [JsonProperty(PropertyName = "encargos")]
        public ICollection<Charge> Charges { get; set; }

        [JsonIgnore]
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [JsonIgnore]
        public long? AccountId { get; set; }

    }
}
