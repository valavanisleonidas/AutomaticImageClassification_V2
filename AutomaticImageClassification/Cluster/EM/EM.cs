using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Cluster.EM
{
    //from vlfeat
    public class EM : ICluster
    {
        private readonly bool _isRandomInit;
       
        public EM()
        {
            _isRandomInit = false;
        }
        
        public EM( bool isRandomInit)
        {
            _isRandomInit = isRandomInit;
        }

        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            try
            {
                var cluster = new MatlabAPI.Cluster();
             
                MWArray[] result = cluster.Em(3,
                    new MWNumericArray(descriptorFeatures.ToArray()),
                    new MWNumericArray(clustersNum),
                    new MWLogicalArray(_isRandomInit));

                var features = (double[,])result[0].ToArray();
                result = null;
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
            return "VlFeatEm_" + (_isRandomInit ? "RandomInit" : "KmeansInit");
        }

    }
}
