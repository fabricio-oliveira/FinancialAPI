using System.Collections.Generic;

namespace FinancialApi.Models.DTO
{
    public class ErrorsDTO : IBaseDTO
    {
        public Dictionary<string,List<string>> Details { get; }

        public ErrorsDTO()
        {
            this.Details = new Dictionary<string, List<string>>();
        }

        public ErrorsDTO(Dictionary<string, List<string>> errors)
        {
            this.Details = errors;
        }

        public void Add(string attr, string error)
        {
            if (!this.Details.ContainsKey(attr))
                this.Details[attr] = new List<string>();
                    
            this.Details[attr].Add(error);
        }

        public bool HasErrors() => this.Details.Count > 0;
    }
}
