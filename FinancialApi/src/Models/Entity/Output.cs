using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.Models.Entity
{
    [Table("Outputs")]
    public class Output : Entry
    {
        
        public Output()
        {
            this.Type = "output";
        }
    }
}
