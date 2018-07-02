using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace FinancialApi.Models
{
    [Table("Entries")]
    public class Entry :Base
    {

        public Entry(string Description, string DestinationAccount, string DestinationBank,
                     string TypeAccount, string DestinationIdentity, decimal Value, decimal FinancialCharges,
                     DateTime Date):base(){
            this.ID = ID;
            this.Description = Description;
            this.DestinationAccount = DestinationAccount;
            this.DestinationBank = DestinationBank;
            this.TypeAccount = TypeAccount;
            this.DestinationIdentity = DestinationIdentity;
            this.Value = Value;
            this.FinancialCharges = FinancialCharges;
            this.Date = Date;
        }

        public Entry(){}

        public int ID { get; set; }

        [Required(ErrorMessage = "Description cant\' be blank")]
        [StringLength(30, ErrorMessage = "Description cannot be longer than 30 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Destination account can\'t be blank")]
        public string DestinationAccount { get; set; }

        [Required(ErrorMessage = "Destination bank can\'t be blank")]
        public string DestinationBank { get; set; }

        [Required(ErrorMessage = "Type account can\'t be blank")]
        public string TypeAccount { get; set; }

        [Required(ErrorMessage = "Destination identity can\'t be blank")]
        public string DestinationIdentity { get; set; }

        [Required(ErrorMessage = "Value can\'t be blank")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "Financial charges can\'t be blank")]
        public decimal FinancialCharges { get; set; }

        [Required(ErrorMessage = "Date charges can\'t be blank")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Type cant\' be blank")]
        public string Type { get; set; }

    }
}