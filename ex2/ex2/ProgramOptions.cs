using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ex2
{
    class ProgramOptions
    {
        public int Count { get; set; }
        public int Steps { get; set; }
        public Decimal Price { get; set; }
        public Decimal Drift { get; set; }
        public Decimal Vol { get; set; }
        public int Years { get; set; }
        public UInt32 Seed { get; set; }

        public static ProgramOptions ConstructFrom(string[] args)
        {
            ProgramOptions options = null;

            if (args.Length == 14)
            {
                options = new ProgramOptions();

                IList<PropertyInfo> propertyInfos = typeof(ProgramOptions).GetProperties();
                for (int i = 1; i < args.Length; i += 2)
                {
                    foreach (var p in propertyInfos) {
                        if (args[i - 1] == String.Format("/{0}", p.Name.ToLower()))
                        {
                            p.SetValue(options, Convert.ChangeType(args[i], p.GetType()));
                        }
                    }
                }
            }

            return options;
        }
    }
}
