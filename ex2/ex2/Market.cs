using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Market;
using CS.Numerical;

namespace ex2
{
    class GeometricBrownianMotionMarket : IEvolve, IMarketData
    {
        public GeometricBrownianMotionMarket(Random random, double price,  double drift, double volatility)
        {
            time            = DateTime.Today; // temporary, no info about start date
            stockPrice      = price;
            this.volatility = volatility;
            this.drift      = drift;
            this.random     = random;
        
            dv = drift - volatility * volatility * 0.5;
        }

        //BoxMuller bm;
        Random random;

        public void Evolve(TimeSpan offset)
        {
            if (offset <= TimeSpan.Zero) 
                return;

            double tn    = offset.TotalDays / 365.0;
            double power = dv * tn + Math.Sqrt(tn) * volatility * NormalDistribution.InverseCDF(random.NextDouble());            
            stockPrice   = stockPrice * Math.Exp(power);
            
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
        double drift, volatility, dv;
    }
}
