using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{

    public class HomogeneousKernelMap
    {
        
        public enum KernelType
        {
            Chi2, JensonShannon, Intersection

        }

        public enum WindowType
        {
            Uniform,
            Rectangular
        }

        private WindowType windowType;
        private KernelType kernelType;
        private double period;
        private double gamma;
        private int order;
        private int numSubdivisions;
        private double subdivision;
        private int minExponent;
        private int maxExponent;
        private double[] table;



        public HomogeneousKernelMap()
        {
            Init_HomogeneousKernelMap(KernelType.Chi2, 1, 1, -1, WindowType.Rectangular);
        }

        /**
         * Construct with the given kernel and window. The Gamma and order values are
         * set at their defaults of 1. The period is computed automatically.
         *
         * @param kernelType
         *            the type of kernel
         * @param windowType
         *            the type of window (use {@link WindowType#Rectangular} if unsure)
         */
        public HomogeneousKernelMap(KernelType kernelType, WindowType windowType)
        {
            Init_HomogeneousKernelMap(kernelType, 1, 1, -1, windowType);
        }

        /**
         * Construct with the given kernel, gamma and window. The period is computed
         * automatically and the approximation order is set to 1.
         *
         * @param kernelType
         *            the type of kernel
         * @param gamma
         *            the gamma value. the standard kernels are 1-homogeneous, but
         *            smaller values can work better in practice.
         * @param windowType
         *            the type of window (use {@link WindowType#Rectangular} if unsure)
         */
        public HomogeneousKernelMap(KernelType kernelType,
                                       double gamma,
                                       WindowType windowType)
        {
            Init_HomogeneousKernelMap(kernelType, gamma, 1, -1, windowType);
        }

        /**
         * Construct with the given kernel, gamma, order and window. The period is
         * computed automatically.
         *
         * @param kernelType
         *            the type of kernel
         * @param gamma
         *            the gamma value. the standard kernels are 1-homogeneous, but
         *            smaller values can work better in practice.
         * @param order
         *            the approximation order (usually 1 is enough)
         * @param windowType
         *            the type of window (use {@link WindowType#Rectangular} if unsure)
         */
        public HomogeneousKernelMap(KernelType kernelType,
                                       double gamma,
                                       int order,
                                       WindowType windowType)
        {
            Init_HomogeneousKernelMap(kernelType, gamma, order, -1, windowType);
        }


        /**
         * Construct with the given kernel, gamma, order, period and window. If the
         * period is negative, it will be replaced by the default.
         *
         * @param kernelType
         *            the type of kernel
         * @param gamma
         *            the gamma value. the standard kernels are 1-homogeneous, but
         *            smaller values can work better in practice.
         * @param order
         *            the approximation order (usually 1 is enough)
         * @param period
         *            the periodicity of the kernel spectrum
         * @param windowType
         *            the type of window (use {@link WindowType#Rectangular} if unsure)
         */
        public void Init_HomogeneousKernelMap(KernelType kernelType,
                                       double gamma,
                                       int order,
                                       double period,
                                       WindowType windowType)
        {
            if (gamma <= 0)
            {
                gamma = 0.00001;
                //throw new IllegalArgumentException("Gamma must be > 0");
            }
            int tableWidth, tableHeight;

            this.kernelType = kernelType;
            this.gamma = gamma;
            this.order = order;
            this.windowType = windowType;

            if (period < 0)
            {
                period = computeDefaultPeriod();
            }
            this.period = period;

            this.numSubdivisions = 8 + 8 * order;
            this.subdivision = 1.0 / this.numSubdivisions;
            this.minExponent = -20;
            this.maxExponent = 8;

            tableHeight = 2 * this.order + 1;
            tableWidth = numSubdivisions * (maxExponent - minExponent + 1);
            table = new double[tableHeight * tableWidth + 2 * (1 + this.order)];

            int tableOffset = 0;
            int kappaOffset = tableHeight * tableWidth;
            int freqOffset = kappaOffset + 1 + order;
            double L = 2.0 * Math.PI / this.period;

            /* precompute the sampled periodicized spectrum */
            int j = 0;
            int i = 0;
            while (i <= this.order)
            {
                table[freqOffset + i] = j;
                table[kappaOffset + i] = GetSmoothSpectrum(j * L, this);
                j++;
                if (table[kappaOffset + i] > 0 || j >= 3 * i)
                    i++;
            }

            /* fill table */
            for (int exponent = minExponent; exponent <= maxExponent; exponent++)
            {
                double x, Lxgamma, Llogx, xgamma;
                double sqrt2kappaLxgamma;
                double mantissa = 1.0;

                for (i = 0; i < numSubdivisions; i++, mantissa += subdivision)
                {
                    x = mantissa * Math.Pow(2, exponent);
                    xgamma = Math.Pow(x, this.gamma);
                    Lxgamma = L * xgamma;
                    Llogx = L * Math.Log(x);

                    table[tableOffset++] = Math.Sqrt(Lxgamma * table[kappaOffset]);
                    for (j = 1; j <= this.order; ++j)
                    {
                        sqrt2kappaLxgamma = Math.Sqrt(2.0 * Lxgamma * table[kappaOffset + j]);
                        table[tableOffset++] = sqrt2kappaLxgamma * Math.Cos(table[freqOffset + j] * Llogx);
                        table[tableOffset++] = sqrt2kappaLxgamma * Math.Sin(table[freqOffset + j] * Llogx);
                    }
                }
            }
        }

        private double GetSmoothSpectrum(double omega, HomogeneousKernelMap map)
        {
            double result = 0;
            switch (windowType)
            {
                case WindowType.Uniform:
                    result = GetSpectrum(omega);
                    break;
                case WindowType.Rectangular:
                    double kappa_hat = 0;
                    double epsilon = 1e-2;
                    double omegaRange = 2.0 / (map.period * epsilon);
                    double domega = 2 * omegaRange / (2 * 1024.0 + 1);

                    for (double omegap = -omegaRange; omegap <= omegaRange; omegap += domega)
                    {
                        double win = Sinc((map.period / 2.0) * omegap);
                        win *= (map.period / (2.0 * Math.PI));
                        kappa_hat += win * GetSpectrum(omegap + omega);
                    }

                    kappa_hat *= domega;

                    // project on the positive orthant (see PAMI)
                    kappa_hat = Math.Max(kappa_hat, 0.0);

                    result = kappa_hat;
                    break;
            }

            return result;

        }

        private double Sinc(double x)
        {
            if (x == 0.0)
                return 1.0;
            return Math.Sin(x) / x;
        }

        private double GetSpectrum(double omega)
        {
            double result = 0;
            switch (kernelType)
            {
                case KernelType.Chi2:
                    result = 2.0 / (Math.Exp(Math.PI * omega) + Math.Exp(-Math.PI * omega));
                    break;
                case KernelType.JensonShannon:
                    result = (2.0 / Math.Log(4.0)) * 2.0 / (Math.Exp(Math.PI * omega) + Math.Exp(-Math.PI * omega))
                        / (1 + 4 * omega * omega);
                    break;
                case KernelType.Intersection:
                    result = (2.0 / Math.PI) / (1 + 4 * omega * omega);
                    break;
            }
            return result;
        }

        private double computeDefaultPeriod()
        {
            double period = 0;

            // compute default period
            switch (windowType)
            {
                case WindowType.Uniform:
                    switch (kernelType)
                    {
                        case KernelType.Chi2:
                            period = 5.86 * Math.Sqrt(order + 0) + 3.65;
                            break;
                        case KernelType.JensonShannon:
                            period = 6.64 * Math.Sqrt(order + 0) + 7.24;
                            break;
                        case KernelType.Intersection:
                            period = 2.38 * Math.Log(order + 0.8) + 5.6;
                            break;
                    }
                    break;
                case WindowType.Rectangular:
                    switch (kernelType)
                    {
                        case KernelType.Chi2:
                            period = 8.80 * Math.Sqrt(order + 4.44) - 12.6;
                            break;
                        case KernelType.JensonShannon:
                            period = 9.63 * Math.Sqrt(order + 1.00) - 2.93;
                            break;
                        case KernelType.Intersection:
                            period = 2.00 * Math.Log(order + 0.99) + 3.52;
                            break;
                    }
                    break;
                default:
                    break;
            }
            return Math.Max(period, 1.0);
        }

        /**
         * Evaluate the kernel for the given <code>x</code> value. The output values
         * will be written into the destination array at <code>offset + j*stride</code>
         * intervals where <code>j</code> is between 0 and <code>2 * order + 1</code>.
         *
         * @param destination
         *            the destination array
         * @param stride
         *            the stride
         * @param offset
         *            the offset
         * @param x
         *            the value to compute the kernel approximation for
         */
        public double[] Evaluate(double[] destination, int stride, int offset, double x)
        {
            ExponentAndMantissa em = Frexp(x);

            double mantissa = em.mantissa;
            int exponent = em.exponent;
            double sign = (mantissa >= 0.0) ? +1.0 : -1.0;
            mantissa *= 2 * sign;
            exponent--;

            if (mantissa == 0 || exponent <= minExponent || exponent >= maxExponent)
            {
                for (int j = 0; j <= order; j++)
                {
                    destination[offset + j * stride] = 0.0;
                }
                return destination;
            }

            int featureDimension = 2 * order + 1;
            int v1offset = (exponent - minExponent) * numSubdivisions * featureDimension;

            mantissa -= 1.0;
            while (mantissa >= subdivision)
            {
                mantissa -= subdivision;
                v1offset += featureDimension;
            }

            int v2offset = v1offset + featureDimension;
            for (int j = 0; j < featureDimension; j++)
            {
                double f1 = table[v1offset++];
                double f2 = table[v2offset++];

                destination[offset + j * stride] = sign * ((f2 - f1) * (numSubdivisions * mantissa) + f1);
            }
            return destination;
        }

        /**
         * Compute the Homogeneous Kernel Map approximation of the given feature vector
         *
         * @param in
         *            the feature vector
         * @return the expanded feature vector
         */
        public double[] Evaluate(double[] inArray)
        {
            int step = 2 * this.order + 1;
            double[] outArray = new double[step * inArray.Length];

            for (int i = 0; i < inArray.Length; ++i)
            {
                outArray = Evaluate(outArray, 1, i * step, inArray[i]);
            }

            return outArray;
        }

        public List<double[]> Evaluate(List<double[]> inArray)
        {
            List<double[]> list = new List<double[]>();

            for (int i = 0; i < inArray.Count; i++)
            {
                list.Add(Evaluate(inArray[i]));
            }
            
            return list;
        }


        private ExponentAndMantissa Frexp(double value)
        {
            ExponentAndMantissa ret = new ExponentAndMantissa();
            ret.exponent = 0;
            ret.mantissa = 0.0D;
            if (value != 0.0D && value != -0.0D)
            {
                if (Double.IsNaN(value))
                {
                    ret.mantissa = 0.0D / 0.0;
                    ret.exponent = -1;
                    return ret;
                }
                else if (Double.IsInfinity(value))
                {
                    ret.mantissa = value;
                    ret.exponent = -1;
                    return ret;
                }
                else
                {
                    ret.mantissa = value;
                    ret.exponent = 0;
                    int sign = 1;
                    if (ret.mantissa < 0.0D)
                    {
                        --sign;
                        ret.mantissa = -ret.mantissa;
                    }

                    while (ret.mantissa < 0.5D)
                    {
                        ret.mantissa *= 2.0D;
                        --ret.exponent;
                    }

                    while (ret.mantissa >= 1.0D)
                    {
                        ret.mantissa *= 0.5D;
                        ++ret.exponent;
                    }

                    ret.mantissa *= (double)sign;
                    return ret;
                }
            }
            else
            {
                return ret;
            }
        }


    }

    public class ExponentAndMantissa
    {
        public int exponent;
        public double mantissa;

        public ExponentAndMantissa()
        {
        }
    }

}


