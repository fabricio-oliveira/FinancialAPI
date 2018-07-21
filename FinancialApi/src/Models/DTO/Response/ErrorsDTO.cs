using System.Collections.Generic;

namespace FinancialApi.Models.DTO.Response
{
    public class ErrorsDTO : IBaseDTO
    {
        public Dictionary<string, List<string>> Details { get; }

        public ErrorsDTO()
        {
            Details = new Dictionary<string, List<string>>();
        }

        public ErrorsDTO(Dictionary<string, List<string>> errors)
        {
            Details = errors;
        }

        public void Add(string attr, string error)
        {
            if (!Details.ContainsKey(attr))
                Details[attr] = new List<string>();

            Details[attr].Add(error);
        }

        public bool HasErrors() => this.Details.Count > 0;
    }
}
