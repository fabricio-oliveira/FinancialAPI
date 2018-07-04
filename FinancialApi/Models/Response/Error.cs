using System;
using System.Collections;
using System.Collections.Generic;

namespace FinancialApi.Models.Response
{
    public class Error : Base
    {
        public List<string> Msg { get; }

        public Error(List<string> msg)
        {
            this.Msg = msg;
        }

        public Error()
        {
            this.Msg = new List<string>();
        }

        public void Add(string val)
        {
            this.Msg.Add(val);
        }
    }
}
