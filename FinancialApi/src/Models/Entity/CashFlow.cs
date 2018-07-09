using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FinancialApi.Models.DTO.Request;
using Newtonsoft.Json;

namespace FinancialApi.src.Models.Entity
{
    [Table("CashFlow")]
    public class CashFlow
    {
        public CashFlow()
        {
        }


        public CashFlow(DateTime date, ICollection<Input> inputs, ICollection<Output> outputs,
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

        [JsonProperty(PropertyName = "data")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "entradas")]
        public ICollection<Input> Inputs { get; set;  }
        
        [JsonProperty(PropertyName = "saidas")]
        public ICollection<Output> Outpus { get; set; }

        [JsonProperty(PropertyName = "encargos")]
        public ICollection<Charge> Charges { get; set; }

        [JsonProperty(PropertyName = "total")]
        public decimal Total { get; set; }

        [JsonProperty(PropertyName = "posicao_do_dia")]
        public decimal DayPosition { get; set; }

        [JsonIgnore]
        public Account Account { get; set; }
    }
}
