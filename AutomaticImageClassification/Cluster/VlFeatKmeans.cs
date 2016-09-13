using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;
using MatlabAPI;

namespace AutomaticImageClassification.Cluster
{
    public class VlFeatKmeans :ICluster
    {
        private int _numberOfFeatures;
        private Clustering cluster = new Clustering();
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
                if (descriptorFeatures.Count > _numberOfFeatures)
                {
                    //TODO check results because vl_colSubset was removed
                    Arrays.GetSubsetOfFeatures(ref descriptorFeatures, _numberOfFeatures);
                }
                var des = descriptorFeatures.ToArray();
                descriptorFeatures = Arrays.TransposeMatrix(ref des).ToList();

                MWArray result = cluster.kmeans(new MWNumericArray(descriptorFeatures.ToArray()),
                        new MWNumericArray(clustersNum));

                var features = (double[,])result.ToArray();

                double[][] _features =Arrays.ToJaggedArray(ref features);
                return Arrays.TransposeMatrix(ref _features).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
