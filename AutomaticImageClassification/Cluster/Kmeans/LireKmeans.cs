using System.Collections.Generic;
using AutomaticImageClassification.Cluster.ClusterModels;

namespace AutomaticImageClassification.Cluster.Kmeans
{
    public class LireKmeans : ICluster
    {

        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {

            KMeans k = new KMeans(ref descriptorFeatures, clustersNum);
            descriptorFeatures.Clear();

            List<double[]> finalClusters = k.getMeans();

            return new KmeansModel(finalClusters);
            
        }

        public override string ToString()
        {
            return "LireKmeans";
        }

    }
}
