using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{

    public class ColorsComparator<T> : IEqualityComparer<T[]>
    {
        public bool Equals(T[] x, T[] y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            return x.SequenceEqual(y);
        }

        public int GetHashCode(T[] obj)
        {
            if (obj == null)
                return 0;

            var hash = 17;

            unchecked
            {
                foreach (T s in obj)
                    hash = hash * 23 + ((s == null) ? 0 : s.GetHashCode());
            }

            return hash;
        }
    }
}
