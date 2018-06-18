using AutomaticImageClassification.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Feature.Bovw.Correlogram
{
    public class AutoColorCorrelogram
    {

        /**
 * <p>Feature for the AutoCorrelogram based on color as described in
 * Huang, J.; Kumar, S. R.; Mitra, M.; Zhu, W. & Zabih, R. (2007) "Image
 * Indexing Using Color Correlograms", IEEE Computer Society</p>
 * <p>see also DOI <a href="http://doi.ieeecomputersociety.org/10.1109/CVPR.1997.609412">10.1109/CVPR.1997.609412</a></p>
 * <p/>
 * Todo: Change the 2-dim array to a one dim array, as this is much faster in Java.
 */
        private static int DEFAULT_NUMBER_COLORS = 256;

        private float quantH;
        private float quantV;
        private float quantS;
        //    private int[][][] quantTable;
        private float[][] correlogram;
        private int[] distanceSet;
        private int numBins;
        private float quantH_f;
        private float quantS_f;
        private float quantV_f;


        private static ExtractionMethod DEFAULT_EXTRACTION_METHOD = ExtractionMethod.NaiveHuangAlgorithm;
        private IAutoCorrelogramFeature extractionAlgorithm;

        /**
         * Defines the available analysis modes: Superfast uses the approach described in the paper, Quarterneighbourhood
         * investigates the pixels in down and to the right of the respective pixel and FullNeighbourhood investigates
         * the whole lot of pixels within maximumDistance of the respective pixel.
         */
        public enum Mode
        {
            FullNeighbourhood,
            QuarterNeighbourhood,
            SuperFast
        }

        /**
         * Defines which algorithm to use to extract the features vector
         */
        public enum ExtractionMethod
        {
            LireAlgorithm,
            NaiveHuangAlgorithm,
            DynamicProgrammingHuangAlgorithm
        }

        public AutoColorCorrelogram() : this(DEFAULT_NUMBER_COLORS, new int[] { 1, 2, 3, 4 }, new MLuxAutoCorrelogramExtraction())
        {
           
        }



        /**
         * Creates a new AutoCorrelogram with specified algorithm of extraction
         * Uses distance set {1,2,3,4} which is chosen to be compatible with legacy code
         *
         * @param extractionAlgorith the algorithm to extract
         */
        public AutoColorCorrelogram(IAutoCorrelogramFeature extractionAlgorith) 
                    : this(DEFAULT_NUMBER_COLORS, new int[] { 1, 2, 3, 4 }, extractionAlgorith)
        {
            
        }

        /**
         * Creates a new AutoColorCorrelogram using a maximum L_inf pixel distance for analysis and given mode
         */
        public AutoColorCorrelogram(int numBins, int[] distanceSet, IAutoCorrelogramFeature extractionAlgorith)
        {
            this.numBins = numBins;
            this.distanceSet = distanceSet;

            if (extractionAlgorith == null)
            {
                switch (DEFAULT_EXTRACTION_METHOD)
                {
                    case ExtractionMethod.LireAlgorithm:
                        this.extractionAlgorithm = new MLuxAutoCorrelogramExtraction();
                        break;
                    case ExtractionMethod.NaiveHuangAlgorithm:
                        this.extractionAlgorithm = new NaiveAutoCorrelogramExtraction();
                        break;
                }
            }
            else this.extractionAlgorithm = extractionAlgorith;

            if (numBins < 17)
            {
                quantH_f = 4f;
                quantS_f = 2f;
                quantV_f = 2f;
                this.numBins = 16;
            }
            else if (numBins < 33)
            {
                quantH_f = 8f;
                quantS_f = 2f;
                quantV_f = 2f;
                this.numBins = 32;
            }
            else if (numBins < 65)
            {
                quantH_f = 8f;
                quantS_f = 4f;
                quantV_f = 2f;
                this.numBins = 64;
            }
            else if (numBins < 129)
            {
                quantH_f = 8f;
                quantS_f = 4f;
                quantV_f = 4f;
                this.numBins = 128;
            }
            else
            {
                quantH_f = 16f;
                quantS_f = 4f;
                quantV_f = 4f;
                this.numBins = 256;
            }
            quantH = 360f / quantH_f;
            quantS = 256f / quantS_f;
            quantV = 256f / quantV_f;

        }

        private static int[][][] HsvImage(Bitmap img)
        {
            int[][][] pixels = new int[img.Width][][];
            // quantize colors for each pixel (done in HSV color space):
            //raster.getpixel returns red green blue and alpha but we want only rgb colors
            int[] pixel = new int[4];

            for (int x = 0; x < img.Width; x++)
            {
                pixels[x] = new int[img.Height][];
                for (int y = 0; y < img.Height; y++)
                {
                    var color = img.GetPixel(x, y);
                    
                    // converting to HSV:
                    pixels[x][y] = convertRgbToHsv(new int[] { color.R, color.G, color.B });
                }
            }
            return pixels;
        }

        private static int[] convertRgbToHsv(int[] rgb)
        {     
            int[] hsv = new int[3];

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
                float maxMinusMin = (float)(max - min);
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

            return hsv;
        }
        

        public double[] extract(LocalBitmap bi)
        {
            int[][][] hsvImage = HsvImage(bi.Bitmap);
            extract(hsvImage);
            return getDoubleHistogram();
        }

        public void extract(int[][][] img)
        {
            int W = img.Length;
            int H = img[0].Length;
            int[][] quantPixels = new int[W][];

            // quantize colors for each pixel (done in HSV color space):
            for (int x = 0; x < W; x++)
            {
                quantPixels[x] = new int[H];
                for (int y = 0; y < H; y++)
                {
                    quantPixels[x][y] = quantize(img[x][y]);
                }
            }


            this.correlogram = this.extractionAlgorithm.extract(this.numBins, this.distanceSet, quantPixels);
        }

        public double[] getDoubleHistogram()
        {
            return getFloatHistogram().Select(a => (double)a).ToArray();
        }

        private float[] getFloatHistogram()
        {
            float[] result = new float[correlogram.Length * correlogram[0].Length];
            for (int i = 0; i < correlogram.Length; i++)
            {
                Array.Copy(correlogram[i], 0, result, i * correlogram[0].Length, correlogram[0].Length);
            }
            return result;
        }



        /**
         * Quantizes a pixel according to numBins number of bins and a respective algorithm.
         *
         * @param pixel the pixel to quantize.
         * @return the quantized value ...
         */
        private int quantize(int[] pixel)
        {
            return (int)((int)(pixel[0] / quantH) * (quantV_f) * (quantS_f)
                    + (int)(pixel[1] / quantS) * (quantV_f)
                    + (int)(pixel[2] / quantV));
        }


        public string ToString()
        {
            return "Auto Color Correlogram";
        }





    }
}

    
