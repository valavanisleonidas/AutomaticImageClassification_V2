using System;
using System.Collections.Generic;
using System.Linq;
using ikvm.extensions;

namespace AutomaticImageClassification.Utilities
{
    public class Normalization
    {
        public static void array(ref List<double[]> list)
        {
            var sqrtArray = list.Select(x => x.Select(y => y * y).ToArray()).ToArray();
            double[] columnSums = Arrays.TransposeMatrix(ref sqrtArray).ToList().Select(a => a.Sum()).ToArray();

            list = list
                .Select(a => a.Select((b, i) => b != 0 ? b * 1 / Math.Sqrt(columnSums[i]) : b).ToArray())
                .ToList();
        }

        public static void HellKernelMapping(ref List<double[]> list)
        {
            list = list.Select(w => w.Select((a, index) => Sign(a) * Math.Sqrt(Math.Abs(a))).ToArray()).ToList();
        }

        public static void Sign(ref List<double[]> list)
        {
            list = list.Select(a => Sign(a)).ToList();
        }

        private static double[] Sign(double[] array)
        {
            return array.Select(a => Sign(a)).ToArray();
        }

        private static double Sign(double feature)
        {
            if (feature > 0)
            {
                return 1;
            }
            if (feature < 0)
            {
                return -1;
            }
            //return 0
            return feature;
        }

        //multiply array with given weight
        public static T[] WeightArray<T>(T[] array, T weight)
        {
            var arraySize = array.Length;
            var arr = new T[arraySize];
            for (var i = 0; i < arraySize; i++)
            {
                arr[i] = (dynamic)array[i] * weight;
            }

            return arr;
        }

        public static void SqrtList<T>(ref List<T[]> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var feature = list[i];
                list[i] = SqrtArray(ref feature);
            }
        }

        public static T[] SqrtArray<T>(ref T[] vector)
        {
            for (var i = 0; i < vector.Length; i++)
            {
                dynamic feature = vector[i];
                vector[i] = Math.Sqrt(feature);
            }
            return vector;
        }

        //computes L1 norm of list
        public static void ComputeL1Features<T>(ref List<T[]> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var features = list[i];
                var norm = ComputeL1Norm(ref features);
                list[i] = NormalizeArray(ref features, ref norm);
            }
        }

        //computes L2 norm of list
        public static void ComputeL2Features<T>(ref List<T[]> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var features = list[i];
                var norm = ComputeL2Norm(ref features);
                list[i] = NormalizeArray(ref features, ref norm);
            }
        }

        //computes L1 norm of double array
        public static T ComputeL1Norm<T>(ref T[] imgVocVector)
        {
            //sum of vector ( sum += list[i]; ) 
            return imgVocVector.Aggregate<T, dynamic>(0, (current, feature) => current + feature);
        }

        //computes L2 norm of double array
        public static T ComputeL2Norm<T>(ref T[] imgVocVector)
        {
            //sum += feature * feature; 
            dynamic sum = imgVocVector.Cast<dynamic>().Aggregate<dynamic, dynamic>(0, (current, feature) => current + feature * feature);
            return Math.Sqrt(sum);
        }

        //normalize double array
        public static T[] NormalizeArray<T>(ref T[] imgVocVector, ref T norm)
        {
            for (var i = 0; i < imgVocVector.Length; i++)
            {
                if (Equals(0, norm))
                    continue;

                imgVocVector[i] = (dynamic)imgVocVector[i] / norm;

            }
            return imgVocVector;
        }

        public static T[] ComputeDf<T>(ref T[][] array)
        {
            var columns = array[0].Length;
            var df = new T[columns];
            array = Arrays.TransposeMatrix(ref array);

            for (var i = 0; i < array.Length; i++)
            {
                var feature = array[i];
                df[i] = ComputeDf(ref feature);
            }
            array = Arrays.TransposeMatrix(ref array);

            return df;
        }

        public static T ComputeDf<T>(ref T[] array)
        {
            dynamic df = 0;
            foreach (dynamic feature in array)
            {
                if (feature != 0)
                {
                    df++;
                }
            }
            return df;
        }

        public static T[] ComputeIdf<T>(ref T[] df, ref int numOfDocs)
        {
            var idf = new T[df.Length];

            //1+log(rows./(DF+1));
            for (var i = 0; i < df.Length; i++)
            {
                var feature = df[i];
                idf[i] = ComputeIdf(ref feature, ref numOfDocs);
            }
            return idf;
        }

        public static T ComputeIdf<T>(ref T df, ref int numOfDocs)
        {
            dynamic typing = df;
            return 1 + Math.Log(numOfDocs / (typing + 1));
        }

        public static T ComputeTf<T>(ref T[] array)
        {
            dynamic tf = 0;
            foreach (var feature in array)
            {
                dynamic feat = feature;
                if (feat != 0)
                {
                    tf++;
                }
            }
            return Math.Sqrt(tf);
        }

        /**
         * Calculated the tf of term termToCheck
         * @param totalterms : Array of all the words under processing document
         * @param termToCheck : term of which tf is to be calculated.
         * @return tf(term frequency) of term termToCheck
         */
        public static double ComputeTf(string[] totalterms, string termToCheck)
        {
            var count = totalterms.Count(s => s.equalsIgnoreCase(termToCheck));  //to count the overall occurrence of the term termToCheck
            return Math.Sqrt(count);
            //return Math.sqrt (count / totalterms.Length );
        }

        /**
         * Calculated idf of term termToCheck
         * @param allTerms : all the terms of all the documents
         * @param termToCheck
         * @return idf(inverse document frequency) score
         */
        public static double ComputeIdf(List<string[]> allTerms, string termToCheck)
        {
            var count = allTerms.Count(ss => ss.Any(s => s.equalsIgnoreCase(termToCheck)));
            return 1 + Math.Log(allTerms.Count / (1 + count));
        }

        public static List<T[]> Tfidf<T>(List<T[]> features)
        {
            var featuresArr = features.ToArray();
            return Tfidf(ref featuresArr).ToList();
        }

        public static T[][] Tfidf<T>(ref T[][] features)
        {
            var numOfDocs = features.Length;
            // compute document frequency for features
            var df = ComputeDf(ref features);
            // compute inverse words freq
            var idf = ComputeIdf(ref df, ref numOfDocs);
            // compute tfidf
            features = ComputeTfidf(ref features, ref idf);
            return features;
        }

        public static T[][] ComputeTfidf<T>(ref T[][] array, ref T[] idf)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var tf = ComputeTf(ref array[i]);
                for (var j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = (dynamic)array[i][j] * tf * idf[j];
                }
            }
            return array;
        }


    }
}
