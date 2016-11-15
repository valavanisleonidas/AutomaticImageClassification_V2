using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class VlFeatSift : IFeatures
    {
        private readonly int _width;
        private readonly int _height;
        private readonly ClusterModel _clusterModel;

        public VlFeatSift(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public VlFeatSift(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }

        public double[] ExtractHistogram(string input)
        {
            List<double[]> features = ExtractDescriptors(input);
            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            List<int> indexes = _clusterModel.Tree.SearchTree(features);
            foreach (var index in indexes)
            {
                imgVocVector[index]++;
            }

            return imgVocVector;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            List<double[]> descriptors;
            ExtractSift(input, out descriptors);
            return descriptors;
        }

        public void ExtractSift(string input, out List<double[]> descriptors)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

                //return frames descriptors( features )
                MWArray[] result = phow.GetPhow(2,
                    new MWCharArray(input),
                    new MWNumericArray(_height),
                    new MWNumericArray(_width));

                var _descriptors = (double[,])result[1].ToArray();

                phow.Dispose();

                descriptors = Arrays.ToJaggedArray(ref _descriptors).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ExtractSift(string input, out List<double[]> descriptors, out List<double[]> frames)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

                //return frames descriptors( features )
                MWArray[] result = phow.GetPhow(2,
                    new MWCharArray(input),
                    new MWNumericArray(_height),
                    new MWNumericArray(_width));

                var _frames = (double[,])result[0].ToArray();
                var _descriptors = (double[,])result[1].ToArray();

                phow.Dispose();

                descriptors = Arrays.ToJaggedArray(ref _descriptors).ToList();
                frames = Arrays.ToJaggedArray(ref _frames).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
