using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{

    public class ColorsComparator : IEqualityComparer<double[]>
    {
        public bool Equals(double[] x, double[] y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            return x.SequenceEqual(y);
        }

        public int GetHashCode(double[] obj)
        {
            if (obj == null)
                return 0;

            int hash = 17;

            unchecked
            {
                foreach (double s in obj)
                    hash = hash * 23 + ((s == null) ? 0 : s.GetHashCode());
            }

            return hash;
        }
    }
}
