using java.awt.image;
using net.semanticmetadata.lire.imageanalysis;
using net.semanticmetadata.lire.imageanalysis.correlogram;
using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Global
{
    public class ColorCorrelogram : IGlobalFeatures
    {
        private readonly ColorCorrelogramExtractionMethod _colorCorrelogramExtractionMethod;
        private readonly IAutoCorrelogramFeatureExtractor _extractionAlgorithm;

        public ColorCorrelogram()
        {
            _colorCorrelogramExtractionMethod = ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm;
            _extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
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
                // this implementation consumes all the memory and is really slow
                //case ColorCorrelogramExtractionMethod.DynamicProgrammingHuangAlgorithm:
                //    _extractionAlgorithm = DynamicProgrammingAutoCorrelogramExtraction.getInstance();
                //    break;
                default:
                    _colorCorrelogramExtractionMethod = ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm;
                    _extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
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
                    // converting to HSV:
                    int[] hsv = new int[3];
                    ConvertRgbToHsv(r.getPixel(x, y, pixel), hsv);
                    // quantize the actual pixel:
                    pixels[x][y] = hsv;
                }
            }
            return pixels;
        }

        /**
     * @param rgb RGB Values
     * @param hsv HSV values to set.
     */

        private static void ConvertRgbToHsv(int[] rgb, int[] hsv)
        {
            //TODO: Conversion
            if (hsv.Length < 3)
            {
                throw new IndexOutOfRangeException("HSV array too small, a minimum of three elements is required.");
            }
            int R = rgb[0];
            int G = rgb[1];
            int B = rgb[2];
            int max, min;
            float hue = 0f;

            max = Math.Max(R, G); //calculation of max(R,G,B)
            max = Math.Max(max, B);

            min = Math.Min(R, G); //calculation of min(R,G,B)
            min = Math.Min(min, B);

            if (max == 0)
                hsv[1] = 0;
            else
            {
                // Saturation in [0,255]
                hsv[1] = (int) (((max - min)/(float) max)*255f);
            }

            if (max == min)
            {
                hue = 0; // (max - min) = 0
            }
            else
            {
                float maxMinusMin = (max - min);
                if (R == max)
                    hue = ((G - B)/maxMinusMin);

                else if (G == max)
                    hue = (2 + (B - R)/maxMinusMin);

                else if (B == max)
                    hue = (4 + (R - G)/maxMinusMin);

                hue *= 60f;

                if (hue < 0f)
                    hue += 360f;
            }
            // hue in [0,359]
            hsv[0] = (int) hue;
            // value in [0,255]
            hsv[2] = max;
        }

        public override string ToString()
        {
            return "ColorCorrelogram" + _colorCorrelogramExtractionMethod;
        }

        
    }
}
