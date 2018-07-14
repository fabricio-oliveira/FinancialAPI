using System.ComponentModel.DataAnnotations;
using System;

namespace FinancialApi.Models.Entity
{
    public class Interest
    {

        public Interest(decimal Value,
                        DateTime Date) : this()
        {
            this.Value = Value;
            this.Date = Date;
        }

        public Interest()
        {
            this.UUID = Guid.NewGuid().ToString();
        }

        [Key]
        public long? Id { get; set; }

        public string UUID { get; set; }

        public decimal? Value { get; set; }

        public DateTime? Date { get; set; }

    }
}