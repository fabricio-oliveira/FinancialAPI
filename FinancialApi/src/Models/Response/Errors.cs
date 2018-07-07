using System.Collections.Generic;

namespace FinancialApi.Models.Response
{
    public class Errors : Base
    {
        public Dictionary<string,List<string>> Details { get; }

        public Errors()
        {
            this.Details = new Dictionary<string, List<string>>();
        }

        public Errors(Dictionary<string, List<string>> errors)
        {
            this.Details = errors;
        }

        public void Add(string attr, string error)
        {
            if (!this.Details.ContainsKey(attr))
                this.Details[attr] = new List<string>();
                    
            this.Details[attr].Add(error);
        }
    }
}
