using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ex1
{
    class CsvWriter
    {
        public static void WriteWithHeader<T>(IEnumerable<T> input, string filename)
        {
            StringBuilder sb = new StringBuilder();

            IList<PropertyInfo> propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                sb.Append(propertyInfo.Name).Append(",");
            }
            sb.Remove(sb.Length - 1, 1).AppendLine();

            foreach (T t in input)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    sb.Append(process(propertyInfo.GetValue(t, null))).Append(",");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            File.WriteAllText(filename, sb.ToString());
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
