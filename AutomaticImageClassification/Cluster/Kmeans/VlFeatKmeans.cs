﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;
using MatlabAPI;

namespace AutomaticImageClassification.Cluster.Kmeans
{
    public class VlFeatKmeans : ICluster
    {
        private int _numberOfFeatures;

        public VlFeatKmeans()
        {
            _numberOfFeatures = int.MaxValue;
        }

        public VlFeatKmeans(int numberOfFeatures)
        {
            _numberOfFeatures = numberOfFeatures;
        }

        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            try
            {
                var cluster = new MatlabAPI.Cluster();
                if (descriptorFeatures.Count > _numberOfFeatures)
                {
                    //TODO check results because vl_colSubset was removed
                    Arrays.GetSubsetOfFeatures(ref descriptorFeatures, _numberOfFeatures);
                }

                MWArray result = cluster.Kmeans(new MWNumericArray(descriptorFeatures.ToArray()),
                        new MWNumericArray(clustersNum));

                var features = (double[,])result.ToArray();
                result.Dispose();
                cluster.Dispose();

                return new KmeansModel(Arrays.ToJaggedArray(ref features).ToList());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public override string ToString()
        {
            return "VlFeatKmeans";
        }

    }
}
