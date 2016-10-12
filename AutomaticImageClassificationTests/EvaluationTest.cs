using System;
using System.Text;
using System.Collections.Generic;
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
            var labels = new[] { 2, 2, 2, 2, 3 };
            var predictions = new[] { 1, 1, 2, 2, 3 };
            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(labels, predictions);
            Assert.AreEqual(accuracy, 0.6);
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

    }
}
