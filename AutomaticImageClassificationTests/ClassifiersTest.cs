using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void CanUseLibLinear()
        {
            string trainDataPath = @"Data\Features\boc_train.txt";
            string testDataPath = @"Data\Features\boc_test.txt";

            string trainlabelsPath = @"Data\Features\train_labels.txt";
            string testlabelsPath = @"Data\Features\test_labels.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);
            double[] testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);


            Parameters _params = new Parameters
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
            LibLinearLib classifier = new LibLinearLib(_params);


            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.ApplyKernelMapping(ref testFeat);

            classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);

            classifier.Predict(ref testFeat);

            string predictedLabelsText = @"Data\Results\LibLinearBocPredictedLabels.txt";
            string predictedprobstext = @"Data\Results\LibLinearBocPredictedProbabilities.txt";

            Files.WriteFile(predictedLabelsText, classifier.GetPredictedCategories().ToList());
            Files.WriteFile(predictedprobstext, classifier.GetPredictedProbabilities().ToList());

        }

        //pass libsvm
        [TestMethod]
        public void CanUseLibSvm()
        {

            string trainDataPath = @"Data\Features\boc_train.txt";
            string testDataPath = @"Data\Features\boc_test.txt";

            string trainlabelsPath = @"Data\Features\train_labels.txt";
            string testlabelsPath = @"Data\Features\test_labels.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);
            double[] testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);

            IClassifier classifier = new LibSvm();
            classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            classifier.Predict(ref testFeat);

            var predictedLabelsText = @"Data\Results\LibSVMBocPredictedLabels.txt";
            var predictedprobstext = @"Data\Results\LibSVMBocPredictedProbabilities.txt";

            Files.WriteFile(predictedLabelsText, classifier.GetPredictedCategories().ToList());
            Files.WriteFile(predictedprobstext, classifier.GetPredictedProbabilities().ToList());



            // Normalize the datasets if you want: L2 Norm => x / ||x||

            //trainingSet = trainingSet.Normalize(SVMNormType.L2);
            //testSet = testSet.Normalize(SVMNormType.L2);

            //// Evaluate the test results
            //int[,] confusionMatrix;
            //double testAccuracy = testSet.EvaluateClassificationProblem(testResults, model.Labels, out confusionMatrix);

        }


    }
}
