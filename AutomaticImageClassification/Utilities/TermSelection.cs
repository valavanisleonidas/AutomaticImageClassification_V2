using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class TermSelection
    {
        public static void TermSelectionThreshold(List<double[]> trainFeatures, List<double[]> testFeatures
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

    }
}
