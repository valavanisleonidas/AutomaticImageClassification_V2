using java.awt.image;
using net.semanticmetadata.lire.imageanalysis;
using net.semanticmetadata.lire.imageanalysis.correlogram;
using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class ColorCorrelogram : IFeatures
    {
        private readonly ColorCorrelogramExtractionMethod _colorCorrelogramExtractionMethod;
        private readonly IAutoCorrelogramFeatureExtractor _extractionAlgorithm;

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
                    break;
                case ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm:
                    _extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
                    break;
                // this implementation consumes all the memory and is really slow
                //case ColorCorrelogramExtractionMethod.DynamicProgrammingHuangAlgorithm:
                //    _extractionAlgorithm = DynamicProgrammingAutoCorrelogramExtraction.getInstance();
                //    break;
                default:
                    _colorCorrelogramExtractionMethod = ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm;
                    break;
            }

        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            var bimage = new BufferedImage(input.Bitmap);
            
            AutoColorCorrelogram color = new AutoColorCorrelogram(_extractionAlgorithm);
            Raster r = bimage.getRaster();
            int[][][] hsvImage = HsvImage(r);
            color.extract(hsvImage);

            return color.getDoubleHistogram();
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            throw new NotImplementedException("Auto color correlogram returns the final histogram and not descriptors of an image!");
        }
 
        public enum ColorCorrelogramExtractionMethod
        {
            LireAlgorithm,
            NaiveHuangAlgorithm,
            //DynamicProgrammingHuangAlgorithm
        }

        private static int[][][] HsvImage(Raster r)
        {
            int[][][] pixels = new int[r.getWidth()][][];
            // quantize colors for each pixel (done in HSV color space):
            //raster.getpixel returns red green blue and alpha but we want only rgb colors
            int[] pixel = new int[4];

            for (int x = 0; x < r.getWidth(); x++)
            {
                pixels[x] = new int[r.getHeight()][];
                for (int y = 0; y < r.getHeight(); y++)
                {
                    int[] rgb = r.getPixel(x, y, pixel);
                    // converting to HSV:
                    
                    var hsv = ColorConversion.rgb2hsv(rgb[0], rgb[1], rgb[2]);
                    // quantize the actual pixel:
                    pixels[x][y] = new int[3];
                    pixels[x][y] = hsv;
                }
            }
            return pixels;
        }

  
        public override string ToString()
        {
            return "ColorCorrelogram" + _colorCorrelogramExtractionMethod;
        }

        
    }
}
