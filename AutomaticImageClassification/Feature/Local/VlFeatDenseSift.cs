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

namespace AutomaticImageClassification.Feature.Local
{
    public class VlFeatDenseSift : IFeatures
    {
        private int _width, _height;
        private readonly int _step = 4;
        private readonly int[,] _numSpatialX = { { 1, 2, 4 } };
        private readonly int[,] _numSpatialY = { { 1, 2, 4 } };
        private readonly bool _rootSift = true;
        private readonly bool _normalizeSift = true;
        private readonly bool _useCombinedQuantization;
        private readonly ClusterModel _clusterModel;

        public bool CanCluster
        {
            get { return true; }
        }

        public VlFeatDenseSift()
        {
            _useCombinedQuantization = true;
        }

        public VlFeatDenseSift(bool useCombinedQuantization)
        {
            _useCombinedQuantization = useCombinedQuantization;
        }

        public VlFeatDenseSift(int step, bool isRootSift, bool isNormalizedSift, bool useCombinedQuantization)
        {
            _step = step;
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
            _useCombinedQuantization = useCombinedQuantization;
        }

        public VlFeatDenseSift(ClusterModel clusterModel, int step, bool isRootSift, bool isNormalizedSift, bool useCombinedQuantization)
        {
            _clusterModel = clusterModel;
            _step = step;
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
            _useCombinedQuantization = useCombinedQuantization;
        }

        public VlFeatDenseSift(ClusterModel clusterModel, bool useCombinedQuantization)
        {
            _clusterModel = clusterModel;
            _step = 4;
            _rootSift = true;
            _normalizeSift = true;
            _useCombinedQuantization = useCombinedQuantization;
        }

        public VlFeatDenseSift(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            _width = input.ImageWidth;
            _height = input.ImageHeight;

            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

            if (!_useCombinedQuantization)
            {
                List<double[]> features;
                ExtractDenseSift(input.Path, out features);

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
                ExtractDenseSift(input.Path, out features, out frames);
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
            ExtractDenseSift(input.Path, out descriptors);
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
