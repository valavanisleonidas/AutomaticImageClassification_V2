using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;
using MatlabAPI;

namespace AutomaticImageClassification.Classifiers
{
    public class LibLinearLib : IClassifier
    {

        private Model _model = new Model();
        public ClassifierResults _results = new ClassifierResults();
        private Parameters _params = new Parameters();

        public LibLinearLib()
        {
            //default parameters
            _params.Gamma = 0.5;
            _params.Cost = 1;
            _params.Homker = "KCHI2";
            _params.Kernel = "chi2";
            _params.BiasMultiplier = 1;
            _params.Solver = "liblinear"; //liblinear
            _params.SolverType = 2;
        }

        public LibLinearLib(Parameters _params)
        {
            this._params = _params;
        }

        public void ApplyKernelMapping(ref List<double[]> features)
        {
            try
            {
                var classifier = new LibLinear();

                MWArray[] result = classifier.applyKernelMapping(1,
                    new MWNumericArray(features.ToArray()),
                    _params.Kernel,
                    _params.Homker,
                    _params.Gamma);

                var mappedFeatures = (double[,])((MWNumericArray)result[0]).ToArray();

                result = null;
                classifier.Dispose();
                features = Arrays.ToJaggedArray(ref mappedFeatures).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Train(ref List<double[]> features, ref double[] labels)
        {
            try
            {
                var classifier = new LibLinear();

                MWArray[] result = classifier.train_liblinear(2,
                        new MWNumericArray(features.ToArray()),
                        new MWNumericArray(labels),
                        _params.Cost,
                        _params.BiasMultiplier,
                        _params.Solver,
                        _params.SolverType);

                var weights = (double[,])((MWNumericArray)result[0]).ToArray(MWArrayComponent.Real);
                _model.Weights = Arrays.ToJaggedArray(ref weights);
                _model.Bias = (double[])((MWNumericArray)result[1]).ToVector(MWArrayComponent.Real);

                result = null;
                classifier.Dispose();

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void GridSearch(ref List<double[]> features, ref double[] labels)
        {
            try
            {
                double bestCv = -1;
                double bestCost = -1;

                //only 0 and 2 support automatic cross validation
                if (!_params.IsManualCv && (_params.SolverType == 0 || _params.SolverType == 2))
                {
                    var parameters = "-s " + _params.SolverType + " -B " + _params.BiasMultiplier + " -C 10";

                    //automatic cross validation
                    double[] results = CrossValidation(ref features, ref labels, parameters);
                    bestCost = results[0];
                    bestCv = results[1];
                }
                else if (_params.IsManualCv)
                {
                    //pattern to replace cost value
                    //string pattern = "\\-c\\ ([0-9,\\.]+)\\ -q";
                    //options = options + " -v 10 -c 10 -q";
                    // options = Regex.Replace(options, pattern, m => "-c " + cost + " -q");

                    double[] costs = { 0.0625, 0.125, 0.25, 0.5, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
                    foreach (double cost in costs)
                    {
                        string parameters = "-s " + _params.SolverType + " -B " + _params.BiasMultiplier + " -v 10 -c " + cost + " -q";
                        double[] results = CrossValidation(ref features, ref labels, parameters);
                        double currentCv = results[1];

                        if (bestCv < currentCv)
                        {
                            bestCost = cost;
                            bestCv = currentCv;
                        }
                    }

                }
                else
                {
                    //ERROR
                }
                _params.Cost = bestCost;
                _params.CvAccuracy = bestCv;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public double[] CrossValidation(ref List<double[]> features, ref double[] labels, string options)
        {
            var classifier = new LibLinear();

            MWArray[] result = classifier.CrossValidation(2,
                        new MWNumericArray(features.ToArray()),
                        new MWNumericArray(labels),
                        options,
                        new MWLogicalArray(_params.IsManualCv));

            double bestCost = ((MWNumericArray)result[0]).ToScalarDouble();
            double bestCv = ((MWNumericArray)result[1]).ToScalarDouble();

            result = null;
            classifier.Dispose();

            return new[] { bestCost, bestCv };
        }

        public void Predict(ref List<double[]> features)
        {
            try
            {
                var classifier = new LibLinear();

                MWArray[] result = classifier.predict_liblinear(3,
                        new MWNumericArray(features.ToArray()),
                        new MWNumericArray(_model.Weights),
                        new MWNumericArray(_model.Bias));

                _results.Probabilities = (double[])((MWNumericArray)result[0]).ToVector(MWArrayComponent.Real);
                _results.PredictedLabels = (double[])((MWNumericArray)result[1]).ToVector(MWArrayComponent.Real);

                result = null;
                classifier.Dispose();

            }
            catch (Exception e)
            {
                throw e;
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

        public override string ToString()
        {
            return "LibLinear";
        }

    }

    public class Model
    {
        public double[][] Weights;
        public double[] Bias;
    }
    
    public class Parameters
    {
        public string Kernel, Homker, Solver;
        public bool IsManualCv;
        public double BiasMultiplier, Gamma, Cost, CvAccuracy = 0;
        public int SolverType;

    }

}
