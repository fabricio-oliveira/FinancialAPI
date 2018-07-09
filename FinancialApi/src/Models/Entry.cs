using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.src.Models.Entity
{
    public class Entry
    {
        
        public Entry()
        {
        }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "data")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "valor")]
        public decimal Value { get; set; }

        public CashFlow CashFlow { get; set; }

    }
}
