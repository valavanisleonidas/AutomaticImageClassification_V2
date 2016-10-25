using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using net.sf.javaml.filter.normalize;

namespace AutomaticImageClassification.FusionTypes
{
    public class LateFusion
    {
        public static Dictionary<double,int> PerformLateFusion(List<double[]> resultsModel1, List<double[]> resultsModel2,double weight)
        {

            //if we want to add the sigmoid..
            //a = sigmoid;
            //b = sigmoid;
            //model1 = 1./ (1 + exp(-a * result_model1));
            //model2 = 1./ (1 + exp(-b * result_model2));


            Dictionary<double, int> categoriesProbs = new Dictionary<double, int>();

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
                categoriesProbs.Add(maxProbability, category);
            }
            return categoriesProbs;
        }
    }
}
