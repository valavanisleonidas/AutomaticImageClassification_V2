using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Evaluation
{
    public class Measures
    {
        public static double Accuracy(int[] labels, int[] predictions)
        {
            var errors = labels.Where((t, i) => t != predictions[i]).Count();
            return (labels.Length - errors) / (double)labels.Length;
        }

        public static double Error(int[] labels, int[] predictions)
        {
            return 1 - Accuracy(labels, predictions);
        }

        public static double Precision(int[] labels, int[] predictions, int category)
        {
            //the number of instances that were classified as category
            var y = predictions.Count(t => t == category);

            if (y == 0)
                return 0;

            // the number of instances that belong in category and were classified in category
            var truePositives = labels.Where((t, i) => category == t && t == predictions[i]).Count();

            return truePositives / (double)y;
        }

        public static double Recall(int[] labels, int[] predictions, int category)
        {

            //the number of instances that belong in category
            var y = labels.Count(t => t == category);

            if (y == 0)
                return 0;

            // the number of instances that belong in category and were classified in category
            var truePositives = labels.Where((t, i) => category == t && t == predictions[i]).Count();

            return truePositives / (double)y;
        }

        public static double MacroF1(int[] labels, int[] predictions, int[] categories)
        {
            var categoriesSize = categories.Length;
            var precisions = new List<double>();
            var recalls = new List<double>();

            foreach (var category in categories)
            {
                precisions.Add(Precision(labels, predictions, category));
                recalls.Add(Recall(labels, predictions, category));
            }
            var precisionsMean = precisions.Sum() / categoriesSize;
            var recallsMean = recalls.Sum() / categoriesSize;

            return (2 * precisionsMean * recallsMean) / (precisionsMean + recallsMean);
        }

        public static double MicroF1(int[] labels, int[] predictions, int[] categories)
        {
            double tp = 0;
            double tpFp = 0;
            double tpFn = 0;

            foreach (var category in categories)
            {
                //true positive
                tp += labels.Where((t, i) => category == t && t == predictions[i]).Count();
                //true positive + false positive
                tpFp += predictions.Count(t => t == category);
                //true positive + false negative
                tpFn += labels.Count(t => t == category);
            }
            var precisionsMin = tp / tpFp;
            var recallsMin = tp / tpFn;

            return (2 * precisionsMin * recallsMin) / (precisionsMin + recallsMin);
        }

        public static double F1(int[] labels, int[] predictions, int category)
        {
            var precision = Precision(labels, predictions, category);
            var recall = Recall(labels, predictions, category);

            if (precision == 0 && recall == 0)
                return 0;

            return (2 * precision * recall) / (precision + recall);
        }

        public static double AvgF1(int[] labels, int[] predictions, int category1, int category2)
        {
            return (
                    F1(labels, predictions, category1)
                    +
                    F1(labels, predictions, category2)
                   ) / 2;
        }
    }
}
