using java.awt.image;
using java.io;
using net.semanticmetadata.lire.imageanalysis;
using net.semanticmetadata.lire.imageanalysis.correlogram;
using System;
using System.Collections.Generic;

namespace AutomaticImageClassification.Feature
{
    public class ColorCorrelogram : IFeatures
    {
        private ExtractionMethod EXTRACTION_METHOD;
        private IAutoCorrelogramFeatureExtractor extractionAlgorithm;

        public ColorCorrelogram()
        {
            EXTRACTION_METHOD = ExtractionMethod.NaiveHuangAlgorithm;
        }

        public ColorCorrelogram(ExtractionMethod ExtractionMethod)
        {
            EXTRACTION_METHOD = ExtractionMethod;
        }

        public double[] ExtractHistogram(string input)
        {
            //convert the image to jpg if it is png because the descriptor does not support png images
            File image = ImageFilter.isPNG(input)
                         ? ImageConverter.PNGtoJPG(input)
                         : new File(input);

            BufferedImage Bimage = ImageUtility.getImage(image.getAbsolutePath());
            if (ImageFilter.isPNG(input))
                image.delete();


            switch (EXTRACTION_METHOD)
            {
                case ExtractionMethod.LireAlgorithm:
                    extractionAlgorithm = new MLuxAutoCorrelogramExtraction();
                    break;
                case ExtractionMethod.NaiveHuangAlgorithm:
                    extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
                    break;
                case ExtractionMethod.DynamicProgrammingHuangAlgorithm:
                    extractionAlgorithm = DynamicProgrammingAutoCorrelogramExtraction.getInstance();
                    break;
            }

            AutoColorCorrelogram color = new AutoColorCorrelogram(extractionAlgorithm);
            Raster r = Bimage.getRaster();
            int[][][] HsvImage = hsvImage(r);
            color.extract(HsvImage);

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

        private static int[][][] hsvImage(Raster r)
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
                    int[] arr = r.getPixel(x, y, pixel);
                    // converting to HSV:
                    int[] hsv = new int[3];
                    convertRgbToHsv(r.getPixel(x, y, pixel), hsv);
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
        private static void convertRgbToHsv(int[] rgb, int[] hsv)
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
            hsv[0] = (int)(hue);
            // value in [0,255]
            hsv[2] = max;
        }




    }
}
