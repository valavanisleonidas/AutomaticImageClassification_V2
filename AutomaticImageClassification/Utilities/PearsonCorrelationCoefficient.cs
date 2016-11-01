using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class PearsonCorrelationCoefficient
    {
        public static double Compute(ref int[] x,ref int[] y)
        {
            if (x.Length != y.Length)
            {
                throw new ArgumentException("Wrong length. X has length : "+x.Length+ " elements while Y has : "+y.Length +" elements!");
            }

            double sumX = 0.00, sumY = 0.00,
                   sumXSquare = 0.00, sumYSquare = 0.00,
                   sumXy = 0.00, r = 0.00;

            for (int i = 0; i < x.Length; i++)
            {
                sumX += x[i];
                sumY += y[i];
                sumXSquare += x[i] * x[i];
                sumYSquare += y[i] * y[i];
                sumXy += (x[i] * y[i]);
            }

            double nTimesSumXy = x.Length * sumXy;
            double sumXTimesSumY = sumX * sumY;
            double squareRoot1 = x.Length * sumXSquare - Math.Pow(sumX, 2);
            double squareRoot2 = x.Length * sumYSquare - Math.Pow(sumY, 2);

            r = (nTimesSumXy - sumXTimesSumY) / (Math.Sqrt(squareRoot1) *
                                 Math.Sqrt(squareRoot2));
            return r;
        }

        public static double Compute(ref double[] x, ref double[] y)
        {

            if (x.Length != y.Length)
            {
                throw new ArgumentException("Wrong length. X has length : " + x.Length + " elements while Y has : " + y.Length + " elements!");
            }

            double sumX = 0.00, sumY = 0.00,
                   sumXSquare = 0.00, sumYSquare = 0.00,
                   sumXy = 0.00, r = 0.00;

            for (int i = 0; i < x.Length; i++)
            {
                sumX += x[i];
                sumY += y[i];
                sumXSquare += x[i] * x[i];
                sumYSquare += y[i] * y[i];
                sumXy += (x[i] * y[i]);
            }

            double nTimesSumXy = x.Length * sumXy;
            double sumXTimesSumY = sumX * sumY;
            double squareRoot1 = x.Length * sumXSquare - Math.Pow(sumX, 2);
            double squareRoot2 = x.Length * sumYSquare - Math.Pow(sumY, 2);

            r = (nTimesSumXy - sumXTimesSumY) / (Math.Sqrt(squareRoot1) *
                                 Math.Sqrt(squareRoot2));
            return r;
        }

        public static double Compute(ref float[] x, ref float[] y)
        {

            if (x.Length != y.Length)
            {
                throw new ArgumentException("Wrong length. X has length : " + x.Length + " elements while Y has : " + y.Length + " elements!");
            }

            double sumX = 0.00, sumY = 0.00,
                   sumXSquare = 0.00, sumYSquare = 0.00,
                   sumXy = 0.00, r = 0.00;

            for (int i = 0; i < x.Length; i++)
            {
                sumX += x[i];
                sumY += y[i];
                sumXSquare += x[i] * x[i];
                sumYSquare += y[i] * y[i];
                sumXy += (x[i] * y[i]);
            }

            double nTimesSumXy = x.Length * sumXy;
            double sumXTimesSumY = sumX * sumY;
            double squareRoot1 = x.Length * sumXSquare - Math.Pow(sumX, 2);
            double squareRoot2 = x.Length * sumYSquare - Math.Pow(sumY, 2);

            r = (nTimesSumXy - sumXTimesSumY) / (Math.Sqrt(squareRoot1) *
                                 Math.Sqrt(squareRoot2));
            return r;
        }

    }
}
