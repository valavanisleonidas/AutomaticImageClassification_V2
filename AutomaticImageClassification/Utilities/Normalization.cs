using System;
using System.Collections.Generic;
using System.Linq;
using MathWorks.MATLAB.NET.Arrays;
using MatlabAPI;

namespace AutomaticImageClassification.Utilities
{
    public class Normalization
    {

        public static double Log(double value, double logBase)
        {
            return value == 0 ? 0 : Math.Log(value, logBase);
        }

        //descriptors = bsxfun(@times, descriptors, 1./sqrt(sum(descriptors.^2) + 0.00001 )) ;
        public static void Normalize(ref List<double[]> list)
        {
            var powArray = list.Select(x => x.Select(y => y * y).ToArray()).ToArray();
            double[] columnSums = Arrays.TransposeMatrix(ref powArray).ToList().Select(a => a.Sum()).ToArray();

            list = list
                .Select(a => a.Select((b, i) => b != 0 ? b * 1 / Math.Sqrt(columnSums[i]) : b).ToArray())
                .ToList();
        }

        //a= 1./ (1 + exp(-sigmoid* a))
        public static void ReNormalize(ref List<double[]> list, double sigmoid)
        {
            list = list.Select(a => a.Select((b, i) => 1 / (1 + Math.Exp(-sigmoid * b))).ToArray()).ToList();
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
            return 0;
        }

        public static void ExtendGeometricalyFeatures(ref List<double[]> descriptors, ref List<double[]> frames, string type, int width, int height)
        {
            try
            {
                var dataExtension = new ExtendFeatures();

                MWArray[] result = dataExtension.ExtendDescriptorsWithGeometry(1,
                    type,
                    new MWNumericArray(frames.ToArray()),
                    new MWNumericArray(descriptors.ToArray()),
                    new MWNumericArray(width),
                    new MWNumericArray(height));

                var mappedFeatures = (double[,])((MWNumericArray)result[0]).ToArray();

                result = null;
                dataExtension.Dispose();
                descriptors = Arrays.ToJaggedArray(ref mappedFeatures).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //multiply array with given weight
        public static double[] WeightArray(double[] array, double weight)
        {
            var arraySize = array.Length;
            var arr = new double[arraySize];
            for (var i = 0; i < arraySize; i++)
            {
                arr[i] = array[i] * weight;
            }
            return arr;
        }

        public static float[] WeightArray(float[] array, double weight)
        {
            var arraySize = array.Length;
            var arr = new float[arraySize];
            for (var i = 0; i < arraySize; i++)
            {
                arr[i] = array[i] * (float)weight;
            }
            return arr;
        }

        public static int[] WeightArray(int[] array, double weight)
        {
            var arraySize = array.Length;
            var arr = new int[arraySize];
            for (var i = 0; i < arraySize; i++)
            {
                arr[i] = array[i] * (int)weight;
            }
            return arr;
        }

        public static void SqrtList(ref List<double[]> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var feature = list[i];
                list[i] = SqrtArray(ref feature);
            }
        }

        public static double[] SqrtArray(ref double[] vector)
        {
            return vector.Select(a => Math.Sqrt(a)).ToArray();
        }

        //computes L1 norm of list
        public static void ComputeL1Features(ref List<double[]> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var features = list[i];
                var norm = ComputeL1Norm(ref features);
                list[i] = NormalizeArray(ref features, ref norm);
            }
        }

        //computes L2 norm of list
        public static void ComputeL2Features(ref List<double[]> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var features = list[i];
                var norm = ComputeL2Norm(ref features);
                list[i] = NormalizeArray(ref features, ref norm);
            }
        }

        //computes L1 norm of double array
        public static double ComputeL1Norm(ref double[] imgVocVector)
        {
            //sum of vector ( sum += list[i]; ) 
            return imgVocVector.Sum();
        }

        //computes L2 norm of double array
        public static double ComputeL2Norm(ref double[] imgVocVector)
        {
            //sum += feature * feature; 
            var sum = imgVocVector.Sum(d => d * d);
            return Math.Sqrt(sum);
        }

        //normalize double array
        public static double[] NormalizeArray(ref double[] imgVocVector, ref double norm)
        {
            if (norm == 0)
                return imgVocVector;

            for (var i = 0; i < imgVocVector.Length; i++)
            {

                imgVocVector[i] = imgVocVector[i] / norm;

            }
            return imgVocVector;
        }

        public static double[] ComputeDf(ref double[][] array)
        {
            var columns = array[0].Length;
            var df = new double[columns];
            array = Arrays.TransposeMatrix(ref array);

            for (var i = 0; i < array.Length; i++)
            {
                df[i] = ComputeDf(ref array[i]);
            }
            array = Arrays.TransposeMatrix(ref array);

            return df;
        }

        public static double[] ComputeDf(double[][] array)
        {
            var columns = array[0].Length;
            var df = new double[columns];
            array = Arrays.TransposeMatrix(ref array);

            for (var i = 0; i < array.Length; i++)
            {
                df[i] = ComputeDf(ref array[i]);
            }
            return df;
        }

        public static int ComputeDf(ref double[] array)
        {
            return array.Count(feature => feature != 0);
        }

        public static double[] ComputeIdf(ref double[] df, ref int numOfDocs)
        {
            var idf = new double[df.Length];

            //1+log(rows./(DF+1));
            for (var i = 0; i < df.Length; i++)
            {
                idf[i] = ComputeIdf(ref df[i], ref numOfDocs);
            }
            return idf;
        }

        public static double ComputeIdf(ref double df, ref int numOfDocs)
        {
            return 1 + Math.Log(numOfDocs / (df + 1));
        }

        public static double ComputeTf(ref double[] array)
        {
            int tf = array.Count(feat => feat != 0);
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
            var count = totalterms.Count(s => s.Equals(termToCheck));  //to count the overall occurrence of the term termToCheck
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
            var count = allTerms.Count(ss => ss.Any(s => string.Equals(s,termToCheck,StringComparison.OrdinalIgnoreCase) ));
            return 1 + Math.Log(allTerms.Count / (1 + count));
        }

        public static List<double[]> Tfidf(List<double[]> features)
        {
            var featuresArr = features.ToArray();
            return Tfidf(ref featuresArr).ToList();
        }

        public static double[][] Tfidf(ref double[][] features)
        {
            var numOfDocs = features.Length;
            // compute document frequency for descriptors
            var df = ComputeDf(ref features);
            // compute inverse words freq
            var idf = ComputeIdf(ref df, ref numOfDocs);
            // compute tfidf
            features = ComputeTfidf(ref features, ref idf);
            return features;
        }

        public static double[][] ComputeTfidf(ref double[][] array, ref double[] idf)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var tf = ComputeTf(ref array[i]);
                for (var j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = array[i][j] * tf * idf[j];
                }
            }
            return array;
        }
    }
}
