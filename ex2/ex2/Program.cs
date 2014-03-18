using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace ex2
{
    class Program
    {
        class StockEstimate
        {
            public double FinalStockPrice { get; set; }
            public double RealizedVol { get; set; }
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            try
            {
                ProgramOptions options = ProgramOptions.ConstructFrom(args);

                TimeSpan ts = new TimeSpan(365 * options.Years, 0, 0, 0);
                ts = new TimeSpan(ts.Ticks / options.Steps); // ts.TotalDays / 365.0 <=> t / n 

                Random rnd = new Random(options.Seed);

                List<StockEstimate> paths = new List<StockEstimate>();
                for (int i = 0; i < options.Count; ++i)
                {
                    GeometricBrownianMotionMarket market = new GeometricBrownianMotionMarket(rnd, options.Price, options.Drift, options.Vol);

                    List<double> logreturn = new List<double>();
                    List<StockEstimate> prices = new List<StockEstimate>();
                    double lastPrice = market.StockPrice;
                    for (int j = 0; j < options.Steps; ++j)
                    {
                        market.Evolve(ts);
                        prices.Add(new StockEstimate() { FinalStockPrice = market.StockPrice, RealizedVol = j });
                        logreturn.Add( Math.Log(market.StockPrice / lastPrice) );
                        lastPrice = market.StockPrice;
                    }

                    File.WriteAllText("prices.txt", prices.ToCsv());

                    double avg = logreturn.Average();

                    double realizedVolSquared = 365.0 / (ts.TotalDays * (options.Steps - 1)) 
                                 * logreturn.Select(x => (x - avg) * (x - avg)).Sum();

                    paths.Add( new StockEstimate() { FinalStockPrice = market.StockPrice,
                                                     RealizedVol     = Math.Sqrt(realizedVolSquared)
                    });
                }

                File.WriteAllText("output.txt", paths.ToCsv());
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Program exceptionally ended. {0}", ex.Message);
            }
        }
    }
}
