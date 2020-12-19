using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Local
{
    //From vlfeat
    public class Sift : IFeatures
    {
        private int[,] _numSpatialX = { { 1, 2, 4 } };
        private int[,] _numSpatialY = { { 1, 2, 4 } };
        private readonly ClusterModel _clusterModel;

        public bool CanCluster
        {
            get { return true; }
        }

        public Sift()
        {
        }
        

        public Sift(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }
        

        public double[] ExtractHistogram(LocalBitmap input)
        {
            double[] imgVocVector = new double[_clusterModel.ClusterNum];

            List<double[]> features;
            ExtractSift(input.Path, input.ImageWidth, input.ImageHeight, out features);

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
            ExtractSift(input.Path, input.ImageWidth, input.ImageHeight, out descriptors);
            return descriptors;
        }

        public void ExtractSift(string input, int width, int height, out List<double[]> descriptors)
        {
            try
            {
                var sift = new MatlabAPI.Sift();

                //return frames descriptors( features )
                MWArray[] result = sift.GetSift(2,
                    new MWCharArray(input),
                    new MWNumericArray(height),
                    new MWNumericArray(width));

                var _descriptors = (double[,])result[1].ToArray();

                sift.Dispose();

                descriptors = Arrays.ToJaggedArray(ref _descriptors).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "Sift" + "_" + string.Join("_", Arrays.ToJaggedArray(ref _numSpatialX)[0]);
        }

    }
}
