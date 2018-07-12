using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.src.Models.Entity
{
    [Table("Inputs")]
    public class Input : Entry
    {
        
        public Input()
        {
            this.Type = "input";
        }

        public Input(DateTime date, decimal value, CashFlow cashFlow):base(date, value, cashFlow)
        {
            this.Type = "input";
        }
    }
}
