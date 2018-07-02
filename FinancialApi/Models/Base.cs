using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace FinancialApi.Models
{
    public class Base
    {
        private MemoryStream _stream = null;
        private DataContractJsonSerializer _ser = null;

        public Base()
        {
            _stream = new MemoryStream();
            _ser = new DataContractJsonSerializer(this.GetType());
        }

        public string ToJson()
        {
            _ser.WriteObject(_stream, this);
            return _stream.ToString();
        }
    }
}
