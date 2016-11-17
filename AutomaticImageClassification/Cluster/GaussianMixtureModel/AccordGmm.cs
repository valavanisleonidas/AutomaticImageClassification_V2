using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Cluster.GaussianMixtureModel
{
    public class AccordGmm : ICluster
    {
       
        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
           
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
