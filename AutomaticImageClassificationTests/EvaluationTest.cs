using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    /// <summary>
    /// EvaluationTest : Tests for metrics
    /// </summary>
    [TestClass]
    public class EvaluationTest
    {
        [TestMethod]
        public void CanComputeAccuracy()
        {
            const string trainlabelsPath = @"Data\Features\boc_labels_test.txt";
            const string testlabelsPath = @"Data\Features\labelsTest.txt";

            int[] categories = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

            var trueLabels = Files.ReadFileTo1DArray<int>(trainlabelsPath);
            var predictedLabels = Files.ReadFileTo1DArray<int>(testlabelsPath);
            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(trueLabels, predictedLabels);
            var macroF1 = AutomaticImageClassification.Evaluation.Measures.MacroF1(trueLabels, predictedLabels, categories);
            var microF1 = AutomaticImageClassification.Evaluation.Measures.MicroF1(trueLabels, predictedLabels, categories);

            //var labels = new[] { 2, 2, 2, 2, 3 };
            //var predictions = new[] { 1, 1, 2, 2, 3 };
            //var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(labels, predictions);
            //Assert.AreEqual(accuracy, 0.6);
        }

        [TestMethod]
        public void CanComputeError()
        {
            var labels = new[] { 2, 2, 2, 2, 3 };
            var predictions = new[] { 1, 1, 2, 2, 3 };
            var error = AutomaticImageClassification.Evaluation.Measures.Error(labels, predictions);
            Assert.AreEqual(error, 0.4);

        }

        [TestMethod]
        public void CanComputePrecision()
        {
            int[] labels = { 0, 1, 2, 0, 1, 2 };
            int[] predictions = { 0, 2, 1, 0, 0, 1 };
            var precision = AutomaticImageClassification.Evaluation.Measures.Precision(labels, predictions, 0);
            Assert.AreEqual(precision, 0.666666666666667);
        }

        [TestMethod]
        public void CanComputeRecall()
        {
            int[] labels = { 0, 1, 2, 0, 1, 2 };
            int[] predictions = { 0, 2, 1, 0, 0, 1 };
            var recall = AutomaticImageClassification.Evaluation.Measures.Recall(labels, predictions, 0);
        }

        [TestMethod]
        public void CanComputeF1()
        {
            int[] labels = { 0, 1, 2, 0, 1, 2 };
            int[] predictions = { 0, 2, 1, 0, 0, 1 };
            var f1 = AutomaticImageClassification.Evaluation.Measures.F1(labels, predictions, 0);
        }

        [TestMethod]
        public void CanComputeConfusionMatrix()
        {

            int[] labels = { 1, 1, 2, 2, 3, 4 };
            int[] predictions = { 1, 1, 2, 3, 4, 4 };
            int[] cats = new[] { 1, 2, 3, 4 };

            var conf = AutomaticImageClassification.Evaluation.Measures.
                        ConfusionMatrix(labels, predictions, cats);

            //check results
            List<double[]> results = new List<double[]>();
            results.Add(new double[] { 100, 0, 0, 0 });
            results.Add(new double[] { 0, 100, 0, 0 });
            results.Add(new double[] { 0, 100, 0, 0 });
            results.Add(new double[] { 0, 0, 50, 50 });

            for (int i = 0; i < conf.Length; i++)
            {
                CollectionAssert.AreEqual(conf[i], results[i]);
            }
            
        }

    }
}
