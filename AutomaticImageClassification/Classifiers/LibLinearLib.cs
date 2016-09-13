using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using ikvm.extensions;
using MathWorks.MATLAB.NET.Arrays;
using MatlabAPI;

namespace AutomaticImageClassification.Classifiers
{
    public class LibLinearLib : IClassifier
    {

        private Model _model = new Model();
        private Results _results = new Results();
        private Parameters _params = new Parameters();
        private LibLinear _classifier = new LibLinear();

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
                MWArray[] result = _classifier.applyKernelMapping(1,
                    new MWNumericArray(features.ToArray()),
                    _params.Kernel,
                    _params.Homker,
                    _params.Gamma);

                var mappedFeatures = (double[,])((MWNumericArray)result[0]).ToArray();
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
                MWArray[] result = _classifier.train_liblinear(2,
                        new MWNumericArray(features.ToArray()),
                        new MWNumericArray(labels),
                        _params.Cost,
                        _params.BiasMultiplier,
                        _params.Solver,
                        _params.SolverType);

                var weights = (double[,])((MWNumericArray)result[0]).ToArray();
                _model.Weights = Arrays.ToJaggedArray(ref weights);
                _model.Bias = (double[,])((MWNumericArray)result[1]).ToArray();

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
                string options = "-s " + _params.SolverType + " -B " + _params.BiasMultiplier;
                //only 0 and 2 support automatic cross validation
                if (!_params.IsManualCv && (_params.SolverType == 0 || _params.SolverType == 2))
                {
                    options = options + " -C 10";
                }
                else
                {
                    options = options + " -v 10 -c 10 -q";
                }

                MWArray[] result = _classifier.CrossValidation(2,
                        new MWNumericArray(features.ToArray()),
                        new MWNumericArray(labels),
                        options,
                        new MWLogicalArray(_params.IsManualCv));

                _params.Cost = ((MWNumericArray)result[0]).ToScalarDouble();
                _params.CvAccuracy = ((MWNumericArray)result[1]).ToScalarDouble();


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Predict(ref List<double[]> features)
        {
            try
            {
                MWArray[] result = _classifier.predict_liblinear(3,
                        new MWNumericArray(features.ToArray()),
                        new MWNumericArray(_model.Weights),
                        new MWNumericArray(_model.Bias));
                
                _results.Probabilities = (double[])((MWNumericArray)result[0]).ToArray();
                _results.PredictedLabels = (int[])((MWNumericArray)result[1]).ToArray();
                
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

        public int[] GetPredictedCategories()
        {
            return _results.PredictedLabels;
        }
    }

    public class Model
    {
        public double[][] Weights;
        public double[,] Bias;
    }

    public class Results
    {
        public double[] Probabilities;
        public int[] PredictedLabels;
    }

    public class Parameters : BaseParameters
    {
        public string Kernel, Homker, Solver;
        public bool IsManualCv;
        public double BiasMultiplier;
        public int SolverType;
    }

}
