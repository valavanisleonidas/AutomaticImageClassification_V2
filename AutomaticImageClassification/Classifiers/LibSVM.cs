using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSVMsharp;
using LibSVMsharp.Extensions;
using LibSVMsharp.Helpers;

namespace AutomaticImageClassification.Classifiers
{
    public class LibSvm : IClassifier
    {

        private SVMParameter _parameter = new SVMParameter();
        private SVMModel _model;
        private ClassifierResults _results = new ClassifierResults();
        private string _filePath;
        private const int _nFold = 10;
        private double _cvAccuracy = 0;

        public LibSvm()
        {
            _parameter.Type = SVMType.C_SVC;
            _parameter.Kernel = SVMKernelType.RBF;
            _parameter.C = 1;
            _parameter.Gamma = 1;
            _parameter.Probability = true;
            _filePath = "model.txt";
        }

        public LibSvm(SVMParameter parameter)
        {
            _parameter = parameter;
            _filePath = "model.txt";
        }

        public LibSvm(SVMParameter parameter, string path)
        {
            _parameter = parameter;
            _filePath = path;
        }

        public LibSvm(string path)
        {
            _parameter.Type = SVMType.C_SVC;
            _parameter.Kernel = SVMKernelType.RBF;
            _parameter.C = 1;
            _parameter.Gamma = 1;
            _parameter.Probability = true;
            _filePath = path;
        }

        public void Train(ref List<double[]> features, ref double[] labels)
        {
            // Load the dataset
            var trainSet = SVMProblemHelper.Load(features, labels);
            // Train the model, If your parameter set gives good result on cross validation
            _model = trainSet.Train(_parameter);
            // Save the model
            SVM.SaveModel(_model, _filePath);
        }

        public void GridSearch(ref List<double[]> features, ref double[] labels)
        {
            // Load the dataset
            var trainSet = SVMProblemHelper.Load(features, labels);

            double bestcv = -1;
            double bestCost = -1;
            double bestGamma = -1;

            double[] costing = { 0.1, 0.25, 0.5, 1, 2, 4, 8, 16, 32 };
            double[] Gamma = { 0.1, 0.5, 1, 2, 4, 8, 16, 32 };

            //double[] costing = { 16, 32 };
            //double[] Gamma = { 2, 4, 8, 16 };

            foreach (var cost in costing)
            {
                _parameter.C = cost;
                foreach (var gamma in Gamma)
                {
                    _parameter.Gamma = gamma;

                    // Do cross validation to check this parameter set for the dataset
                    double[] crossValidationResults; // output labels
                    trainSet.CrossValidation(_parameter, _nFold, out crossValidationResults);

                    // Evaluate the cross validation result
                    double crossValidationAccuracy = trainSet.EvaluateClassificationProblem(crossValidationResults);

                    if (!(crossValidationAccuracy >= bestcv)) continue;

                    bestcv = crossValidationAccuracy;
                    bestCost = cost;
                    bestGamma = gamma;
                }
            }
            _parameter.C = bestCost;
            _parameter.Gamma = bestGamma;
            _cvAccuracy = bestcv;

        }

        public void Predict(ref List<double[]> features)
        {
            // Load the datasets
            var testSet = SVMProblemHelper.Load(features);

            if (_parameter.Probability)
            {
                //list of probabilities for each category
                List<double[]> probabilities;
                //predict probabilities
                _results.PredictedLabels = testSet.PredictProbability(_model, out probabilities);
                //get max probability for each image
                _results.Probabilities = probabilities.Select(l => l.Max()).ToArray();
            }
            else
            {
                // Predict the instances in the test set
                _results.PredictedLabels = testSet.Predict(_model);
            }

        }

        public double[] GetPredictedProbabilities()
        {
            return _results.Probabilities;
        }

        public double[] GetPredictedCategories()
        {
            return _results.PredictedLabels;
        }
    }
}
