using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.KDTree;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class VlFeatPhow : IFeatures
    {
        private string _extractionColor = "rgb", _quantizer = "kdtree";
        private IKdTree _tree;
        private List<double[]> _vocab;

        private int[,] _numSpatialX = { { 1, 2, 4 } }, _numSpatialY = { { 1, 2, 4 } };
        private int _clusterNum, _width, _height;
        private bool _isFastPhow = true;

        public VlFeatPhow() { }

        public VlFeatPhow(List<double[]> vocab, bool isFastPhow)
        {
            _vocab = vocab;
            _isFastPhow = isFastPhow;
        }

        public VlFeatPhow(List<double[]> vocab)
        {
            _vocab = vocab;
            _isFastPhow = true;
        }

        public VlFeatPhow(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }

        public VlFeatPhow(IKdTree tree, int clusterNum, bool isFastPhow)
        {
            _tree = tree;
            _clusterNum = clusterNum;
            _isFastPhow = isFastPhow;
        }

        public VlFeatPhow(IKdTree tree, int clusterNum, int width, int height, bool isFastPhow)
        {
            _tree = tree;
            _clusterNum = clusterNum;
            _width = width;
            _height = height;
            _isFastPhow = isFastPhow;

        }

        public VlFeatPhow(string extractionColor, IKdTree tree, int[,] numSpatialX, int[,] numSpatialY,
            int clusterNum, int width, int height, bool isFastPhow)
        {
            _extractionColor = extractionColor;
            _tree = tree;
            _numSpatialX = numSpatialX;
            _numSpatialY = numSpatialY;
            _clusterNum = clusterNum;
            _width = width;
            _height = height;
            _isFastPhow = isFastPhow;
        }

        public double[] ExtractHistogram(string input)
        {
            try
            {
                //if not right width height then error so  BE CAREFUL
                //get image width and height
                //Bitmap image = new Bitmap(input);
                //if (image.Height > 480)
                //{
                //    image = ImageProcessing.ResizeImage(image, 480);
                //}
                //_width = image.Width;
                //_height = image.Height;

                double[] imgVocVector;
                if (_isFastPhow)
                {
                    imgVocVector = ExtractFastPhow(input);
                }
                else
                {
                    List<double[]> features;
                    List<double[]> frames;
                    ExtractPhow(input, out features, out frames, out _height, out _width);
                    List<int> indexes = _tree.SearchTree(features);
                    imgVocVector = Quantization.CombineQuantizations(frames, indexes, _width, _height, _clusterNum, _numSpatialX, _numSpatialY);
                }
                return imgVocVector;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            //if not right width height then error so  BE CAREFUL
            //get image width and height
            //Bitmap image = new Bitmap(input);
            //if (image.Height > 480)
            //{
            //    image = ImageProcessing.ResizeImage(image, 480);
            //}
            //_width = image.Width;
            //_height = image.Height;

            List<double[]> features;
            ExtractPhow(input, out features);
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
                    _extractionColor);
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
                    _extractionColor);

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

        public void ExtractPhow(string input, out List<double[]> descriptors, out List<double[]> frames, out int height, out int width)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

                //return frames descriptors( features )
                MWArray[] result = phow.GetPhow(2,
                    new MWCharArray(input),
                    _extractionColor);

                var _frames = (double[,])result[0].ToArray();
                var _descriptors = (double[,])result[1].ToArray();
                height = ((MWNumericArray)result[2]).ToScalarInteger();
                width = ((MWNumericArray)result[3]).ToScalarInteger();

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
                    new MWNumericArray(_vocab.ToArray()),
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
            return "Phow_" + _extractionColor + "_" + string.Join("_", Arrays.ToJaggedArray(ref _numSpatialX)[0]);
        }
    }
}
