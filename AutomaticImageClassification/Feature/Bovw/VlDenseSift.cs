using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.KDTree;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class VlDenseSift : IFeatures
    {
        private static int _scale;
        private static bool _rootSift;
        private static bool _normalizeSift;
        private IKdTree _tree;
        private int _clusterNum;

        public VlDenseSift()
        {
            _scale = 4;
            _rootSift = false;
            _normalizeSift = true;
        }

        public VlDenseSift(int scale,bool isRootSift, bool isNormalizedSift)
        {
            _scale = scale;
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
        }


        public VlDenseSift(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }

        public double[] ExtractHistogram(string input)
        {
            List<double[]> features = ExtractDescriptors(input);
            double[] imgVocVector = new double[_clusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            foreach (var feature in features)
            {
                int positionofMin = _tree.SearchTree(feature);
                imgVocVector[positionofMin]++;
            }

            return imgVocVector;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            try
            {
                var phow = new MatlabAPI.DenseSift();

                //return frames, descriptors( features ), contrast
                MWArray[] result = phow.GetDenseSIFT(3, new MWCharArray(input), 
                    new MWLogicalArray(_rootSift),
                    new MWLogicalArray(_normalizeSift), 
                    _scale);
                var features = (double[,])result[1].ToArray();

                phow.Dispose();

                return Arrays.ToJaggedArray(ref features).ToList();
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
