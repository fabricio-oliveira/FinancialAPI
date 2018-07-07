using System.Collections.Generic;

namespace FinancialApi.Models.Response
{
    public class Errors : Base
    {
        public Dictionary<string,List<string>> errors { get; }

        public Errors()
        {
            this.errors = new Dictionary<string, List<string>>();
        }

        public Errors(Dictionary<string, List<string>> errors)
        {
            this.errors = errors;
        }

        public void Add(string attr, string error)
        {
            if (!this.errors.ContainsKey(attr))
                this.errors[attr] = new List<string>();
                    
            this.errors[attr].Add(error);
        }
    }
}
