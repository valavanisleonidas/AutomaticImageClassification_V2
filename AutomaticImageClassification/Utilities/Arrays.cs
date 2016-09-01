using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class Arrays
    {
        public static List<double[]> ConvertIntArrayToDoubleList(ref int[][] array)
        {
            return array.ToList().Select<int[], double[]>(i => i.Select(x => (double)x).ToArray()).ToList();
        }
        
        public static T[][] TransposeMatrix<T>(ref T[][] matrix)
        {
            int m = matrix.Length;
            int n = matrix[0].Length;

            T[][] trasposedMatrix = new T[n][];

            for (var x = 0; x < n; x++)
            {
                trasposedMatrix[x] = new T[m];
                for (var y = 0; y < m; y++)
                {
                    trasposedMatrix[x][y] = matrix[y][x];
                }
            }
            return trasposedMatrix;
        }

        public static T[,] To2D<T>(ref T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (var i = 0; i < FirstDim; ++i)
                    for (var j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

        public static T[][] ToJaggedArray<T>(ref T[,] multiArray)
        {
            int firstElement = multiArray.GetLength(0);
            int secondElement = multiArray.GetLength(1);

            T[][] jaggedArray = new T[firstElement][];

            for (var c = 0; c < firstElement; c++)
            {
                jaggedArray[c] = new T[secondElement];
                for (var r = 0; r < secondElement; r++)
                {
                    jaggedArray[c][r] = multiArray[c, r];
                }
            }
            return jaggedArray;
        }

    }

   


}
