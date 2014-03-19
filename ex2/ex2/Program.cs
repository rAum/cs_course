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
                if (options == null)
                {
                    System.Console.WriteLine("Bad program options.");
                }
                else
                {
                    RunSimulations(options);
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Program exceptionally ended. {0}", ex.Message);
            }
        }

        private static void RunSimulations(ProgramOptions options)
        {
            TimeSpan ts = new TimeSpan(365 * options.Years, 0, 0, 0);
            ts = new TimeSpan(ts.Ticks / options.Steps); // note that ts.TotalDays / 365.0 <=> t / n 

            Random rnd = new Random(options.Seed);

            List<StockEstimate> paths = new List<StockEstimate>();
            for (int i = 0; i < options.Count; ++i)
            {
                PathSimulate(options, ts, rnd, paths);
            }

            File.WriteAllText("output.txt", paths.ToCsv());

            PrintEndInfo(options, paths);
        }

        private static void PrintEndInfo(ProgramOptions options, List<StockEstimate> paths)
        {
            var avg = paths.Select(x => x.RealizedVol).Average();
            var avgPrice = paths.Select(x => x.FinalStockPrice).Average();
            Console.WriteLine("Experimental average vol: {0} and relative error is {1}", avg, Math.Abs(avg - options.Vol) / Math.Abs(options.Vol));
            Console.WriteLine("Average final stock price: {0} which gives {1:P2} return", avgPrice, avgPrice / options.Price);

            Console.ReadKey();
        }

        private static void PathSimulate(ProgramOptions options, TimeSpan ts, Random rnd, List<StockEstimate> paths)
        {
            GeometricBrownianMotionMarket market = new GeometricBrownianMotionMarket(rnd, options.Price, options.Drift, options.Vol);

            List<double> logreturn = new List<double>();
            double lastPrice = market.StockPrice;
            for (int j = 0; j < options.Steps; ++j)
            {
                market.Evolve(ts);
                logreturn.Add(Math.Log(market.StockPrice / lastPrice));
                lastPrice = market.StockPrice;
            }

            double avg = logreturn.Average();

            double realizedVolSquared = 365.0 / (ts.TotalDays * (options.Steps - 1))
                         * logreturn.Select(x => (x - avg) * (x - avg)).Sum();

            paths.Add(new StockEstimate()
            {
                FinalStockPrice = market.StockPrice,
                RealizedVol = Math.Sqrt(realizedVolSquared)
            });
        }
    }
}
