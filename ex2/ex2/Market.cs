using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Market;
using ex2_BoxMuller;
using CS.Numerical;

namespace ex2
{
    class GeometricBrownianMotionMarket : IEvolve, IMarketData
    {
        public GeometricBrownianMotionMarket(Random random, double price,  double drift, double volatility)
        {
            time = DateTime.Today;
            stockPrice = price;
            this.volatility = volatility;
            this.drift = drift;
            
            dv = drift - volatility * volatility * 0.5;

            //bm = new BoxMuller(random);
            this.random = random;
        }

        //BoxMuller bm;
        Random random;

        public void Evolve(TimeSpan offset)
        {
            if (offset <= TimeSpan.Zero) 
                return;

            double tn = offset.TotalDays / 365.0;
            //bm.Next();
            double power = dv * tn + Math.Sqrt(tn) * volatility * NormalDistribution.InverseCDF(random.NextDouble());// bm.Z1;
            stockPrice = stockPrice * Math.Exp(power);
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
