using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class VlFeatDenseSift : IFeatures
    {
        private static int _step, _width, _height;
        private readonly int[,] _numSpatialX = { { 1, 2, 4 } };
        private readonly int[,] _numSpatialY = { { 1, 2, 4 } };
        private static bool _rootSift, _normalizeSift, _useCombinedQuantization;
        private readonly ClusterModel _clusterModel;

        public VlFeatDenseSift()
        {
            _step = 4;
            _rootSift = false;
            _normalizeSift = true;
            _useCombinedQuantization = true;
        }

        public VlFeatDenseSift(int step, bool isRootSift, bool isNormalizedSift, bool useCombinedQuantization, int height, int width)
        {
            _step = step;
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
            _useCombinedQuantization = useCombinedQuantization;
            _height = height;
            _width = width;
        }


        public VlFeatDenseSift(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }

        public VlFeatDenseSift(ClusterModel clusterModel, int height, int width)
        {
            _clusterModel = clusterModel;
            _height = height;
            _width = width;
        }

        public double[] ExtractHistogram(string input)
        {
            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

            //if not right width height then error so  BE CAREFUL
            //get image width and height
            //Bitmap image = new Bitmap(input);
            //if (image.Height > 480)
            //{
            //    image = ImageProcessing.ResizeImage(image, 480);
            //}
            //_width = image.Width;
            //_height = image.Height;

            if (!_useCombinedQuantization)
            {
                List<double[]> features;
                ExtractDenseSift(input, out features);

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
                ExtractDenseSift(input, out features, out frames);
                List<int> indexes = _clusterModel.Tree.SearchTree(features);

                imgVocVector = Quantization.CombineQuantizations(frames, indexes, _width, _height, _clusterModel.ClusterNum, _numSpatialX, _numSpatialY);
            }

            return imgVocVector;
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

            List<double[]> descriptors;
            ExtractDenseSift(input, out descriptors);
            return descriptors;
        }

        public void ExtractDenseSift(string input, out List<double[]> descriptors)
        {
            try
            {
                var phow = new MatlabAPI.DenseSift();

                //return frames, descriptors( features ), contrast
                MWArray[] result = phow.GetDenseSIFT(3, new MWCharArray(input),
                    new MWLogicalArray(_rootSift),
                    new MWLogicalArray(_normalizeSift),
                    _step,
                    new MWNumericArray(_height),
                    new MWNumericArray(_width));

                var features = (double[,])result[1].ToArray();

                phow.Dispose();

                descriptors = Arrays.ToJaggedArray(ref features).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ExtractDenseSift(string input, out List<double[]> descriptors, out List<double[]> frames)
        {
            try
            {
                var phow = new MatlabAPI.DenseSift();

                //return frames, descriptors( features ), contrast
                MWArray[] result = phow.GetDenseSIFT(3, new MWCharArray(input),
                    new MWLogicalArray(_rootSift),
                    new MWLogicalArray(_normalizeSift),
                    _step,
                    new MWNumericArray(_height),
                    new MWNumericArray(_width));


                var fr = (double[,])result[0].ToArray();
                var features = (double[,])result[1].ToArray();

                phow.Dispose();

                descriptors = Arrays.ToJaggedArray(ref features).ToList();
                frames = Arrays.ToJaggedArray(ref fr).ToList();
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
