using System.Collections.Generic;


namespace AutomaticImageClassification.Cluster
{
    public interface ICluster
    {
        List<double[]> CreateClusters(List<double[]> descriptorFeatures, int clustersNum);
    }
}
