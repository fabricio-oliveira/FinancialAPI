using System;
using System.Linq;
using Newtonsoft.Json;

namespace FinancialApi.Utils
{
    public static class StringUtil
    {
        public static string ToUnderScore(this System.String str) => string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();

        public static string ToJson(this Object obj)
        {
            return JsonConvert.SerializeObject(obj,
                                        Formatting.None,
                                        new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore
                                        });
        }

        public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}