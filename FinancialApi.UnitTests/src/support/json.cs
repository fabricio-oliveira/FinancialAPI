using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace FinancialApiUnitTests.src.support
{
    public class Json
    {
        public static string From(Object obj)
        {
            var _stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(_stream, obj);
            return _stream.ToString();
        }
    }
}
