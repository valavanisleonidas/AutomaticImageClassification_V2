using System;
using System.Collections.Generic;
using System.Linq;


namespace AutomaticImageClassification.Utilities
{
    public class Arrays
    {

        public static int[][] ConvertDoubleListToIntArray(ref List<double[]> list)
        {
            return list.ConvertAll(
                            des => Array.ConvertAll(des, x => (int)x ) ).ToArray();
        }

        public static List<T[]> ConvertArrayToList<T>(ref T[,] array)
        {
            return ToJaggedArray(ref array).ToList().Select(floatset => floatset.ToList().Select(i => i).ToArray()).ToList();
        }
        
        public static void GetDistinctObjects<T>(ref List<T[]> objects)
        {
            if (objects.Count == 0)
            {
                throw new ArgumentException("List is empty");
            }
            objects = objects.Distinct(new CustomGenericArrayEqualityComparator<T>()).ToList();
        }
        
        public static T[][] TransposeMatrix<T>(ref T[][] matrix)
        {
            var m = matrix.Length;
            var n = matrix[0].Length;

            var trasposedMatrix = new T[n][];

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

        public static T[][] TransposeMatrix<T>(T[][] matrix)
        {
            var m = matrix.Length;
            var n = matrix[0].Length;

            var trasposedMatrix = new T[n][];

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
                int firstDim = source.Length;
                int secondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[firstDim, secondDim];
                for (var i = 0; i < firstDim; ++i)
                    for (var j = 0; j < secondDim; ++j)
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

        public static T[][] ToJaggedArray<T>(T[,] multiArray)
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
        
        public static void GetSubsetOfFeatures<T>(ref List<T[]> descriptorFeatures, int numberOfFeatures)
        {
            descriptorFeatures = descriptorFeatures.OrderBy(x => Guid.NewGuid()).Take(numberOfFeatures).ToList();
        }

    }
    
}
