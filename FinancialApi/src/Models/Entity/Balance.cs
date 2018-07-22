using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using FinancialApi.Models.DTO;
using System.Linq;

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

        [Column("total", TypeName = "numeric(10,2)")]
        [JsonProperty(PropertyName = "total")]
        public decimal Total { get; set; }

        [Column("day_position", TypeName = "numeric(5,2)")]
        [JsonProperty(PropertyName = "posicao_do_dia")]
        public decimal? DayPosition { get; set; }


        //InPuts
        [Column("inputs", TypeName = "nvarchar(4000)")]
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

        //Outputs
        [Column("outputs", TypeName = "nvarchar(4000)")]
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

        //charges
        [Column("charges", TypeName = "nvarchar(4000)")]
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

        //RelationShips

        //hasMany
        [NotMapped]
        [JsonProperty(PropertyName = "entradas")]
        public ICollection<ShortEntryDTO> Inputs { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "saidas")]
        public ICollection<ShortEntryDTO> Outputs { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "encargos")]
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
                DayPosition = (Total / yestarday - 1);
            }
        }
    }
}
