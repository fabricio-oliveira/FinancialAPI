using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.Models.Entity
{
    [Table("Accounts")]
    public class Account
    {
        public Account() { }

        public Account(string number, string bank, string identity, string type)
        {
            Number = number;
            Bank = bank;
            Type = type;
            Identity = identity;
        }

        [Key]
        public long? Id { get; set; }

        public string Number { get; set; }

        public string Bank { get; set; }

        public string Type { get; set; }

        public string Identity { get; set; }

        [Timestamp]
        [JsonIgnore]
        public byte[] RowVersion { get; set; }

        //RelationShip
        public ICollection<Balance> Balances { get; set; }

        public ICollection<Interest> Interests { get; set; }

        public override bool Equals(Object obj)
        {

            if (obj == null || GetType() != obj.GetType())
                return false;

            Account a = (Account)obj;

            if (a.Id != null)
                return a.Id == Id;

            return a.Bank == Bank && a.Number == Number && a.Identity == Identity && a.Type == Type;
        }

        public override int GetHashCode()
        {
            return (int)Id.GetValueOrDefault();
        }

    }
}
