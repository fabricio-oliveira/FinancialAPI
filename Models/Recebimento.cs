using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
     [Table("Recebimento")]
     public class Recebimento
     {
         public int ID { get; set; }

         [Required(ErrorMessage="Nome não pode ser branco.")]
         public string Nome { get; set; }
     }
}
