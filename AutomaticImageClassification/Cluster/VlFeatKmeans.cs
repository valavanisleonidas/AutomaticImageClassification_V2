using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;
using MatlabAPI;

namespace AutomaticImageClassification.Cluster
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

        public List<double[]> CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            try
            {
                var cluster = new MatlabAPI.Cluster();
                if (descriptorFeatures.Count > _numberOfFeatures)
                {
                    //TODO check results because vl_colSubset was removed
                    Arrays.GetSubsetOfFeatures(ref descriptorFeatures, _numberOfFeatures);
                }

                MWArray result = cluster.kmeans(new MWNumericArray(descriptorFeatures.ToArray()),
                        new MWNumericArray(clustersNum));

                var features = (double[,])result.ToArray();
                result.Dispose();
                cluster.Dispose();

                return Arrays.ToJaggedArray(ref features).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
