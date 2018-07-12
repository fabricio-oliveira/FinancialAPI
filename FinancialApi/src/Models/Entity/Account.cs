﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.src.Models.Entity
{
    [Table("Account")]
    public class Account
    {
        public Account() {}

        public Account(string number, string bank, string identity, string type) 
        {
            this.Number = number;
            this.Bank = bank;
            this.TypeAccount = type;
            this.Identity = identity;
        }

        [Key]
        public int ID { get; set; }

        public string Number { get; set; }

        public string Bank { get; set; }

        public string TypeAccount { get; set; }

        public string Identity { get; set; }

        //RelationShip
        public ICollection<CashFlow> CashFlows { get; set; }

    }
}
