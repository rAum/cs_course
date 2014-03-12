using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ex1
{
    class Csv
    {
        public static string GenerateWithHeader<T>(IEnumerable<T> input)
        {
            StringBuilder sb = new StringBuilder();

            IList<PropertyInfo> propertyInfos = typeof(T).GetProperties();

            sb.Append(propertyInfos.First().Name);
            foreach (PropertyInfo propertyInfo in propertyInfos.Skip(1))
            {
                sb.Append(",").Append(propertyInfo.Name);
            }
            sb.AppendLine();

            foreach (T t in input)
            {
                sb.Append(process(propertyInfos.First().GetValue(t, null)));
                foreach (PropertyInfo propertyInfo in propertyInfos.Skip(1))
                {
                    sb.Append(",").Append(process(propertyInfo.GetValue(t, null)));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string process<T>(T x)
        {
            if (x == null) return "";
            if (x is Nullable && ((System.Data.SqlTypes.INullable)x).IsNull) return "";
            string output = x.ToString();
            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';
            return output;
        }
    }
}
