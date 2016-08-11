using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class Arrays
    {
        public static List<double[]> ConvertIntArrayToDoubleList(int[][] array)
        {
            return array.ToList().Select<int[], double[]>(i => i.Select(x => (double)x).ToArray()).ToList();
        }


    }
}
