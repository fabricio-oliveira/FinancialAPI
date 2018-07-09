using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.src.Models.Entity
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
