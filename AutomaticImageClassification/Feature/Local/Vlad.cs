using System;
using System.Collections.Generic;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Local
{
    //from mklab 
    public class Vlad : IFeatures
    {
        private readonly IFeatures _featureExtractor;
        private readonly ClusterModel _clusterModel;
        
        public bool CanCluster
        {
            get { return true; }
        }

        public Vlad()
        {
            _featureExtractor = new Surf();
        }

        public Vlad(IFeatures extractor)
        {
            _featureExtractor = extractor;
        }

        public Vlad(ClusterModel clusterModel)
        {
            _featureExtractor = new Surf();
            _clusterModel = clusterModel;
        }

        public Vlad(ClusterModel clusterModel, IFeatures extractor)
        {
            _featureExtractor = extractor;
            _clusterModel = clusterModel;
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
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

            List<int> indices = _clusterModel.Tree?.SearchTree(descriptors)
                    ?? DistanceMetrics.ComputeNearestCentroidL2NotSquare(ref _clusterModel.Means, descriptors);

            for (int y = 0; y < descriptors.Count; y++)
            {
                for (int i = 0; i < codebookDimensions; i++)
                {
                    vlad[indices[y] * codebookDimensions + i] += descriptors[y][i] - _clusterModel.Means[indices[y]][i];
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
