using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using FinancialApi.Models.DTO;

namespace FinancialApi.Models.Entity
{
    [Table("Balances")]
    public class Balance
    {
        public Balance()
        {
        }


        public Balance(DateTime date, ICollection<ShortEntryDTO> inputs, ICollection<ShortEntryDTO> outputs,
                       ICollection<ShortEntryDTO> charges, decimal total, decimal dayPosition,
                        Account account)
        {
            this.Date = date;
            this.Inputs = inputs;
            this.Outputs = outputs;
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

        [Column(TypeName = "nvarchar(max)")]
        [JsonProperty(PropertyName = "entradas")]
        internal string _Inputs
        {
            get { return Inputs == null ? null : JsonConvert.SerializeObject(Inputs); }
            set
            {
                if (value == null)
                    Inputs = null;

                Inputs = JsonConvert.DeserializeObject<List<ShortEntryDTO>>(value);
            }
        }

        [Column(TypeName = "nvarchar(max)")]
        [JsonProperty(PropertyName = "saidas")]
        internal string _Outputs
        {
            get { return Outputs == null ? null : JsonConvert.SerializeObject(Outputs); }
            set
            {
                if (value == null)
                    Outputs = null;

                Outputs = JsonConvert.DeserializeObject<List<ShortEntryDTO>>(value);
            }

        }

        [Column(TypeName = "nvarchar(max)")]
        [JsonProperty(PropertyName = "encargos")]
        internal string _Charges
        {
            get { return Charges == null ? null : JsonConvert.SerializeObject(Charges); }
            set
            {
                if (value == null)
                    Charges = null;

                Charges = JsonConvert.DeserializeObject<List<ShortEntryDTO>>(value);
            }
        }

        //RelationShips

        //hasMany
        [NotMapped]
        public ICollection<ShortEntryDTO> Inputs { get; set; }

        [NotMapped]
        public ICollection<ShortEntryDTO> Outputs { get; set; }

        [NotMapped]
        public ICollection<ShortEntryDTO> Charges { get; set; }


        //Belongs
        [JsonIgnore]
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [JsonIgnore]
        [Column("Closed")]
        public bool Closed { get; set; }

        [JsonIgnore]
        public long? AccountId { get; set; }

        // optimistic lock
        [Timestamp]
        public byte[] RowVersion { get; set; }


        public void UpdateDayPostionNewDay(decimal yestarday)
        {
            if (Total == 0m && yestarday == 0m)
                DayPosition = 0.0m;
            else if (yestarday == 0m)
                DayPosition = 1.0m; // 0 to any value I will considere 100%
            else
                DayPosition = -1 * (1 - Total / yestarday);
        }

        public void UpdateDayPostionNewEntry(decimal newEntry)
        {
            if (DayPosition == 0m) DayPosition = 1m;
            DayPosition = Total == 0m ? 1.0m : (Total + newEntry) * DayPosition / Total;
        }
    }
}
