using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Market;

namespace ex2
{
    class GeometricBrownianMotionMarket : IEvolve, IMarketData
    {
        public GeometricBrownianMotionMarket(System.Random random, double price,  double drift, double volatility)
        {
            rnd = random;
            time = DateTime.Today;
            stockPrice = price;
            this.volatility = volatility;
            this.drift = drift;

            dv = drift - volatility * volatility * 0.5;
        }

        public void Evolve(TimeSpan offset)
        {
            if (offset <= TimeSpan.Zero) 
                return;

            double tn = offset.TotalDays / 365.0;
            
            stockPrice = stockPrice * Math.Exp(dv * tn + Math.Sqrt(tn)*volatility * rnd.NextDouble());
            time += offset;
        }

        public DateTime Time
        {
            get { return time; }
        }

        public double StockPrice
        {
            get { return stockPrice; }
        }

        DateTime time;
        double stockPrice;
        System.Random rnd;
        double drift, volatility, dv;
    }
}
