using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ex1
{
    class StateGDPCensusData
    {
        public static StateGDPCensusData Construct(string file_state_gdp, string file_census)
        {
            StateGDPCensusData data = null;

            if (File.Exists(file_state_gdp) && File.Exists(file_census))
            {
                data = new StateGDPCensusData();
                
                try
                {
                    using (StreamReader f_state_gdp = File.OpenText(file_state_gdp))
                    {
                        data.state_gdp = (JArray)JToken.ReadFrom(new JsonTextReader(f_state_gdp));
                    }
    
                    using (StreamReader f_census = File.OpenText(file_census))
                    {
                        data.census = (JArray)JToken.ReadFrom(new JsonTextReader(f_census));
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex);
                    data = null;
                }
            }

            return data;
        }

        private JArray census, state_gdp;

        public JArray Census { get { return census; } }
        public JArray StateGDP { get { return state_gdp; } }
    }
}
