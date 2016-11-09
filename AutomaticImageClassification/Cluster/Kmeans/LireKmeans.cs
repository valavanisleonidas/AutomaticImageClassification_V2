﻿using System.Collections.Generic;
using AutomaticImageClassification.Cluster.ClusterModels;
using java.util;
using net.semanticmetadata.lire.utils.cv;
using Arrays = AutomaticImageClassification.Utilities.Arrays;

namespace AutomaticImageClassification.Cluster.Kmeans
{
    public class LireKmeans : ICluster
    {
        private int _numberOfFeatures;

        public LireKmeans()
        {
            _numberOfFeatures = int.MaxValue;
        }

        public LireKmeans(int numberOfFeatures)
        {
            _numberOfFeatures = numberOfFeatures;
        }

        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            if (descriptorFeatures.Count > _numberOfFeatures)
            {
                Arrays.GetSubsetOfFeatures(ref descriptorFeatures, _numberOfFeatures);
            }

            //cluster
            KMeans mean = new KMeans(
                Arrays.ConvertGenericListToArrayList(ref descriptorFeatures), 
                clustersNum);

            descriptorFeatures.Clear();

            //get centers 
            List javaCenters = mean.getMeans();
            //convert to generics list
            return new KmeansModel(Arrays.ConvertArrayListToGenericList<double>(ref javaCenters));      
        }

        public override string ToString()
        {
            return "LireKmeans";
        }

    }
}
