﻿using System;
using System.Linq;
using AutomaticImageClassification.Classifiers;
using AutomaticImageClassification.Utilities;
using LibSVMsharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class ClassifiersTest
    {
        //pass liblinear
        [TestMethod]
        public void CanUseLibLinear()
        {
            var trainDataPath = @"Data\Features\boc_train.txt";
            var testDataPath = @"Data\Features\boc_test.txt";

            var trainlabelsPath = @"Data\Features\boc_labels_train.txt";
            var testlabelsPath = @"Data\Features\boc_labels_test.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);
            double[] testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);


            var _params = new Parameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 1,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 0,
                IsManualCv = false
            };
            
            //liblinear
            var classifier = new LibLinearLib(_params);

            //normalize 
            Normalization.SqrtList(ref trainFeat);
            Normalization.SqrtList(ref testFeat);

            trainFeat = Normalization.Tfidf(trainFeat);
            testFeat = Normalization.Tfidf(testFeat);

            Normalization.ComputeL1Features(ref trainFeat);
            Normalization.ComputeL1Features(ref testFeat);


            // APPLY KERNEL MAPPING
            //classifier.ApplyKernelMapping(ref trainFeat);
            //classifier.ApplyKernelMapping(ref testFeat);

            //classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);

            classifier.Predict(ref testFeat);

            var predictedLabelsText = @"Data\Results\LibLinearBocPredictedLabels.txt";
            var predictedprobstext = @"Data\Results\LibLinearBocPredictedProbabilities.txt";

            Files.WriteFile(predictedLabelsText, classifier.GetPredictedCategories().ToList());
            Files.WriteFile(predictedprobstext, classifier.GetPredictedProbabilities().ToList());

            //compute accuracy
            int[] predictedLabels = Array.ConvertAll(classifier.GetPredictedCategories(), x => (int) x);
            int[] labels = Array.ConvertAll(testlabels, x => (int)x);
            

            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(labels, predictedLabels);
            Console.WriteLine(@"Accuracy is : "+accuracy);

        }

        //pass libsvm
        [TestMethod]
        public void CanUseLibSvm()
        {

            var trainDataPath = @"Data\Features\boc_train.txt";
            var testDataPath = @"Data\Features\boc_test.txt";

            var trainlabelsPath = @"Data\Features\boc_labels_train.txt";
            var testlabelsPath = @"Data\Features\boc_labels_test.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            var trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);
            var testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);

            var parameter = new SVMParameter
            {
                Type = SVMType.C_SVC,
                Kernel = SVMKernelType.RBF,
                C = 32,
                Gamma = 2,
                Probability = true
            };

            //normalize 
            Normalization.SqrtList(ref trainFeat);
            Normalization.SqrtList(ref testFeat);

            trainFeat = Normalization.Tfidf(trainFeat);
            testFeat = Normalization.Tfidf(testFeat);

            Normalization.ComputeL1Features(ref trainFeat);
            Normalization.ComputeL1Features(ref testFeat);


            IClassifier classifier = new LibSvm(parameter);
            //classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            classifier.Predict(ref testFeat);

            var predictedLabelsText = @"Data\Results\LibSVMBocPredictedLabels.txt";
            var predictedprobstext = @"Data\Results\LibSVMBocPredictedProbabilities.txt";

            Files.WriteFile(predictedLabelsText, classifier.GetPredictedCategories().ToList());
            Files.WriteFile(predictedprobstext, classifier.GetPredictedProbabilities().ToList());

            //compute accuracy
            int[] predictedLabels = Array.ConvertAll(classifier.GetPredictedCategories(), x => (int)x);
            int[] labels = Array.ConvertAll(testlabels, x => (int)x);

            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(labels, predictedLabels);
            Console.WriteLine(@"Accuracy is : " + accuracy);

            // Normalize the datasets if you want: L2 Norm => x / ||x||

            //trainingSet = trainingSet.Normalize(SVMNormType.L2);
            //testSet = testSet.Normalize(SVMNormType.L2);

            //// Evaluate the test results
            //int[,] confusionMatrix;
            //double testAccuracy = testSet.EvaluateClassificationProblem(testResults, model.Labels, out confusionMatrix);

        }

    }
}
