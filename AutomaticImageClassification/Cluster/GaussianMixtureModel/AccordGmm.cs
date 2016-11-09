using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Cluster.GaussianMixtureModel
{
    public class AccordGmm : ICluster
    {
        private int _numberOfFeatures;

        public AccordGmm()
        {
            _numberOfFeatures = int.MaxValue;
        }

        public AccordGmm(int numberOfFeatures)
        {
            _numberOfFeatures = numberOfFeatures;
        }

        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            if (descriptorFeatures.Count > _numberOfFeatures)
            {
                Arrays.GetSubsetOfFeatures(ref descriptorFeatures, _numberOfFeatures);
            }
            
            Accord.MachineLearning.GaussianMixtureModel gmm = new Accord.MachineLearning.GaussianMixtureModel(clustersNum);
            GaussianClusterCollection clusters = gmm.Learn(descriptorFeatures.ToArray());
                        
            var covariance = clusters.Model.Covariance;
            return new GmmModel(clusters.Means.ToList(), Arrays.ConvertArrayToList(ref covariance), clusters.Proportions);
        }

        public override string ToString()
        {
            return "AccordGmm";
        }

    }
}
