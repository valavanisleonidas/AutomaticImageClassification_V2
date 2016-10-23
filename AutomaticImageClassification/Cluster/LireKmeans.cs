using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.util;
using net.semanticmetadata.lire.utils.cv;
using Arrays = AutomaticImageClassification.Utilities.Arrays;

namespace AutomaticImageClassification.Cluster
{
    public class LireKmeans : ICluster
    {
        private int _numberOfFeatures = int.MaxValue;

        public LireKmeans() { }

        public LireKmeans(int numberOfFeatures)
        {
            _numberOfFeatures = numberOfFeatures;
        }

        public List<double[]> CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            if (descriptorFeatures.Count > _numberOfFeatures)
            {
                //TODO check results because vl_colSubset was removed
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
            return Arrays.ConvertArrayListToGenericList<double>(ref javaCenters);      
        }

        public override string ToString()
        {
            return "LireKmeans";
        }

    }
}
