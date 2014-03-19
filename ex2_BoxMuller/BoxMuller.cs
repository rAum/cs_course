using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex2_BoxMuller
{
    class BoxMuller
    {
        Random rnd;
        const double TWO_PI = 2.0 * Math.PI;
        double z1, z2;

        public double Z1 { get { return z1; } }
        public double Z2 { get { return z2; } }

        public BoxMuller()
        {
            rnd = new Random();
            Next();
        }

        public BoxMuller(int seed)
        {
            rnd = new Random(seed);
            Next();
        }

        public void Next()
        {
            double u2 = TWO_PI * rnd.NextDouble();

            z1 = z2 = Math.Sqrt(-2.0 * Math.Log(rnd.NextDouble()));

            z1 *= Math.Cos(u2);
            z2 *= Math.Sin(u2);
        }
    }
}
