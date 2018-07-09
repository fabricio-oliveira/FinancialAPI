using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.src.Models.Entity
{
    public class Output : ShortEntry
    {
        
        public Output()
        {
            this.Type = "output";
        }
    }
}
