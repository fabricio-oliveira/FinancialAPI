using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Model
{
     [Table("Pagamento")]
     public class Pagamento
     {
         public int ID { get; set; }

         [Required(ErrorMessage="Nome não pode ser branco.")]
         public string Nome { get; set; }
     }
}
