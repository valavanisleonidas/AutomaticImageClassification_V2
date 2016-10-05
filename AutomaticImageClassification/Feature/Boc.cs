using java.awt.image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.KDTree;
using java.awt;

namespace AutomaticImageClassification.Feature
{
    public class Boc : IFeatures
    {
        private int[][] _palette;
        private int _resize, _patches;
        private ColorConversion.ColorSpace _cs;
        private IKdTree _tree;

        public Boc(int resize, ColorConversion.ColorSpace cs, int[][] palette, IKdTree tree)
        {
            _resize = resize;
            _cs = cs;
            _palette = palette;
            _tree = tree;
        }

        public Boc(int resize, int patches, ColorConversion.ColorSpace cs)
        {
            _patches = patches;
            _resize = resize;
            _cs = cs;
        }


        public double[] ExtractHistogram(string input)
        {
            BufferedImage img = ImageUtility.getImage(input);
            img = ImageProcessor.resizeImage(img, _resize, _resize, false);
            return ExtractHistogram(img);
        }

        public double[] ExtractHistogram(BufferedImage img)
        {
            var vector = new double[_palette.Length];
            for (var x = 0; x < img.getWidth(); x++)
            {
                for (var y = 0; y < img.getHeight(); y++)
                {
                    var color = new Color(img.getRGB(x, y));
                    int[] cl = ColorConversion.convertFromRGB(_cs, color.getRed(), color.getGreen(), color.getBlue());

                    int indx = _tree?.SearchTree(new double[] { cl[0], cl[1], cl[2] }) 
                        ?? ImageProcessor.getClosestColorIndx(_palette, cl);

                    vector[indx]++;
                }
            }
            return vector;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            var colors = new List<double[]>();

            BufferedImage img = ImageUtility.getImage(input);
            img = ImageProcessor.resizeImage(img, _resize, _resize, false);
            int[][] domColors = ImageProcessor.getDominantColors(img, _patches, _patches, _cs);
            foreach (var domColor in domColors)
            {
                colors.Add(new double[] { domColor[0], domColor[1], domColor[2] });
            }
            return colors;
        }





    }
}
