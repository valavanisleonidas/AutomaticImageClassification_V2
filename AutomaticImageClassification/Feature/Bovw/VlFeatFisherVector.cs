using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class VlFeatFisherVector : IFeatures
    {
        private IFeatures _featureExtractor;
        private ClusterModel _model;
        public VlFeatFisherVector()
        {
            _featureExtractor = new MkLabSurf();
        }

        public VlFeatFisherVector(IFeatures extractor)
        {
            _featureExtractor = extractor;
        }

        public VlFeatFisherVector(ClusterModel model)
        {
            _featureExtractor = new MkLabSurf();
            _model = model;
        }

        public VlFeatFisherVector(IFeatures extractor, ClusterModel model)
        {
            _featureExtractor = extractor;
            _model = model;
        }

        public double[] ExtractHistogram(string input)
        {
            try
            {
                var fisher = new MatlabAPI.FisherVector();
                //features of descriptor
                var features = ExtractDescriptors(input);
              
                if (features[0].Length != _model.Means[0].Length)
                {
                    throw new ArgumentException("Incorrect dimension size.Features dimensions : " + features[0].Length 
                        + ".Clusters dimensions : " + _model.Means[0].Length+".Please use features of the same dimensions!");
                }

                MWArray[] result = fisher.GetFisherVector(1,
                    new MWNumericArray(features.ToArray()),
                    new MWNumericArray(_model.Means.ToArray()),
                    new MWNumericArray(_model.Covariances.ToArray()),
                    new MWNumericArray(_model.Priors));

                fisher.Dispose();

                return (double[])((MWNumericArray)result[0]).ToVector(MWArrayComponent.Real);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            return _featureExtractor.ExtractDescriptors(input);
        }

        public override string ToString()
        {
            return "Fisher_" + _featureExtractor;
        }

    }
}
