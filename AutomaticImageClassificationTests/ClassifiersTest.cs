using System;
using System.Diagnostics;
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
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            var trainDataPath = @"Data\Features\lboc__K-Means_50_1024_Lire_AccordKDTree_train.txt";
            var testDataPath = @"Data\Features\lboc__K-Means_50_1024_Lire_AccordKDTree_test.txt";

            var trainlabelsPath = @"Data\Features\boc_labels_train.txt";
            var testlabelsPath = @"Data\Features\boc_labels_test.txt";
            
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);

            const bool doCrossVal = true;

            //normalize
            const bool sqrt = false;
            const bool tfidf = false;
            const bool l1 = true;
            const bool l2 = false;

            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 64,
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
            }
            if (tfidf)
            {
                trainFeat = Normalization.Tfidf(trainFeat);
            }
            if (l1)
            {
                Normalization.ComputeL1Features(ref trainFeat);
            }
            if (l2)
            {
                Normalization.ComputeL2Features(ref trainFeat);
            }

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);

            if (doCrossVal)
            {
                classifier.GridSearch(ref trainFeat, ref trainlabels);
            }

            classifier.Train(ref trainFeat, ref trainlabels);
            trainFeat.Clear();


            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();
            var testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);


            //normalize 
            if (sqrt)
            {
                Normalization.SqrtList(ref testFeat);
            }
            if (tfidf)
            {
                testFeat = Normalization.Tfidf(testFeat);
            }
            if (l1)
            {
                Normalization.ComputeL1Features(ref testFeat);
            }
            if (l2)
            {
                Normalization.ComputeL2Features(ref testFeat);
            }
            classifier.ApplyKernelMapping(ref testFeat);

            classifier.Predict(ref testFeat);


            var predictedLabelsText = @"Data\Results\LibLinearBocPredictedLabels.txt";
            var predictedprobstext = @"Data\Results\LibLinearBocPredictedProbabilities.txt";

            Files.WriteFile(predictedLabelsText, classifier.GetPredictedCategories().ToList());
            Files.WriteFile(predictedprobstext, classifier.GetPredictedProbabilities().ToList());

            //compute accuracy
            int[] predictedLabels = Array.ConvertAll(classifier.GetPredictedCategories(), x => (int)x);
            int[] trueLabels = Array.ConvertAll(testlabels, x => (int)x);


            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(trueLabels, predictedLabels);
            var macrof1 = AutomaticImageClassification.Evaluation.Measures.MacroF1(trueLabels, predictedLabels, trainlabels.Distinct().Select(a => (int)a).ToArray());
            Console.WriteLine(@"Accuracy is : " + accuracy + " , macro f1 : " + macrof1);

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed);

        }

        //pass libsvm
        [TestMethod]
        public void CanUseLibSvm()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            var trainDataPath = @"Data\Features\MkLabSurf_VlFeatEm_RandomInit_512_VlFeatKdTree_Randomized_train.txt";
            var testDataPath = @"Data\Features\MkLabSurf_VlFeatEm_RandomInit_512_VlFeatKdTree_Randomized_test.txt";

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
                Gamma = 4,
                Probability = true
            };

            //normalize 
            //Normalization.SqrtList(ref trainFeat);
            //Normalization.SqrtList(ref testFeat);

            //trainFeat = Normalization.Tfidf(trainFeat);
            //testFeat = Normalization.Tfidf(testFeat);

            Normalization.ComputeL1Features(ref trainFeat);
            Normalization.ComputeL1Features(ref testFeat);


            IClassifier classifier = new LibSvm(parameter);
            classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            classifier.Predict(ref testFeat);

            var predictedLabelsText = @"Data\Results\LibSVMBocPredictedLabels.txt";
            var predictedprobstext = @"Data\Results\LibSVMBocPredictedProbabilities.txt";

            Files.WriteFile(predictedLabelsText, classifier.GetPredictedCategories().ToList());
            Files.WriteFile(predictedprobstext, classifier.GetPredictedProbabilities().ToList());

            //compute accuracy
            int[] predictedLabels = Array.ConvertAll(classifier.GetPredictedCategories(), x => (int)x);
            int[] trueLabels = Array.ConvertAll(testlabels, x => (int)x);

            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(trueLabels, predictedLabels);
            var macrof1 = AutomaticImageClassification.Evaluation.Measures.MacroF1(trueLabels, predictedLabels, trainlabels.Distinct().Select(a => (int)a).ToArray());
            Console.WriteLine(@"Accuracy is : " + accuracy + " , macro f1 : " + macrof1);

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed);

            // Normalize the datasets if you want: L2 Norm => x / ||x||

            //trainingSet = trainingSet.Normalize(SVMNormType.L2);
            //testSet = testSet.Normalize(SVMNormType.L2);

            //// Evaluate the test results
            //int[,] confusionMatrix;
            //double testAccuracy = testSet.EvaluateClassificationProblem(testResults, model.Labels, out confusionMatrix);

        }

    }
}
