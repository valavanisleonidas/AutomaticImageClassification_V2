using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;
using AutomaticImageClassification.Feature.Global.Correlogram;

namespace AutomaticImageClassification.Feature.Global
{
    public class ColorCorrelogram : IFeatures
    {
        private readonly ColorCorrelogramExtractionMethod _colorCorrelogramExtractionMethod;
        private readonly IAutoCorrelogramFeature _extractionAlgorithm;
        private readonly AutoColorCorrelogram _color;
        public bool CanCluster
        {
            get { return false; }
        }

        public ColorCorrelogram()
        {
            _colorCorrelogramExtractionMethod = ColorCorrelogramExtractionMethod.LireAlgorithm;
        }

        public ColorCorrelogram(ColorCorrelogramExtractionMethod colorCorrelogramExtractionMethod)
        {
            switch (colorCorrelogramExtractionMethod)
            {
                case ColorCorrelogramExtractionMethod.LireAlgorithm:
                    _extractionAlgorithm = new MLuxAutoCorrelogramExtraction();
                    _colorCorrelogramExtractionMethod = ColorCorrelogramExtractionMethod.LireAlgorithm;
                    break;
                case ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm:
                    _extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
                    _colorCorrelogramExtractionMethod = ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm;
                    break;
                default:
                    _extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
                    _colorCorrelogramExtractionMethod = ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm;
                    break;
            }

            _color = new AutoColorCorrelogram(_extractionAlgorithm);

        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            return _color.extract(input);
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            throw new NotImplementedException("Auto color correlogram returns the final histogram and not descriptors of an image!");
        }
 
        public enum ColorCorrelogramExtractionMethod
        {
            LireAlgorithm,
            NaiveHuangAlgorithm
        }

        public override string ToString()
        {
            return "ColorCorrelogram" + _colorCorrelogramExtractionMethod;
        }

        
    }
}
