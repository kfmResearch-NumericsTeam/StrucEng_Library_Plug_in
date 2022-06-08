using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StrucEngLib.Utils
{
    /// <summary>Utils</summary>
    public class StringUtils
    {
        public static string ListToPyStr<T>(List<T> data, Func<T, string> toStr)
        {
            StringBuilder b = new StringBuilder();
            b.Append("[ ");
            int n = 0;
            foreach (var l in data)
            {
                if (n > 0)
                {
                    b.Append(", ");
                }

                b.Append($"'{toStr(l)}'");
                n++;
            }

            b.Append(" ] ");
            return b.ToString();
        }

        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
}