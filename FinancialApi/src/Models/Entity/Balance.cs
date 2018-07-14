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

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [JsonProperty(PropertyName = "entradas")]
        internal string _Inputs { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [JsonProperty(PropertyName = "saidas")]
        internal string _Outputs { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [JsonProperty(PropertyName = "encargos")]
        internal string _Charges { get; set; }

        //RelationShips

        //hasMany
        [NotMapped]
        public ICollection<ShortEntryDTO> Inputs
        {
            get { return _Inputs == null ? null : JsonConvert.DeserializeObject<List<ShortEntryDTO>>(_Inputs); }
            set { _Inputs = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public ICollection<ShortEntryDTO> Outputs
        {
            get { return _Outputs == null ? null : JsonConvert.DeserializeObject<List<ShortEntryDTO>>(_Outputs); }
            set { _Outputs = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public ICollection<ShortEntryDTO> Charges
        {
            get { return _Charges == null ? null : JsonConvert.DeserializeObject<List<ShortEntryDTO>>(_Charges); }
            set { _Charges = JsonConvert.SerializeObject(value); }
        }

        //Belongs
        [JsonIgnore]
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [JsonIgnore]
        public long? AccountId { get; set; }

    }
}
