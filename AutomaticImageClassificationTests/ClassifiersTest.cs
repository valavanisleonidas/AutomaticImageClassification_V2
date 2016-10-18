using System;
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
            const string trainDataPath = @"Data\Features\AutomaticImageClassification.Feature.MkLabSurf_Lire_JavaML_512_train.txt";
            const string testDataPath = @"Data\Features\AutomaticImageClassification.Feature.MkLabSurf_Lire_JavaML_512_test.txt";

            const string trainlabelsPath = @"Data\Features\boc_labels_train.txt";
            const string testlabelsPath = @"Data\Features\boc_labels_test.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);
            double[] testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);
            const bool doCrossVal = false;
            
            //normalize
            const bool sqrt = false;
            const bool tfidf = false;
            const bool l1 = true;

            var _params = new Parameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 8,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 0,
                IsManualCv = false
            };

            //liblinear
            var classifier = new LibLinearLib(_params);

            //normalize 
            if (sqrt)
            {
                Normalization.SqrtList(ref trainFeat);
                Normalization.SqrtList(ref testFeat);
            }
            if (tfidf)
            {
                trainFeat = Normalization.Tfidf(trainFeat);
                testFeat = Normalization.Tfidf(testFeat);
            }
            if (l1)
            {
                Normalization.ComputeL1Features(ref trainFeat);
                Normalization.ComputeL1Features(ref testFeat);
            }

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.ApplyKernelMapping(ref testFeat);

            if (doCrossVal)
            {
                classifier.GridSearch(ref trainFeat, ref trainlabels);
            }

            classifier.Train(ref trainFeat, ref trainlabels);

            classifier.Predict(ref testFeat);

            var predictedLabelsText = @"Data\Results\LibLinearBocPredictedLabels.txt";
            var predictedprobstext = @"Data\Results\LibLinearBocPredictedProbabilities.txt";

            Files.WriteFile(predictedLabelsText, classifier.GetPredictedCategories().ToList());
            Files.WriteFile(predictedprobstext, classifier.GetPredictedProbabilities().ToList());

            //compute accuracy
            int[] predictedLabels = Array.ConvertAll(classifier.GetPredictedCategories(), x => (int)x);
            int[] labels = Array.ConvertAll(testlabels, x => (int)x);


            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(labels, predictedLabels);
            Console.WriteLine(@"Accuracy is : " + accuracy);

        }

        //pass libsvm
        [TestMethod]
        public void CanUseLibSvm()
        {

            var trainDataPath = @"Data\Features\lboc_50_1024_train.txt";
            var testDataPath = @"Data\Features\lboc_50_1024_test.txt";

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
