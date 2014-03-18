using System;

namespace CS.Numerical
{
    public static class NormalDistribution
    {
        /// <summary>
        /// The probability density function for the standard normal distribution
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double PDF(double x)
        {
            return Math.Exp(-0.5 * x * x) / Math.Sqrt(2 * Math.PI);
        }

        /// <summary>
        /// The cumulative distribution function for the standard normal distribution
        ///     We accept an input x and output the value:	
        ///     Integrate(Normal(y), y=-Infinity...x)
        ///
        ///     where Normal(y) is the PDF for the normal distribution function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double CDF(double x)
        {
            var a = new double[]    { 
                                        0.319381530,
                                        -0.356563782,
                                        1.781477937,
                                        -1.821255978,
                                        1.330274429
                                    };

            double result;

            if (x < -7.0)
                result = PDF(x) / Math.Sqrt(1.0 + x * x);

            else
            {
                if (x > 7.0)
                    result = 1.0 - CDF(-x);
                else
                {
                    var tmp = 1.0 / (1.0 + 0.2316419 * Math.Abs(x));
                    result = 1.0 - PDF(x) * (tmp * (a[0] + tmp * (a[1] + tmp * (a[2] + tmp * (a[3] + tmp * a[4])))));

                    if (x <= 0.0)
                        result = 1.0 - result;
                }
            }
            return result;
        }

        /// <summary>
        /// The inverse cumulative Normal distribution.
        ///     We accept an input x between 0 and 1 and output the value Z such that:	
        ///     x = Integrate(Normal(y), y=-Infinity...Z)
        ///     
        ///     where Normal(y) is the PDF for the normal distribution function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double InverseCDF(double x)
        {
            // This uses the Beasley-Springer/Moro approximation.
            //  taken from "The concepts and practice of Mathematical Finance" by M. Joshi.

            var a = new double[]    {   
                                        2.50662823884, 
                                        -18.61500062529, 
                                        41.39119773534, 
                                        -25.44106049637
                                    };
            var b = new double[]   {  
                                        -8.47351093090, 
                                        23.08336743743, 
                                        -21.06224101826, 
                                        3.13082909833
                                    };
            var c = new double[]   { 
                                        0.3374754822726147,
                                        0.9761690190917186,
                                        0.1607979714918209,
                                        0.0276438810333863,
                                        0.0038405729373609,
                                        0.0003951896511919,
                                        0.0000321767881768,
                                        0.0000002888167364,
                                        0.0000003960315187
                                    };
            var u = x - 0.5;
            double r;
            if (Math.Abs(u) < 0.42) // Beasley-Springer
            {
                double y = u * u;
                r = u * (((a[3] * y + a[2]) * y + a[1]) * y + a[0]) /
                        ((((b[3] * y + b[2]) * y + b[1]) * y + b[0]) * y + 1.0);
            }
            else // Moro
            {
                r = x;
                if (u > 0.0)
                    r = 1.0 - x;

                r = Math.Log(-Math.Log(r));
                r = c[0] + r * (c[1] + r * (c[2] + r * (c[3] + r * (c[4] + r * (c[5] + r * (c[6] + r * (c[7] + r * c[8])))))));
                if (u < 0.0)
                    r = -r;
            }
            return r;
        }
    }
}
