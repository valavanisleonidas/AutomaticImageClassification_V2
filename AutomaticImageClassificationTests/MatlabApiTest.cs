using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Classifiers;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    /// <summary>
    /// Summary description for MatlabApiTest
    /// </summary>
    [TestClass]
    public class MatlabApiTest
    {

        /*correct answer for list a = [1 2 3 ; 100 200 300 ; 0.4 0.6 0.8 
        0.2932    0.3338         0    0.2943    0.4502    0.1851    0.8121    0.6733    0.9866
        0.9272   -0.9215   -0.9461    0.9306   -0.8175   -0.9773         0         0         0
        0.2332    0.1984   -0.3240    0.2178    0.3591   -0.1034    0.5835    0.7394   -0.1632*/
        //same results as matlab (check results file)
        [TestMethod]
        public void CanApplyKernelMapping()
        {
            //works fine too
            var trainDataPath = @"Data\Features\boc_train.txt";
            var features = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();

            //List<double[]> features = new List<double[]>();
            //features.Add(new double[] { 1, 2, 3 });
            //features.Add(new double[] { 100, 200, 300 });
            //features.Add(new double[] { 0.4, 0.6, 0.8 });

            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2"
            };

            //liblinear
            var classifier = new LibLinearLib(_params);
            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref features);
            Files.WriteFile("liblinear_ApplyKernelMap_bocTrain.txt", features);

        }

        //same results as matlab (check results file)
        [TestMethod]
        public void CanTrainLibLinear()
        {
            var trainDataPath = @"Data\Features\boc_train.txt";
            var testDataPath = @"Data\Features\boc_test.txt";
            var trainlabelsPath = @"Data\Features\train_labels.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);

            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 1,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 2,
                IsManualCv = false
            };

            //liblinear
            var classifier = new LibLinearLib(_params);

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);

            //classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            //Files.WriteFile("liblinear_model_bias.txt",classifier._model.Bias.ToList());
            //Files.WriteFile("liblinear_model_weights.txt", classifier._model.Weights.ToList());

        }

        //best cost 64 , best cv 0.37470409198512006 CORRECT -s 2 -B 1 -C 10
        //best cost 512 , best cv 0.37673317551572538 CORRECT -s 0 -B 1 -C 10
        [TestMethod]
        public void CanUseLibLinearGridSearch()
        {
            var trainDataPath = @"Data\Features\boc_train.txt";
            var trainlabelsPath = @"Data\Features\train_labels.txt";
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);

            var _params = new LibLinearParameters
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

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.GridSearch(ref trainFeat, ref trainlabels);
            
        }

        //same results as matlab (check results file)
        //same accuracy
        [TestMethod]
        public void CanTestLibLinear()
        {
            var trainDataPath = @"Data\Features\boc_train.txt";
            var testDataPath = @"Data\Features\boc_test.txt";

            var trainlabelsPath = @"Data\Features\train_labels.txt";
            var testlabelsPath = @"Data\Features\test_labels.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);
            double[] testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);


            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 1,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 2,
                IsManualCv = false
            };

            //liblinear
            var classifier = new LibLinearLib(_params);

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.ApplyKernelMapping(ref testFeat);

            //classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            classifier.Predict(ref testFeat);

            //compute accuracy
            int[] predictedLabels = Array.ConvertAll(classifier.GetPredictedCategories(), x => (int)x);
            int[] labels = Array.ConvertAll(testlabels, x => (int)x);

            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(labels, predictedLabels);

            Assert.AreEqual(accuracy, 0.39310611928737416);

        }



    }
}
