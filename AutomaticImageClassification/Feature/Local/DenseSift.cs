using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Local
{
    //From VLFeat
    public class DenseSift : ILocalFeatures
    {
        private readonly int _step = 4;
        private readonly int[,] _numSpatialX = { { 1, 2, 4 } };
        private readonly int[,] _numSpatialY = { { 1, 2, 4 } };
        private readonly bool _rootSift = true;
        private readonly bool _normalizeSift = true;
        private readonly ClusterModel _clusterModel;
        
        public DenseSift() {  }

        public DenseSift(bool isRootSift, bool isNormalizedSift)
        {
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
        }


        public DenseSift(int step, bool isRootSift, bool isNormalizedSift)
        {
            _step = step;
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
        }

        public DenseSift(ClusterModel clusterModel, int step, bool isRootSift, bool isNormalizedSift)
        {
            _clusterModel = clusterModel;
            _step = step;
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
        }
        

        public DenseSift(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            

            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

            List<double[]> features;
            ExtractDenseSift(input.Path, input.ImageHeight, input.ImageWidth, out features);

            //for each centroid find min position in tree and increase corresponding index
            List<int> indexes = _clusterModel.Tree.SearchTree(features);
            foreach (var index in indexes)
            {
                imgVocVector[index]++;
            }

            return imgVocVector;
        }
        

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            List<double[]> descriptors;
            ExtractDenseSift(input.Path, input.ImageHeight, input.ImageWidth, out descriptors);
            return descriptors;
        }

  
        public void ExtractDenseSift(string input, int height, int width, out List<double[]> descriptors)
        {
            try
            {
                var phow = new MatlabAPI.DenseSift();

                //return frames, descriptors( features ), contrast
                MWArray[] result = phow.GetDenseSIFT(3, new MWCharArray(input),
                    new MWLogicalArray(_rootSift),
                    new MWLogicalArray(_normalizeSift),
                    _step,
                    new MWNumericArray(height),
                    new MWNumericArray(width));

                var features = (double[,])result[1].ToArray();

                phow.Dispose();

                descriptors = Arrays.ToJaggedArray(ref features).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public override string ToString()
        {
            return "DenseSift" + (_rootSift ? "_root" : "") + (_normalizeSift ? "_normalized" : "");
        }

    }
}
