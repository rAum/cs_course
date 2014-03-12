using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex1
{
    class Program
    {
        static void Main(string[] args)
        {
            var data_file = ReadArgs(args);

            if (data_file == null) {
                System.Console.WriteLine("Exiting. No data to process - invalid files. Kitten is dead.");
                return;
            }

            ProcessData(data_file);
        }

        private static void ProcessData(StateGDPCensusData data_file)
        {
            var state_info = data_file.GetStateInfo();
            string csv = state_info.OrderBy(x => x.State).ToCsv();
            File.WriteAllText("output.txt", csv);
        }

        private static StateGDPCensusData ReadArgs(string[] args)
        {
            StateGDPCensusData data = null;

            if (args.Length == 0) {
                string census_data_filepath = "CensusData.txt";  // this could be moved to settings properties
                string state_gdp_filepath   = "StateGDP.txt";
                System.Console.WriteLine("Using default files: [{0}] and [{1}].", census_data_filepath, state_gdp_filepath);
                data = StateGDPCensusData.Construct(state_gdp_filepath, census_data_filepath);
            } else if (args.Length == 2) {
                 data = StateGDPCensusData.Construct(args[0], args[1]);
            } else {
                System.Console.WriteLine("Usage: <application>.exe <path>\\StateGDP.txt <path>\\CensusData.txt \n" +
                                         "You can skip parameters to run with default, example files. Exiting.");
                data = null;
            }
            
            return data;
        }
    }
}
