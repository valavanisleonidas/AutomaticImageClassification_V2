using System;
using System.Collections.Generic;
using System.Drawing;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using gr.iti.mklab.visual.aggregation;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class MkLabVlad : IFeatures
    {
        private readonly IFeatures _featureExtractor;
        private ClusterModel _clusterModel;
        /*
                private VladAggregator _vlad;
        */

        public MkLabVlad()
        {
            _featureExtractor = new MkLabSurf();
        }

        public MkLabVlad(IFeatures extractor)
        {
            _featureExtractor = extractor;
        }

        public MkLabVlad(ClusterModel clusterModel)
        {
            _featureExtractor = new MkLabSurf();
            _clusterModel = clusterModel;
            // _vlad = new VladAggregator(codebook.ToArray());
        }

        public MkLabVlad(ClusterModel clusterModel, IFeatures extractor)
        {
            _featureExtractor = extractor;
            _clusterModel = clusterModel;
            //_vlad = new VladAggregator(codebook.ToArray());
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            //return _vlad.aggregate(_featureExtractor.ExtractDescriptors(input).ToArray());
            var codebookDimensions = _clusterModel.Means[0].Length;
            var clusterNum = _clusterModel.Means.Count;

            List<double[]> descriptors = _featureExtractor.ExtractDescriptors(input);

            if (descriptors[0].Length != codebookDimensions)
            {
                throw new ArgumentException("Incorrect dimension size.Features dimensions : " + descriptors[0].Length
                    + ".Clusters dimensions : " + codebookDimensions + ".Please use features of the same dimensions!");
            }

            double[] vlad = new double[clusterNum * codebookDimensions];

            if (descriptors.Count == 0)
            {
                // when there are 0 local descriptors extracted
                return vlad;
            }

            foreach (var descriptor in descriptors)
            {
                int index = _clusterModel.Tree?.SearchTree(descriptor)
                    ?? DistanceMetrics.ComputeNearestCentroidL2NotSquare(ref _clusterModel.Means, descriptor);

                for (int i = 0; i < codebookDimensions; i++)
                {
                    vlad[index * codebookDimensions + i] += descriptor[i] - _clusterModel.Means[index][i];
                }
            }
            return vlad;
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            return _featureExtractor.ExtractDescriptors(input);
        }

        public override string ToString()
        {
            return "Vlad_" + _featureExtractor;
        }




    }
}
