using System;
using System.Collections.Generic;

namespace AutomaticImageClassification.Cluster.ClusterModels
{
    public abstract class ClusterModel
    {
        public enum ClusterModelTypes { Kmeans , Gmm }

        public List<double[]> Means;
        public List<double[]> Covariances;
        public double[] Priors;

        protected ClusterModel(List<double[]> means)
        {
            Means = means;
        }
        protected ClusterModel(List<double[]> means,List<double[]> covariances,double[] priors)
        {
            Means = means;
            Covariances = covariances;
            Priors = priors;
        }
    }
}
