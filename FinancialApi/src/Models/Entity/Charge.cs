using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.Models.Entity
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
