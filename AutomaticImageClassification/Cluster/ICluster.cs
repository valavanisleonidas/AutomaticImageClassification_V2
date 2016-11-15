using System.Collections.Generic;
using AutomaticImageClassification.Cluster.ClusterModels;


namespace AutomaticImageClassification.Cluster
{
    public interface ICluster
    {
        ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum);
    }

    public enum ClusterMethod
    {
        VlFeatEm,
        VlFeatGmm,
        VlFeatKmeans,
        AccordKmeans,
        LireKmeans
    }

}
