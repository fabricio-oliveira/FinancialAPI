using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace FinancialApi.Models.Entity
{
    [Table("Entries")]
    public class Entry
    {

        public Entry(string Description, string DestinationAccount, string DestinationBank,
                     string TypeAccount, string DestinationIdentity, decimal Value, decimal FinancialCharges,
                     DateTime Date):base(){
            this.UUID = Guid.NewGuid().ToString();
            this.Description = Description;
            this.DestinationAccount = DestinationAccount;
            this.DestinationBank = DestinationBank;
            this.TypeAccount = TypeAccount;
            this.DestinationIdentity = DestinationIdentity;
            this.Value = Value;
            this.FinancialCharges = FinancialCharges;
            this.Date = Date;
        }

        public Entry(){
            this.UUID = Guid.NewGuid().ToString();
        }

        [Key]
        public int ID { get; set; }

        public string UUID { get; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "cant\' be blank")]
        [StringLength(30, ErrorMessage = "Description cannot be longer than 30 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "can\'t be blank")]
        [RegularExpression(@"^\d{5}-\d{1}$", ErrorMessage = "This field accept values format xxxxx-x")]
        public string DestinationAccount { get; set; }

        [Required(ErrorMessage = "can\'t be blank")]
        [RegularExpression(@"^\d{3}-\d{1}$", ErrorMessage = "This field accept values format xxx-x")]
        public string DestinationBank { get; set; }

        [Required(ErrorMessage = "can\'t be blank")]
        [RegularExpression(@"^(corrente|poupança)$", ErrorMessage = "This field accept values \"corrente\" or \"poupança\"")]
        public string TypeAccount { get; set; }

        [Required(ErrorMessage = "can\'t be blank")]
        [RegularExpression(@"^((\d{3}\.\d{3}\.\d{3}-\d{2})|(\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}))$", ErrorMessage = "This field accept CPF or CNPJ number values")]
        public string DestinationIdentity { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "This field accept numeric values $xxxx.xx or xxxx.xx")]
        [Required(ErrorMessage = "can\'t be blank")]
        public decimal Value { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "This field accept numeric values $xxxx.xx or xxxx.xx")]
        [Required(ErrorMessage = "can\'t be blank")]
        [DisplayFormat(DataFormatString = "{0,c}")]
        public decimal FinancialCharges { get; set; }

        [Required(ErrorMessage = "can\'t be blank")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "cant\' be blank")]
        [RegularExpression(@"^(payment|receipt)$", ErrorMessage = "This field accept values \"corrente\" or \"poupança\"")]
        public string Type { get; set; }

    }
}