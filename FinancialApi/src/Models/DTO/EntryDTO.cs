using System;
using Newtonsoft.Json;

namespace FinancialApi.Models.DTO
{
    public class EntryDTO
    {
        private Func<DateTime> getValueOrDefault;
        private decimal? value;

        public EntryDTO(DateTime date, decimal value)
        {
            this.Date = date;
            this.Value = value;
        }

        public EntryDTO() { }

        public EntryDTO(Func<DateTime> getValueOrDefault, decimal? value)
        {
            this.getValueOrDefault = getValueOrDefault;
            this.value = value;
        }

        [JsonProperty(PropertyName = "data")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "valor")]
        public decimal Value { get; set; }
    }
}
