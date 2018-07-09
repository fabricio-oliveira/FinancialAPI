using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.src.Models.Entity
{
    [Table("Charges")]
    public class Charge : Entry
    {
        
        public Charge()
        {
            this.Type = "charge";
        }
    }
}
