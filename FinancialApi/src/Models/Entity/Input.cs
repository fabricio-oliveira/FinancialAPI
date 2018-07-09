using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FinancialApi.src.Models.Entity
{
    public class Input : ShortEntry
    {
        
        public Input()
        {
            this.Type = "input";
        }
    }
}
