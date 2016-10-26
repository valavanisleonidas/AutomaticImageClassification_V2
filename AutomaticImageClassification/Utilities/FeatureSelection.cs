using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class FeatureSelection
    {
        public static void InformationGainThreshold(List<double[]> trainFeatures, List<double[]> testFeatures
            , int[] trainLabels, int[] categories, double threshold)
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

            // calculate the size of each category
            int[] categoriesSize = new int[categories.Length];
            // probabilities of categories
            double[] categoriesProbs = new double[categories.Length];
            for (int i = 0; i < categories.Length; i++)
            {
                categoriesSize[i] = trainLabels.Where(a => a == categories[i]).Count();
                categoriesProbs[i] = categoriesSize[i] / (double)trainFeaturesLen;
            }
            // calculate df(w)
            double[] dfw = Normalization.ComputeDf(trainFeatures.ToArray());
            //calculate probabilities of terms
            double[] pw = new double[dfw.Length];
            double[] pnW = new double[dfw.Length];
            for (int i = 0; i < dfw.Length; i++)
            {
                pw[i] = dfw[i] / (double)trainFeaturesLen;
                pnW[i] = 1 - pw[i];
            }

            //for each feature
            for (int i = 0; i < trainFeaturesDimensions; i++)
            {
                double[] dfwcj = new double[categories.Length];
                //for each document
                for (int j = 0; j < trainFeaturesLen; j++)
                {
                    if (trainFeatures[j][i] != 0)
                    {
                        dfwcj[trainLabels[j]] += 1;
                    }



                }

            }



            //for i = 1:M %for each feature

            //     DFWCJ = zeros(1, k);
            //     for j = 1:N %for each document
            //         if (X(j, i)~= 0), DFWCJ(labels(j)) = DFWCJ(labels(j)) + 1;
            //            end
            //        end
            //    PCJW = DFWCJ / DFW(i);
            //            PCJnW = (SC'-DFWCJ)/(N-DFW(i)+0.00001);


            //    for l = 1:k
            //        if (PCJW(l) == 0), log2PCJW = 0; else log2PCJW = log2(PCJW(l)); end
            //        if (PCJnW(l) == 0), log2PCJnW = 0; else log2PCJnW = log2(PCJnW(l)); end
            //        IG(i) = IG(i) - PCJ(l) * log2(PCJ(l)) + PW(i) * PCJW(l) * log2PCJW + PnW(i) * PCJnW(l) * log2PCJnW;
            //            end

            //        end


        }

        public static void RemoveMostFrequentFeatures(ref List<double[]> trainList, ref List<double[]> testList, int kMostFrequent)
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
            //var sorted = df
            //    .Select((x, i) => new KeyValuePair<double, int>(x, i))
            //    .OrderByDescending(x => x.Key)
            //    .ToList();

            //List<int> indexes = sorted.Select(x => x.Value).ToList();

            //get indexes of sorted items by descending order
            var indexes = df
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderByDescending(x => x.Key).Select(x => x.Value)
                .ToList();


            //transpose features in order to remove Columns ( features ) not Rows ( images )
            var transposedTrain = Arrays.TransposeMatrix(trainList.ToArray()).ToList();
            var transposedTest = Arrays.TransposeMatrix(testList.ToArray()).ToList();

            for (int i = 0; i < kMostFrequent; i++)
            {
                var index = indexes[i];
                transposedTrain.RemoveAt(index);
                transposedTest.RemoveAt(index);
            }
            trainList = Arrays.TransposeMatrix(transposedTrain.ToArray()).ToList();
            transposedTrain.Clear();
            testList = Arrays.TransposeMatrix(transposedTest.ToArray()).ToList();
            transposedTest.Clear();

        }

        public static void RemoveMostFrequentFeaturesUsingThreshold(ref List<double[]> trainList, ref List<double[]> testList, double threshold)
        {

            double[] df = Normalization.ComputeDf(trainList.ToArray());

            //for debugging
            //var sorted = df
            //    .Select((x, i) => new KeyValuePair<double, int>(x, i))
            //    .OrderByDescending(x => x.Key)
            //    .ToList();

            //List<double> B = sorted.Select(x => x.Key).ToList();
            //List<int> indexes = sorted.Select(x => x.Value).ToList();

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

            //get indexes of df by descending order and keep those that are greater than threshold in order to remove them
            var indexes = df
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderByDescending(x => x.Key)
                .Select(x => x.Value)
                .Take(lenFeaturesOverThreshold)
                .OrderByDescending(v => v)
                .ToList();


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
