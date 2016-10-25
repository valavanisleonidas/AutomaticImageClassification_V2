using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.FusionTypes
{
    public class EarlyFusion
    {
        public static double[] ConcatArrays(ref double[] array1, ref double[] array2)
        {
            var concat = new double[array1.Length + array2.Length];
            array1.CopyTo(concat, 0);
            array2.CopyTo(concat, array1.Length);
            return concat;
        }

        public static int[] ConcatArrays(ref int[] array1, ref int[] array2)
        {
            var concat = new int[array1.Length + array2.Length];
            array1.CopyTo(concat, 0);
            array2.CopyTo(concat, array2.Length);
            return concat;
        }

        public static float[] ConcatArrays(ref float[] array1, ref float[] array2)
        {
            var concat = new float[array1.Length + array2.Length];
            array1.CopyTo(concat, 0);
            array2.CopyTo(concat, array2.Length);
            return concat;
        }

    }
}
