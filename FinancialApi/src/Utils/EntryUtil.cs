using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace FinancialApi.Utils
{
    public static class EntryUtil
    {
        public static string GetJSonFieldName(this Object obj, string name)
        {
            var modelType = obj.GetType();
            var attr = modelType.GetProperties()
                            .Where(p => p.Name.Equals(name))
                            .Select(p => p.GetCustomAttribute<JsonPropertyAttribute>())
                            .Select(jp => jp.PropertyName)
                            .FirstOrDefault();

            return attr ?? name.ToUnderScore();
        }


        public static string GetFieldName(this Object obj, string name)
        {
            var modelType = obj.GetType();
            var attr = modelType.GetProperties()
                            .Where(p => p.Name.Equals(name))
                            .Select(p => p.GetCustomAttribute<JsonPropertyAttribute>())
                            .Select(jp => jp.PropertyName)
                            .FirstOrDefault();

            return attr ?? name.ToUnderScore();
        }
    }
}
