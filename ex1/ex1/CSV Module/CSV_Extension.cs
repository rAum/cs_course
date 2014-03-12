using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex1
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DumpToCsv<T>(this IEnumerable<T> data, string filename)
        {
            CsvWriter.WriteWithHeader<T>(data, filename);
            return data;
        }
    }
}
