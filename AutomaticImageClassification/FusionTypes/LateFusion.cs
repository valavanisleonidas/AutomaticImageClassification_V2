using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Evaluation;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.FusionTypes
{
    public class LateFusion
    {
        public static Dictionary<double, int> PerformLateFusion(ref List<double[]> resultsModel1, ref List<double[]> resultsModel2, double weight, double sigmoid)
        {
            Normalization.ReNormalize(ref resultsModel1, sigmoid);
            Normalization.ReNormalize(ref resultsModel2, sigmoid);

            Dictionary<double, int> probsCategories = new Dictionary<double, int>();

            //initialize weights for models
            double weightModel1;
            double weightModel2;
            if (weight == 1 || weight == 0)
            {
                weightModel1 = 1;
                weightModel2 = 1;
            }
            else
            {
                weightModel1 = weight;
                weightModel2 = 1 - weight;
            }
            //multiply each model with corresponding weight
            for (int i = 0; i < resultsModel1.Count; i++)
            {
                resultsModel1[i] = Normalization.WeightArray(resultsModel1[i], weightModel1);
                resultsModel2[i] = Normalization.WeightArray(resultsModel2[i], weightModel2);
            }
            //add values of two arrays and get max element for each list row
            for (int i = 0; i < resultsModel1.Count; i++)
            {
                var lateFusionProbs = resultsModel1[i].Zip(resultsModel2[i], (x, y) => x + y).ToArray();
                // Finding max
                var maxProbability = lateFusionProbs.Max();
                // index max is category but plus 1 because arrays starts from zero
                var category = Array.IndexOf(lateFusionProbs, maxProbability) + 1;
                probsCategories.Add(maxProbability, category);
            }
            return probsCategories;
        }

        public static Dictionary<double, int> PerformLateFusion(ref List<double[]> resultsModel1,
            ref List<double[]> resultsModel2, int[] trueLabels, double[] weights, double[] sigmoids,
            out double bestAccuracy, out double bestWeight, out double bestSigmoid)
        {
            bestAccuracy = -1;
            bestWeight = -1;
            bestSigmoid = -1;
            var probsCategories = new Dictionary<double, int>();

            foreach (var weight in weights)
            {
                foreach (var sigmoid in sigmoids)
                {
                    //perform late fusion and get results
                    probsCategories = PerformLateFusion(ref resultsModel1, ref resultsModel2, weight, sigmoid);
                    //get accuracy
                    var accuracy = Measures.Accuracy(trueLabels, probsCategories.Values.ToArray());
                    if (!(accuracy >= bestAccuracy)) continue;

                    bestAccuracy = accuracy;
                    bestWeight = weight;
                    bestSigmoid = sigmoid;
                }
            }
            return probsCategories;
        }

    }
}
