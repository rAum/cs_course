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
                        var tmp = (JArray)JToken.ReadFrom(new JsonTextReader(f_state_gdp));
                        data.state_gdp_data = tmp.Select(x => new StateGDPModel() {
                            State = (string)x["state"],
                            GDP   = Decimal.Parse((string)x["gdp"])
                        });
                    }
    
                    using (StreamReader f_census = File.OpenText(file_census))
                    {
                        var tmp = (JArray)JToken.ReadFrom(new JsonTextReader(f_census));
                        data.census_data = tmp.Select(x => new CensusModel() {
                            State = (string)x["state"],
                            SavingBalance = ParseMoneyAmount((string)x["savingsBalance"])
                        });

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


        private IEnumerable<StateGDPModel> state_gdp_data;
        private IEnumerable<CensusModel>   census_data;


        public IEnumerable<StateInfoModel> GetStateInfo()
        {
            var state_info = from state in state_gdp_data
                             join c in census_data
                             on state.State equals c.State
                             into merged
                             orderby state.GDP
                             select new StateInfoModel()
                             {
                                 State = state.State,
                                 GDP = state.GDP,
                                 AverageSavings = merged.Select(x => x.SavingBalance).DefaultIfEmpty(0).Average(),
                                 CensusEntries = merged.Count()
                             };                          


            // probably much slower.
            //var re = (from state in state_gdp_data
            //           select new StateAverageModel()
            //           {
            //               State = state.State,
            //               StateAverage = census_data.Where(x => x.State == state.State).Select(x => x.SavingBalance).DefaultIfEmpty(0).Average()
            //           });

            return state_info;
        }

        #region Helpers
        
        private static readonly System.Globalization.NumberFormatInfo MyNFI = new System.Globalization.NumberFormatInfo()
        {
            NegativeSign           = "-",
            NumberDecimalSeparator = ".",
            NumberGroupSeparator   = ",",
            CurrencySymbol         = "$"
        };

        private static Decimal ParseMoneyAmount(string input)
        {
            return Decimal.Parse(input, System.Globalization.NumberStyles.Currency, MyNFI);
        }

        #endregion
    }
}
