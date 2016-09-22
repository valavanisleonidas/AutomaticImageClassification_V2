using System;
using System.Collections.Generic;
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
            string trainDataPath = @"Data\train1.txt";
            string testDataPath = @"Data\test1.txt";

            List<double[]> trainFeat = Files.ReadFileToArray(trainDataPath).ToList();
            double[] trainlabels = { 1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0
                ,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0
                ,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0
                ,1,0,0,0,0,0,0};

            double[] testlabels =
                        {
                1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 1, 0, 1, 0, 1, 2, 1, 0, 2, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1,
                0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1,
                1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 0, 0, 0, 0, 1, 0, 2, 1, 0, 1, 0, 1, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
            };

            List<double[]> testFeat = Files.ReadFileToArray(testDataPath).ToList();


            //List<double[]> train_feat_arr = { { 1, 2, 3 }, { 2, 3, 4 }, { 3, 2, 1 } };
            //double[] trainlabels = { 0, 1, 1 };

            //		System.out.println(train_feat_arr.length +" : "+ train_feat_arr[0].length);
            //		System.out.println(test_feat_arr.length +" : "+ test_feat_arr[0].length);


            Parameters _params = new Parameters();
            _params.Gamma = 0.5;
            _params.Homker = "KCHI2";
            _params.Kernel = "chi2";
            _params.Cost = 1;
            _params.BiasMultiplier = 1;
            _params.Solver = "liblinear"; //liblinear
            _params.SolverType = 0;
            _params.IsManualCv = false;

            LibLinearLib classifier = new LibLinearLib(_params);


            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.ApplyKernelMapping(ref testFeat);

            classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);

            classifier.Predict(ref testFeat);


        }

        //pass libsvm
        [TestMethod]
        public void CanUseLibSvm()
        {

            string trainDataPath = @"Data\train1.txt";
            string testDataPath = @"Data\test1.txt";

            List<double[]> trainFeat = Files.ReadFileToArray(trainDataPath).ToList();
            double[] trainlabels =
            {
                1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1,
                1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1,
                1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 0, 0, 0, 0
            };

            double[] testlabels =
            {
                1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 1, 0, 1, 0, 1, 2, 1, 0, 2, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1,
                0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 1, 1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1,
                1, 1, 0, 0, 2, 1, 0, 2, 1, 0, 1, 0, 2, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
                , 1, 0, 0, 0, 0, 0, 0, 1, 0, 2, 1, 0, 1, 0, 1, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0
            };
            List<double[]> testFeat = Files.ReadFileToArray(testDataPath).ToList();


            IClassifier classifier = new LibSvm();
            classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            classifier.Predict(ref testFeat);


            // Normalize the datasets if you want: L2 Norm => x / ||x||

            //trainingSet = trainingSet.Normalize(SVMNormType.L2);
            //testSet = testSet.Normalize(SVMNormType.L2);

            //// Evaluate the test results
            //int[,] confusionMatrix;
            //double testAccuracy = testSet.EvaluateClassificationProblem(testResults, model.Labels, out confusionMatrix);

        }


    }
}
