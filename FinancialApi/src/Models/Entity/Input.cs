using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.Models.Entity
{
    [Table("Inputs")]
    public class Input : Entry
    {
        
        public Input()
        {
            this.Type = "input";
        }

        public Input(DateTime date, decimal value, Balance balance):base(date, value, balance)
        {
            this.Type = "input";
        }
    }
}
