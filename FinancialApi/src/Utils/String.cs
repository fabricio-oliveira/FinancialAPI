using System;
using System.Linq;

namespace FinancialApi.Utils
{
    public static class StringTransforExtensions
    {
        public static string ToUnderScore(this String str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
