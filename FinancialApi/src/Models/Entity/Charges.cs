using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.src.Models.Entity
{
    public class Charges : ShortEntry
    {
        
        public Charges()
        {
            this.Type = "charges";
        }
    }
}
