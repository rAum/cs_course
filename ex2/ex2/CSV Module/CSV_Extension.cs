using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex2
{
    public static class EnumerableExtensions
    {
        public static string ToCsv<T>(this IEnumerable<T> data)
        {
            return Csv.GenerateWithHeader<T>(data);
        }
    }
}
