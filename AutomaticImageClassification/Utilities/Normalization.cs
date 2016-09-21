using System;
using System.Collections.Generic;
using System.Linq;
using ikvm.extensions;

namespace AutomaticImageClassification.Utilities
{
    public class Normalization
    {

        public static double[] SqrtArray(ref double[] vector)
        {
            for (var i = 0; i < vector.Length; i++)
            {
                if (vector[i] != 0)
                    vector[i] = Math.Sqrt(vector[i]);
            }
            return vector;
        }

        //computes L1 norm of double array
        public static double ComputeL1Norm(ref double[] imgVocVector)
        {
            double sum = 0;
            for (var i = 0; i < imgVocVector.Length; i++)
            {
                sum += imgVocVector[i];
            }
            return sum;
        }

        //computes L2 norm of double array
        public static double ComputeL2Norm(ref double[] imgVocVector)
        {
            double sum = 0;
            for (var i = 0; i < imgVocVector.Length; i++)
            {
                sum += imgVocVector[i] * imgVocVector[i];
            }
            return Math.Sqrt(sum);
        }

        //normalize double array
        public static double[] NormalizeArray(ref double[] imgVocVector,ref  double norm)
        {
            for (int i = 0; i < imgVocVector.Length; i++)
            {
                if (imgVocVector[i] != 0)
                {
                    imgVocVector[i] = imgVocVector[i]/norm;
                }
            }
            return imgVocVector;
        }

        public static double[] ComputeDf(ref double[][] array)
        {
            int columns = array[0].Length;
            double[] df = new double[columns];
            array = Arrays.TransposeMatrix(ref array);

            for (int i = 0; i < array.Length; i++)
            {
                df[i] = ComputeDf(ref array[i]);
            }
            array = Arrays.TransposeMatrix(ref array);

            return df;
        }

        public static double ComputeDf(ref double[] array)
        {
            double df = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
                {
                    df++;
                }
            }
            return df;
        }

        public static double[] ComputeIdf(ref double[] df, ref int numOfDocs)
        {
            double[] idf = new double[df.Length];

            //1+log(rows./(DF+1));
            for (int i = 0; i < df.Length; i++)
            {
                idf[i] = ComputeIdf(ref df[i],ref  numOfDocs);
            }
            return idf;
        }

        public static double ComputeIdf(ref double df,ref  int numOfDocs)
        {
            return 1 + Math.Log(numOfDocs / (df + 1));
        }

        public static double ComputeTf(ref double[] array)
        {
            double tf = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
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
            double count = 0;  //to count the overall occurrence of the term termToCheck
            foreach (string s in totalterms)
            {
                if (s.equalsIgnoreCase(termToCheck))
                {
                    count++;
                }
            }
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
            double count = 0;
            foreach (string[] ss in allTerms)
            {
                foreach (string s in ss)
                {
                    if (s.equalsIgnoreCase(termToCheck))
                    {
                        count++;
                        break;
                    }
                }
            }
            return 1 + Math.Log(allTerms.Count / (1 + count));
        }

        //wrapper for tfidf for list<double[]>
        public static List<double[]> Tfidf(List<double[]> features)
        {
            var featuresArr= features.ToArray();
            return Tfidf(ref featuresArr).ToList();
        }

        public static double[][] Tfidf(ref double[][] features)
        {
            int numOfDocs = features.Length;
            // compute document frequency for features
            double[] df = ComputeDf(ref features);
            // compute inverse words freq
            double[] idf = ComputeIdf(ref df,ref numOfDocs);
            // compute tfidf
            features = ComputeTfidf(ref features, ref idf);
            return features;
        }

        public static double[][] ComputeTfidf(ref double[][] array, ref double[] idf)
        {
            for (int i = 0; i < array.Length; i++)
            {
                double tf = ComputeTf(ref array[i]);
                for (var j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = array[i][j] * tf * idf[j];
                }
            }
            return array;
        }
        

    }
}
