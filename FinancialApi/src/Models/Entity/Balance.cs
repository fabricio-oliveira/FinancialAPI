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
                       ICollection<ShortEntryDTO> charges, decimal total, decimal? dayPosition,
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
        public decimal? DayPosition { get; set; }


        //InPuts
        [Column("inputs", TypeName = "nvarchar(max)")]
        [JsonIgnore]
        public string _Inputs
        {
            get { return Inputs == null ? null : JsonConvert.SerializeObject(Inputs); }
            set
            {
                if (value == null)
                    Inputs = null;
                else
                    Inputs = JsonConvert.DeserializeObject<List<ShortEntryDTO>>(value);
            }
        }

        [JsonProperty(PropertyName = "entradas")]
        internal List<ShortEntryDTO> FormatInput => _Inputs == null ? new List<ShortEntryDTO>() : JsonConvert.DeserializeObject<List<ShortEntryDTO>>(_Inputs);


        //Outputs
        [Column("outputs", TypeName = "nvarchar(max)")]
        [JsonIgnore]
        public string _Outputs
        {
            get { return Outputs == null ? null : JsonConvert.SerializeObject(Outputs); }
            set
            {
                if (value == null)
                    Outputs = null;
                else
                {
                    Outputs = JsonConvert.DeserializeObject<List<ShortEntryDTO>>(value);
                }
            }
        }

        [JsonProperty(PropertyName = "saidas")]
        internal List<ShortEntryDTO> FormatOutput => _Inputs == null ? new List<ShortEntryDTO>() : JsonConvert.DeserializeObject<List<ShortEntryDTO>>(_Outputs);


        //charges
        [Column("charges", TypeName = "nvarchar(max)")]
        [JsonIgnore]
        public string _Charges
        {
            get { return Charges == null ? null : JsonConvert.SerializeObject(Charges); }
            set
            {
                if (value == null)
                    Charges = null;
                else
                    Charges = JsonConvert.DeserializeObject<List<ShortEntryDTO>>(value);
            }
        }

        [JsonProperty(PropertyName = "encargos")]
        internal List<ShortEntryDTO> FormatCharges => _Charges == null ? new List<ShortEntryDTO>() : JsonConvert.DeserializeObject<List<ShortEntryDTO>>(_Charges);
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
        [Required]
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [JsonIgnore]
        public long? AccountId { get; set; }

        [JsonIgnore]
        [Column("Closed")]
        public bool Closed { get; set; }

        // optimistic lock
        [Timestamp]
        [JsonIgnore]
        public byte[] RowVersion { get; set; }


        public void UpdateDayPostionNewDay(decimal yestarday)
        {
            if (Total == 0m && yestarday == 0)
            {
                DayPosition = 0.0m; // 0 to any value I aproximadament
            }
            else if (yestarday == 0m)
            {
                DayPosition = null; // 0 to any value I aproximadament
            }
            else
            {
                DayPosition = -1 * (1 - Total / yestarday);
            }
        }

        public void UpdateDayPostionNewEntry(decimal newEntry)
        {
            if (DayPosition != null)
            {
                DayPosition = (Total + newEntry) * DayPosition / Total;
            }
        }
    }
}
