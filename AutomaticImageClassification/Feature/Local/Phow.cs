using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Local
{
    //from vlfeat
    public class Phow : ILocalFeatures
    {
        private readonly string _extractionColor = "rgb";
        private string _quantizer = "kdtree";
        private int[,] _numSpatialX = { { 1, 2, 4 } };
        private readonly int[,] _numSpatialY = { { 1, 2, 4 } };
        private int _width;
        private int _height;
        private readonly ClusterModel _clusterModel;

   
        public Phow() {
        }

        public Phow(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }


        public Phow(ClusterModel clusterModel, int width, int height)
        {
            _clusterModel = clusterModel;
            _width = width;
            _height = height;

        }

        public Phow(ClusterModel clusterModel, string extractionColor, int[,] numSpatialX, int[,] numSpatialY,
            int width, int height)
        {
            _clusterModel = clusterModel;
            _extractionColor = extractionColor;
            _numSpatialX = numSpatialX;
            _numSpatialY = numSpatialY;
            _width = width;
            _height = height;
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
         
            try
            {
                return ExtractFastPhow(input.Path); ;
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
            return "Phow_" + _extractionColor + "_" + string.Join("_", Arrays.ToJaggedArray(ref _numSpatialX)[0]);
        }
    }
}
