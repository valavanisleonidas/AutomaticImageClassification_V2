using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Cluster.GaussianMixtureModel
{
    //from vlfeat
    public class GMM : ICluster
    {
      
        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            try
            {
                var cluster = new MatlabAPI.Cluster();
              
                MWArray[] result = cluster.Gmm(3,
                    new MWNumericArray(descriptorFeatures.ToArray()),
                    new MWNumericArray(clustersNum));

                var means = (double[,])result[0].ToArray();
                var covariances= (double[,])result[1].ToArray();
                var priors = (double[]) ((MWNumericArray) result[2]).ToVector(MWArrayComponent.Real);

                result = null;
                cluster.Dispose();
                
                return new GmmModel(
                    Arrays.ToJaggedArray(ref means).ToList(),
                    Arrays.ToJaggedArray(ref covariances).ToList(),
                    priors);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "GMM";
        }

    }
}
