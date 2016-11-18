using System;
using System.Collections.Generic;
using AutomaticImageClassification.KDTree;

namespace AutomaticImageClassification.Cluster.ClusterModels
{
    public enum ClusterModelTypes { Kmeans, Gmm }

    public abstract class ClusterModel
    {
        public List<double[]> Means;
        public List<double[]> Covariances;
        public double[] Priors;
        public IKdTree Tree;
        public int ClusterNum;

        protected ClusterModel(List<double[]> means)
        {
            ClusterNum = means.Count;
            Means = means;
        }

        protected ClusterModel(List<double[]> means, List<double[]> covariances, double[] priors)
        {
            ClusterNum = means.Count;
            Means = means;
            Covariances = covariances;
            Priors = priors;
        }
    }
}
