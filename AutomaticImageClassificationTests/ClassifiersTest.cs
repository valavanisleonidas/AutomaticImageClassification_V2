using System;
using System.Diagnostics;
using System.Linq;
using AutomaticImageClassification.Classifiers;
using AutomaticImageClassification.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class ClassifiersTest
    {
        //pass liblinear
        [TestMethod]
        public void CanUseSVM()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            var trainDataPath = @"Data\Features\PHOG_train.txt";
            var testDataPath = @"Data\Features\PHOG_test.txt";

            //var trainDataPath = @"Data\Features\MkLabSurf_KMeans_512_VlFeatKdTree_Randomized_train.txt";
            //var testDataPath = @"Data\Features\MkLabSurf_KMeans_512_VlFeatKdTree_Randomized_test.txt";


            var trainlabelsPath = @"Data\Features\clef2013_train_labels.txt";
            var testlabelsPath = @"Data\Features\clef2013_test_labels.txt";
            
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);

            const bool doCrossVal = false;

            //normalize
            const bool sqrt = false;
            const bool tfidf = false;
            const bool l1 = false;
            const bool l2 = false;

            var _params = new SvmParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 16,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 0,
                IsManualCv = false,
                applyKernelMap = true
            };

            //liblinear
            var classifier = new AutomaticImageClassification.Classifiers.SVM(_params);

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
                //Files.WriteFile(@"Data\trainL1.txt", trainFeat);
            }
            if (l2)
            {
                Normalization.ComputeL2Features(ref trainFeat);
            }

            // APPLY KERNEL MAPPING
            //classifier.ApplyKernelMapping(ref trainFeat);
            
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
            //classifier.ApplyKernelMapping(ref testFeat);
            
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


      





    }
}
