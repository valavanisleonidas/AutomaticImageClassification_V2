using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.FusionTypes
{
    public class EarlyFusion
    {

        public static List<double[]> ConcatArrays(ref List<double[]> list1, ref List<double[]> list2)
        {
            List<double[]> concatList = new List<double[]>();
            for (int i = 0; i < list1.Count; i++)
            {
                concatList.Add(ConcatArrays(list1[i], list2[i]));
            }

            return concatList;
        }

        public static List<int[]> ConcatArrays(ref List<int[]> list1, ref List<int[]> list2)
        {
            List<int[]> concatList = new List<int[]>();
            for (int i = 0; i < list1.Count; i++)
            {
                concatList.Add(ConcatArrays(list1[i], list2[i]));
            }

            return concatList;
        }

        public static List<float[]> ConcatArrays(ref List<float[]> list1, ref List<float[]> list2)
        {
            List<float[]> concatList = new List<float[]>();
            for (int i = 0; i < list1.Count; i++)
            {
                concatList.Add(ConcatArrays(list1[i], list2[i]));
            }

            return concatList;
        }

        public static double[] ConcatArrays(double[] array1, double[] array2)
        {
            var concat = new double[array1.Length + array2.Length];
            array1.CopyTo(concat, 0);
            array2.CopyTo(concat, array1.Length);
            return concat;
        }

        public static int[] ConcatArrays(int[] array1, int[] array2)
        {
            var concat = new int[array1.Length + array2.Length];
            array1.CopyTo(concat, 0);
            array2.CopyTo(concat, array2.Length);
            return concat;
        }

        public static float[] ConcatArrays(float[] array1, float[] array2)
        {
            var concat = new float[array1.Length + array2.Length];
            array1.CopyTo(concat, 0);
            array2.CopyTo(concat, array2.Length);
            return concat;
        }

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
