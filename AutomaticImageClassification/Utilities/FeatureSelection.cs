using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Utilities
{
    public class FeatureSelection
    {

        public static double[] InformationGain(ref List<double[]> trainFeatures, ref int[] trainLabels, int[] categories)
        {
            // Select terms(columns) that are greater than specified threshold
            // based on information gain

            // Input : Feature matrix, N x M.
            // N = number of documents,
            // M = number of terms,
            // labels = denote the label of each document
            //  k = input number of categories
            //  threshold : input threshold

            // Output: The Information Gain of the terms, IG mx1


            int trainFeaturesLen = trainFeatures.Count;
            int trainFeaturesDimensions = trainFeatures[0].Length;
            int sizeCategory = categories.Length;

            // calculate the size of each category
            double[] informationGain = new double[trainFeaturesDimensions];

            // calculate the size of each category
            int[] categoriesSize = new int[sizeCategory];
            // probabilities of categories
            double[] pcj = new double[sizeCategory];
            for (int i = 0; i < sizeCategory; i++)
            {
                categoriesSize[i] = trainLabels.Where(a => a == categories[i]).Count();
                pcj[i] = categoriesSize[i] / (double)trainFeaturesLen;
            }
            // calculate SelectionValues(w)
            double[] dfw = Normalization.ComputeDf(trainFeatures.ToArray());
            //calculate probabilities of terms
            double[] pw = new double[dfw.Length];
            double[] pnW = new double[dfw.Length];
            for (int i = 0; i < dfw.Length; i++)
            {
                pw[i] = dfw[i] / trainFeaturesLen;
                pnW[i] = 1 - pw[i];
            }

            //for each feature
            for (int i = 0; i < trainFeaturesDimensions; i++)
            {
                double[] dfwcj = new double[sizeCategory];
                //for each document
                for (int j = 0; j < trainFeaturesLen; j++)
                {
                    if (trainFeatures[j][i] == 0)
                    {
                        continue;
                    }
                    dfwcj[trainLabels[j] - 1] += 1;
                }

                for (int k = 0; k < sizeCategory; k++)
                {
                    var pcjw = dfwcj[k] / dfw[i];
                    var pcjnW = (categoriesSize[k] - dfwcj[k]) / (trainFeaturesLen - dfw[i] + 0.00001);

                    var log2Pcjw = Normalization.Log(pcjw, 2);
                    var log2PcjnW = Normalization.Log(pcjnW, 2);

                    informationGain[i] = informationGain[i] - pcj[k] * Normalization.Log(pcj[k], 2) +
                                         pw[i] * pcjw * log2Pcjw + pnW[i] * pcjnW * log2PcjnW;
                }
            }
            return informationGain;
        }

        public static void InformationGainThreshold(ref List<double[]> trainFeatures, ref List<double[]> testFeatures, ref int[] trainLabels, double threshold)
        {
            var categories = trainLabels.Distinct().ToArray();
            double[] informationGain = InformationGain(ref trainFeatures, ref trainLabels, categories);

            var trainFeaturesLength = trainFeatures[0].Length;

            //count how many values are greather than threshold
            var lenFeaturesOverThreshold = informationGain.Where(a => a < threshold).Count();


            if (lenFeaturesOverThreshold == 0)
            {
                return;
            }

            if (lenFeaturesOverThreshold >= trainFeaturesLength)
            {
                throw new ArgumentException("Parameter 'kMostFrequent' with value : " + lenFeaturesOverThreshold + " greater or equal to train length : " + trainFeaturesLength);
            }

            //get indexes of SelectionValues by descending order and keep those that are greater than threshold in order to remove them
            var indexes = informationGain
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .Take(lenFeaturesOverThreshold)
                .OrderByDescending(v => v)
                .ToList();

            RemoveIndexes(ref indexes, ref trainFeatures, ref testFeatures);


        }

        public static void RemoveMostFrequentFeaturesUsingThreshold(ref List<double[]> trainList, ref List<double[]> testList, double threshold)
        {
            double[] df = Normalization.ComputeDf(trainList.ToArray());

            var trainDocsLength = trainList.Count;
            var trainFeaturesLength = trainList[0].Length;

            //count how many values are greather than threshold
            var lenFeaturesOverThreshold = df.Where(a => a / trainDocsLength > threshold).Count();


            if (lenFeaturesOverThreshold == 0)
            {
                return;
            }

            if (lenFeaturesOverThreshold >= trainFeaturesLength)
            {
                throw new ArgumentException("Parameter 'kMostFrequent' with value : " + lenFeaturesOverThreshold + " greater or equal to train length : " + trainFeaturesLength);
            }

            //get indexes of SelectionValues by descending order and keep those that are greater than threshold in order to remove them
            var indexes = df
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderByDescending(x => x.Key)
                .Select(x => x.Value)
                .Take(lenFeaturesOverThreshold)
                .OrderByDescending(v => v)
                .ToList();

            RemoveIndexes(ref indexes, ref trainList, ref testList);
        }

        public static void InformationGainKFirst(ref List<double[]> trainFeatures, ref List<double[]> testFeatures, ref int[] trainLabels, int kFirst)
        {
            var trainFeaturesLength = trainFeatures[0].Length;

            if (kFirst >= trainFeaturesLength)
            {
                throw new ArgumentException("Parameter 'kMostFrequent' with value : " + kFirst + " greater or equal to train length : " + trainFeaturesLength);
            }
            if (kFirst <= 0)
            {
                return;
            }
            var categories = trainLabels.Distinct().ToArray();
            double[] informationGain = InformationGain(ref trainFeatures, ref trainLabels, categories);

            //get indexes of sorted items by ascending order
            var indexes = informationGain
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderBy(a => a.Key)
                .Select(x => x.Value)
                .Take(kFirst)
                .OrderByDescending(v => v)
                .ToList();

            RemoveIndexes(ref indexes, ref trainFeatures, ref testFeatures);
        }

        public static void RemoveKMostFrequentFeatures(ref List<double[]> trainList, ref List<double[]> testList, int kMostFrequent)
        {
            var trainFeaturesLength = trainList[0].Length;

            if (kMostFrequent >= trainFeaturesLength)
            {
                throw new ArgumentException("Parameter 'kMostFrequent' with value : " + kMostFrequent + " greater or equal to train length : " + trainFeaturesLength);
            }
            if (kMostFrequent <= 0)
            {
                return;
            }

            double[] df = Normalization.ComputeDf(trainList.ToArray());

            //for debugging
            //var sorted = SelectionValues
            //    .Select((x, i) => new KeyValuePair<double, int>(x, i))
            //    .OrderByDescending(x => x.Key)
            //    .ToList();

            //List<int> indexes = sorted.Select(x => x.Value).ToList();

            //get indexes of sorted items by descending order
            var indexes = df
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderByDescending(x => x.Key)
                .Select(x => x.Value)
                .Take(kMostFrequent)
                .OrderByDescending(v => v)
                .ToList();

            RemoveIndexes(ref indexes, ref trainList, ref testList);

        }


        public static void RemoveIndexes(ref List<int> indexes, ref List<double[]> trainList, ref List<double[]> testList)
        {
            //transpose features in order to remove Columns ( features ) not Rows ( images )
            var transposedTrain = Arrays.TransposeMatrix(trainList.ToArray()).ToList();
            var transposedTest = Arrays.TransposeMatrix(testList.ToArray()).ToList();

            foreach (var index in indexes)
            {
                transposedTrain.RemoveAt(index);
                transposedTest.RemoveAt(index);
            }
            trainList = Arrays.TransposeMatrix(transposedTrain.ToArray()).ToList();
            transposedTrain.Clear();
            testList = Arrays.TransposeMatrix(transposedTest.ToArray()).ToList();
            transposedTest.Clear();
        }
    

    }
}
