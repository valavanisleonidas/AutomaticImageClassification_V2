using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;
using MatlabAPI;

namespace AutomaticImageClassification.Feature
{
    public class Phow : IFeatures
    {
        private string _extractionColor = "gray", _quantizer = "kdtree";
        private List<double[]> _vocab;
        private int[,] _numSpatialX = { { 1, 2, 4 } }, _numSpatialY = { { 1, 2, 4 } };

        public Phow() { }

        public Phow(List<double[]> vocab)
        {
            _vocab = vocab;
        }

        public Phow(string extractionColor, string quantizer, int[,] numSpatialX, int[,] numSpatialY,
            List<double[]> vocab)
        {
            _extractionColor = extractionColor;
            _quantizer = quantizer;
            _numSpatialX = numSpatialX;
            _numSpatialY = numSpatialY;
            _vocab = vocab;
        }

        public double[] ExtractHistogram(string input)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

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

        public List<double[]> ExtractDescriptors(string input)
        {
            try
            {
                var phow = new MatlabAPI.Phow();

                //return frames descriptors( features )
                MWArray[] result = phow.GetPhow(2, new MWCharArray(input));
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
            return "Phow_" + _extractionColor + "_" + string.Join("_", Arrays.ToJaggedArray(ref _numSpatialX)[0]);
        }
    }
}
