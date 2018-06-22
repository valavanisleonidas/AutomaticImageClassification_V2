using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Evaluation
{
    public class Measures
    {
        public static double Accuracy(int[] trueLabels, int[] predictions)
        {
            var errors = trueLabels.Where((t, i) => t != predictions[i]).Count();
            return (trueLabels.Length - errors) / (double)trueLabels.Length;
        }

        public static double Error(int[] trueLabels, int[] predictions)
        {
            return 1 - Accuracy(trueLabels, predictions);
        }

        public static double Precision(int[] trueLabels, int[] predictions, int category)
        {
            //the number of instances that were classified as category
            var y = predictions.Count(t => t == category);

            if (y == 0)
                return 0;

            // the number of instances that belong in category and were classified in category
            var truePositives = trueLabels.Where((t, i) => category == t && t == predictions[i]).Count();

            return truePositives / (double)y;
        }

        public static double Recall(int[] trueLabels, int[] predictions, int category)
        {

            //the number of instances that belong in category
            var y = trueLabels.Count(t => t == category);

            if (y == 0)
                return 0;

            // the number of instances that belong in category and were classified in category
            var truePositives = trueLabels.Where((t, i) => category == t && t == predictions[i]).Count();

            return truePositives / (double)y;
        }

        public static double MacroF1(int[] trueLabels, int[] predictions, int[] categories)
        {
            var categoriesSize = categories.Length;
            var precisions = new List<double>();
            var recalls = new List<double>();

            foreach (var category in categories)
            {
                precisions.Add(Precision(trueLabels, predictions, category));
                recalls.Add(Recall(trueLabels, predictions, category));
            }
            var precisionsMean = precisions.Sum() / categoriesSize;
            var recallsMean = recalls.Sum() / categoriesSize;

            return (2 * precisionsMean * recallsMean) / (precisionsMean + recallsMean);
        }

        public static double MicroF1(int[] trueLabels, int[] predictions, int[] categories)
        {
            double tp = 0;
            double tpFp = 0;
            double tpFn = 0;

            foreach (var category in categories)
            {
                //true positive
                tp += trueLabels.Where((t, i) => category == t && t == predictions[i]).Count();
                //true positive + false positive
                tpFp += predictions.Count(t => t == category);
                //true positive + false negative
                tpFn += trueLabels.Count(t => t == category);
            }
            var precisionsMin = tp / tpFp;
            var recallsMin = tp / tpFn;

            return (2 * precisionsMin * recallsMin) / (precisionsMin + recallsMin);
        }

        public static double F1(int[] trueLabels, int[] predictions, int category)
        {
            var precision = Precision(trueLabels, predictions, category);
            var recall = Recall(trueLabels, predictions, category);

            if (precision == 0 && recall == 0)
                return 0;

            return (2 * precision * recall) / (precision + recall);
        }

        public static double AvgF1(int[] trueLabels, int[] predictions, int category1, int category2)
        {
            return (
                    F1(trueLabels, predictions, category1)
                    +
                    F1(trueLabels, predictions, category2)
                   ) / 2;
        }

        public static double[][] ConfusionMatrix(int[] trueLabels, int[] predictions, int[] categories, bool normalize = true)
        {
            double[][] confusionMatrix = new double[categories.Length][];

            foreach (var category in categories)
            {
                //get indexes of items belong to category
                var indices = trueLabels.Select((cat, index) => new { cat, index })
                    .Where(x => x.cat == category)
                    .Select(x => x.index).ToList();

                confusionMatrix[category - 1] = new double[categories.Length];
                foreach (var index in indices)
                {
                    confusionMatrix[category - 1][predictions[index] - 1] += 1;
                }
            }
            confusionMatrix = Arrays.TransposeMatrix(ref confusionMatrix);

            if (!normalize) return confusionMatrix;

            for (int i = 0; i < confusionMatrix.Length; i++)
            {
                double sum = confusionMatrix[i].Sum();
                if (sum == 0)
                {
                    continue;
                }
                sum = (1 / sum) * 100;
                confusionMatrix[i] = Normalization.WeightArray(confusionMatrix[i], sum);
            }
            return confusionMatrix;
        }

        public static void PlotConfusionMatrix(ref double[][] confusionMatrix,string resFileName,string titleName,int[] categories,int rotationDegrees = 90)
        {
            try
            {
                var plotConfusionMatrix = new PlotAPI.Plot();

                plotConfusionMatrix.PlotConfusionMatrix(
                    new MWNumericArray(confusionMatrix),
                    new MWCharArray(resFileName),
                    new MWCharArray(titleName),
                    new MWNumericArray(new[] { categories }),
                    new MWNumericArray(rotationDegrees));
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
