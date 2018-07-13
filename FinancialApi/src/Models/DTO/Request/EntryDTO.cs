using System.ComponentModel.DataAnnotations;
using System;
using Newtonsoft.Json;
using FinancialApi.validate;

namespace FinancialApi.Models.DTO.Request
{
    public abstract class EntryDTO
    {

        public EntryDTO(string Description, string DestinationAccount, string DestinationBank,
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

        public EntryDTO(){
            this.UUID = Guid.NewGuid().ToString();
        }

        public string UUID { get; set; }

        [Required(ErrorMessage = @"can't be blank"),StringLength(30, ErrorMessage = "Description cannot be longer than 30 characters.")]
        [JsonProperty(PropertyName = "descricao")]
        public string Description { get; set; }

        [Required(ErrorMessage = "can\'t be blank"),RegularExpression(@"^\d{5}-\d{1}$",ErrorMessage = "This field accept values format xxxxx-x")]
        [JsonProperty(PropertyName = "conta_destino")]
        public string DestinationAccount { get; set; }

        [Required(ErrorMessage = "can\'t be blank"),RegularExpression(@"^\d{3}-\d{1}$", ErrorMessage = "This field accept values format xxx-x")]
        [JsonProperty(PropertyName = "banco_destino")]
        public string DestinationBank { get; set; }

        [Required(ErrorMessage = "can\'t be blank"),RegularExpression(@"^(corrente|poupança)$", ErrorMessage = @"This field accept values 'corrente' or 'poupança'")]
        [JsonProperty(PropertyName = "tipo_de_conta")]
        public string TypeAccount { get; set; }

        [Required(ErrorMessage = "can\'t be blank"),RegularExpression(@"^((\d{3}\.\d{3}\.\d{3}-\d{2})|(\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}))$", ErrorMessage = "This field accept CPF or CNPJ number values")]
        [JsonProperty(PropertyName = "cpf_cnpj_destino")]
        public string DestinationIdentity { get; set; }

        [Required(ErrorMessage = "can\'t be blank"),RegularExpression(@"^\d+(\.(\d{2}))?$", ErrorMessage = "This field accept numeric values $xxx.xx or xxxx.xx")]
        [JsonProperty(PropertyName = "valor_do_lancamento")]
        public decimal? Value { get; set; }

        [Required(ErrorMessage = "can\'t be blank"),RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "This field accept numeric values $xxxx.xx or xxxx.xx")]
        [JsonProperty(PropertyName = "encargos")]
        public decimal? FinancialCharges { get; set; }

        [Required(ErrorMessage = "can\'t be blank"), RangeDate(ErrorMessage = "this field needs to be a valid date between now and one year ahead.")]
        [JsonProperty(PropertyName = "data_de_lancamento")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "cant\' be blank"),RegularExpression(@"^(pagamento|recebimento)$", ErrorMessage = @"This field accept values 'pagamento' or 'recebimento'")]
        [JsonProperty(PropertyName = "tipo_da_lancamento")]
        public string Type { get; set; }

    }
}