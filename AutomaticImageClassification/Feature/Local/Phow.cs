using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Local
{
    //from vlfeat
    public class Phow : IFeatures
    {
        private readonly string _extractionColor = "rgb";
        private string _quantizer = "kdtree";
        private int[,] _numSpatialX = { { 1, 2, 4 } };
        private readonly int[,] _numSpatialY = { { 1, 2, 4 } };
        private int _width;
        private int _height;
        private readonly bool _isFastPhow;
        private readonly ClusterModel _clusterModel;

        public bool CanCluster
        {
            get { return true; }
        }

        public Phow() {
            _isFastPhow = true;
        }

        public Phow(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
            _isFastPhow = true;
        }

        public Phow(ClusterModel clusterModel, bool isFastPhow)
        {
            _clusterModel = clusterModel;
            _isFastPhow = isFastPhow;
        }

        public Phow(ClusterModel clusterModel, int width, int height, bool isFastPhow)
        {
            _clusterModel = clusterModel;
            _width = width;
            _height = height;
            _isFastPhow = isFastPhow;

        }

        public Phow(ClusterModel clusterModel, string extractionColor, int[,] numSpatialX, int[,] numSpatialY,
            int width, int height, bool isFastPhow)
        {
            _clusterModel = clusterModel;
            _extractionColor = extractionColor;
            _numSpatialX = numSpatialX;
            _numSpatialY = numSpatialY;
            _width = width;
            _height = height;
            _isFastPhow = isFastPhow;
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            _width = input.ImageWidth;
            _height = input.ImageHeight;

            try
            {
                double[] imgVocVector;
                if (_isFastPhow)
                {
                    imgVocVector = ExtractFastPhow(input.Path);
                }
                else
                {
                    List<double[]> features;
                    List<double[]> frames;
                    ExtractPhow(input.Path, out features, out frames);
                    List<int> indexes = _clusterModel.Tree.SearchTree(features);
                    imgVocVector = Quantization.CombineQuantizations(frames, indexes, _width, _height, _clusterModel.ClusterNum, _numSpatialX, _numSpatialY);
                }
                return imgVocVector;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            _width = input.ImageWidth;
            _height = input.ImageHeight;

            List<double[]> features;
            ExtractPhow(input.Path, out features);
            return features;
        }

        private void ExtractPhow(string input, out List<double[]> descriptors)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

                //return frames descriptors( features )
                MWArray[] result = phow.GetPhow(2,
                    new MWCharArray(input),
                    _extractionColor,
                    new MWNumericArray(_height),
                    new MWNumericArray(_width));


                var desc = (double[,])result[1].ToArray();

                phow.Dispose();
                descriptors = Arrays.ToJaggedArray(ref desc).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ExtractPhow(string input, out List<double[]> descriptors, out List<double[]> frames)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

                //return frames descriptors( features )
                MWArray[] result = phow.GetPhow(2,
                    new MWCharArray(input),
                    _extractionColor,
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

        public double[] ExtractFastPhow(string input)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

                //return histogram
                MWArray[] result = phow.ExtractFeatures(1,
                    new MWCharArray(input),
                    new MWNumericArray(_clusterModel.Means.ToArray()),
                    _quantizer,
                    new MWNumericArray(_numSpatialX),
                    new MWNumericArray(_numSpatialY),
                    _extractionColor);

                phow.Dispose();

                return (double[])((MWNumericArray)result[0]).ToVector(MWArrayComponent.Real);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "Phow_" + (_isFastPhow ? "_fast" : "")+ "_" + _extractionColor + "_" + string.Join("_", Arrays.ToJaggedArray(ref _numSpatialX)[0]);
        }
    }
}
