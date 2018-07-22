using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.Models.Entity
{
    public class Interest
    {

        public Interest(decimal value,
                        DateTime date,
                        Account account) : this()
        {
            this.Value = value;
            this.Date = date;
            this.Account = account;
        }

        public Interest()
        {
            this.UUID = Guid.NewGuid().ToString();
        }

        [Key]
        public long? Id { get; set; }

        public string UUID { get; set; }

        [Column(TypeName = "numeric(10,2)")]
        public decimal? Value { get; set; }

        public DateTime? Date { get; set; }

        public Account Account { get; set; }

        public long? AccountId { get; set; }

        // optimistic lock
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}