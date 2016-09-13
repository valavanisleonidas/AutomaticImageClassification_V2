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
    public class VlFeatEm : ICluster
    {
        private bool _isRandomInit;
        private int _numberOfFeatures;
        private Clustering cluster = new Clustering();

        public VlFeatEm()
        {
            _isRandomInit = false;
            _numberOfFeatures = int.MaxValue;
        }

        public VlFeatEm(bool isRandomInit)
        {
            this._isRandomInit = isRandomInit;
        }
        public VlFeatEm(int numberOfFeatures)
        {
            this._numberOfFeatures = numberOfFeatures;
        }

        public VlFeatEm(bool isRandomInit, int numberOfFeatures)
        {
            this._isRandomInit = isRandomInit;
            this._numberOfFeatures = numberOfFeatures;
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

                MWArray[] result = cluster.EM(3,
                    new MWNumericArray(descriptorFeatures.ToArray()),
                    new MWNumericArray(clustersNum),
                    new MWLogicalArray(_isRandomInit));

                var features = (double[,])result[0].ToArray();

                return Arrays.ToJaggedArray(ref features).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
