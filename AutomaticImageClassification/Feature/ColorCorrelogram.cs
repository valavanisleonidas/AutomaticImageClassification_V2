using java.awt.image;
using net.semanticmetadata.lire.imageanalysis;
using net.semanticmetadata.lire.imageanalysis.correlogram;
using System;
using System.Collections.Generic;
using System.Drawing;
using java.awt;

namespace AutomaticImageClassification.Feature
{
    public class ColorCorrelogram : IFeatures
    {
        private ExtractionMethod _extractionMethod;
        private IAutoCorrelogramFeatureExtractor _extractionAlgorithm;

        public ColorCorrelogram()
        {
            _extractionMethod = ExtractionMethod.NaiveHuangAlgorithm;
        }

        public ColorCorrelogram(ExtractionMethod extractionMethod)
        {
            _extractionMethod = extractionMethod;
        }

        public double[] ExtractHistogram(string input)
        {
            var bimage = new BufferedImage(new Bitmap(input));

            BufferedImage newImage = new BufferedImage(bimage.getWidth(), bimage.getHeight(), 5);

            Graphics2D g = newImage.createGraphics();
            g.drawImage(bimage, 0, 0, null);
            g.dispose();


            switch (_extractionMethod)
            {
                case ExtractionMethod.LireAlgorithm:
                    _extractionAlgorithm = new MLuxAutoCorrelogramExtraction();
                    break;
                case ExtractionMethod.NaiveHuangAlgorithm:
                    _extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
                    break;
                case ExtractionMethod.DynamicProgrammingHuangAlgorithm:
                    _extractionAlgorithm = DynamicProgrammingAutoCorrelogramExtraction.getInstance();
                    break;
            }

            AutoColorCorrelogram color = new AutoColorCorrelogram(_extractionAlgorithm);
            Raster r = bimage.getRaster();
            int[][][] hsvImage = ColorCorrelogram.HsvImage(r);
            color.extract(hsvImage);

            return color.getDoubleHistogram();
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            throw new NotImplementedException("Auto color correlogram returns the final histogram and not descriptors of an image!");
        }

        public enum ExtractionMethod
        {
            LireAlgorithm,
            NaiveHuangAlgorithm,
            DynamicProgrammingHuangAlgorithm
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
                    pixels[x][y] = new int[3];
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
        {     //TODO: Conversion
            if (hsv.Length < 3)
            {
                throw new IndexOutOfRangeException("HSV array too small, a minimum of three elements is required.");
            }
            int R = rgb[0];
            int G = rgb[1];
            int B = rgb[2];
            int max, min;
            float hue = 0f;

            max = Math.Max(R, G);     //calculation of max(R,G,B)
            max = Math.Max(max, B);

            min = Math.Min(R, G);     //calculation of min(R,G,B)
            min = Math.Min(min, B);

            if (max == 0)
                hsv[1] = 0;
            else
            {
                // Saturation in [0,255]
                hsv[1] = (int)(((max - min) / (float)max) * 255f);
            }

            if (max == min)
            {
                hue = 0;     // (max - min) = 0
            }
            else
            {
                float maxMinusMin = (max - min);
                if (R == max)
                    hue = ((G - B) / maxMinusMin);

                else if (G == max)
                    hue = (2 + (B - R) / maxMinusMin);

                else if (B == max)
                    hue = (4 + (R - G) / maxMinusMin);

                hue *= 60f;

                if (hue < 0f)
                    hue += 360f;
            }
            // hue in [0,359]
            hsv[0] = (int)hue;
            // value in [0,255]
            hsv[2] = max;
        }

        public override string ToString()
        {
            return "ColorCorrelogram";
        }


    }
}
