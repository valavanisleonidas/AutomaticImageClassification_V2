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
        private string _extractionColor = "rgb";
        private IKdTree _tree;
        private int[,] _numSpatialX = { { 1, 2, 4 } }, _numSpatialY = { { 1, 2, 4 } };
        private int _clusterNum, _width = 256, _height = 256;

        public VlFeatPhow() { }

        public VlFeatPhow(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }

        public VlFeatPhow(IKdTree tree, int clusterNum,int width,int height)
        {
            _tree = tree;
            _clusterNum = clusterNum;
            _width = width;
            _height = height;
        }

        public VlFeatPhow(string extractionColor, IKdTree tree, int[,] numSpatialX, int[,] numSpatialY,
            int clusterNum, int width, int height)
        {
            _extractionColor = extractionColor;
            _tree = tree;
            _numSpatialX = numSpatialX;
            _numSpatialY = numSpatialY;
            _clusterNum = clusterNum;
            _width = width;
            _height = height;
        }

        public double[] ExtractHistogram(string input)
        {
            try
            {
                //if not right width height then error so if not 256 BE CAREFUL
                //get image width and height
                //Bitmap image = new Bitmap(input);
                //_width = image.Width > 256 ? 256 : image.Width;
                //_height = image.Height > 256 ? 256 : image.Height;
                
                List<double[]> features;
                List<double[]> frames;
                ExtractPhow(input, out features, out frames);
                List<int> indexes = _tree.SearchTree(features);
                
                return CombineQuantizations(frames, indexes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<double[]> ExtractDescriptors(string input)
        {
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
                MWArray[] result = phow.GetPhow(2, new MWCharArray(input), _extractionColor);
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
                MWArray[] result = phow.GetPhow(2, new MWCharArray(input), _extractionColor);
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

        private double[] CombineQuantizations(List<double[]> frames, List<int> indexes)
        {
            try
            {
                var quantizations = new MatlabAPI.Quantizations();
                
                //return frames descriptors( features )
                MWArray[] result = quantizations.CombineQuantizations(1,
                    new MWNumericArray(frames.ToArray()),
                    new MWNumericArray(new[] { indexes.Select(a => a + 1).ToArray() }),
                    new MWNumericArray(_width),
                    new MWNumericArray(_height),
                    new MWNumericArray(_clusterNum),
                    new MWNumericArray(_numSpatialX),
                    new MWNumericArray(_numSpatialY)
                    );

                quantizations.Dispose();
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
