using System;
using System.IO;
using System.Runtime.Serialization.Json;
using FinancialApi.Models.Entity;

namespace FinancialApi.Utils
{
    public static class EntryTransforExtensions
    {
        public static string ToJson(this Entry obj)
        {
            var _stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(_stream, obj);
            return _stream.ToString();
        }

    }
}