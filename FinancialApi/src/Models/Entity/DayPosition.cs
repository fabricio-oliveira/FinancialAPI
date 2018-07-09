using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FinancialApi.Models.Entity;
using Newtonsoft.Json;

namespace FinancialApi.src.Models.Entity
{
    [Table("CashFlow")]
    public class CashFlow
    {
        public CashFlow()
        {
        }

        [JsonProperty(PropertyName = "data")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "entradas")]
        public ICollection<Receipt> inputs { get; set;  }
        
        [JsonProperty(PropertyName = "saidas")]
        public ICollection<Payment> outpus { get; set; }
        
        [JsonProperty(PropertyName = "encargos")]
        public ICollection<Entry> FinancialCharges { get; set; }

        [JsonProperty(PropertyName = "total")]
        public decimal Total { get; set; }

        [JsonProperty(PropertyName = "posicao_do_dia")]
        public decimal DayPosition { get; set; }

        [JsonProperty(PropertyName = "conta_destino")]
        public string DestinationAccount { get; set; }

        [JsonProperty(PropertyName = "banco_destino")]
        public string DestinationBank { get; set; }

    }
}
