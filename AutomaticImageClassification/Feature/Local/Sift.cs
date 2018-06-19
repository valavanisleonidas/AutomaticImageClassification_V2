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
        private int _width;
        private int _height;
        private readonly bool _useCombinedQuantization;
        private readonly int[,] _numSpatialX = { { 1, 2, 4 } };
        private readonly int[,] _numSpatialY = { { 1, 2, 4 } };
        private readonly ClusterModel _clusterModel;

        public bool CanCluster
        {
            get { return true; }
        }

        public Sift()
        {
            _useCombinedQuantization = true;
        }

        public Sift(bool useCombinedQuantization)
        {
            _useCombinedQuantization = useCombinedQuantization;
        }

        public Sift(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
            _useCombinedQuantization = true;
        }

        public Sift(ClusterModel clusterModel, bool useCombinedQuantization)
        {
            _clusterModel = clusterModel;
            _useCombinedQuantization = useCombinedQuantization;
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            _width = input.ImageWidth;
            _height = input.ImageHeight;

            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

            if (!_useCombinedQuantization)
            {
                List<double[]> features;
                ExtractSift(input.Path, out features);

                //for each centroid find min position in tree and increase corresponding index
                List<int> indexes = _clusterModel.Tree.SearchTree(features);
                foreach (var index in indexes)
                {
                    imgVocVector[index]++;
                }
            }
            else
            {
                List<double[]> features;
                List<double[]> frames;
                ExtractSift(input.Path, out features, out frames);
                List<int> indexes = _clusterModel.Tree.SearchTree(features);

                imgVocVector = Quantization.CombineQuantizations(frames, indexes, _width, _height, _clusterModel.ClusterNum, _numSpatialX, _numSpatialY);
            }
            return imgVocVector;            
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            _width = input.ImageWidth;
            _height = input.ImageHeight;

            List<double[]> descriptors;
            ExtractSift(input.Path, out descriptors);
            return descriptors;
        }

        public void ExtractSift(string input, out List<double[]> descriptors)
        {
            try
            {
                var sift = new MatlabAPI.Sift();

                //return frames descriptors( features )
                MWArray[] result = sift.GetSift(2,
                    new MWCharArray(input),
                    new MWNumericArray(_height),
                    new MWNumericArray(_width));

                var _descriptors = (double[,])result[1].ToArray();

                sift.Dispose();

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
                var sift = new MatlabAPI.Sift();

                //return frames descriptors( features )
                MWArray[] result = sift.GetSift(2,
                    new MWCharArray(input),
                    new MWNumericArray(_height),
                    new MWNumericArray(_width));

                var _frames = (double[,])result[0].ToArray();
                var _descriptors = (double[,])result[1].ToArray();

                sift.Dispose();

                descriptors = Arrays.ToJaggedArray(ref _descriptors).ToList();
                frames = Arrays.ToJaggedArray(ref _frames).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "VlFeatSift";
        }

    }
}
