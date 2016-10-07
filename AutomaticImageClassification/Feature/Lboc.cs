using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.KDTree;
using AutomaticImageClassification.Utilities;
using java.awt.image;

namespace AutomaticImageClassification.Feature
{
    public class Lboc : IFeatures
    {

        private int[][] _palette;
        private int _resize, _patches;
        private ColorConversion.ColorSpace _cs;
        private List<double[]> _dictionary;
        private IKdTree _boctree;
        private IKdTree _lBoctree;

        public Lboc(int resize, int patches, List<double[]> dictionary, ColorConversion.ColorSpace cs, int[][] palette, IKdTree boctree, IKdTree lBoctree)
        {
            _resize = resize;
            _cs = cs;
            _palette = palette;
            _patches = patches;
            _dictionary = dictionary;
            _boctree = boctree;
            _lBoctree = lBoctree;
        }

        public Lboc(int resize, int patches, ColorConversion.ColorSpace cs, int[][] palette, IKdTree boctree)
        {
            _resize = resize;
            _cs = cs;
            _palette = palette;
            _patches = patches;
            _boctree = boctree;
        }

        public double[] ExtractHistogram(string input)
        {
            var bimg = new BufferedImage(new Bitmap(input));
            return ExtractHistogram(bimg);
        }

        public double[] ExtractHistogram(BufferedImage input)
        {
            input = ImageProcessor.resizeImage(input, _resize, _resize, false);
            var vector = new double[_dictionary.Count];
            BufferedImage[] blocks = ImageProcessor.splitImage(input, _patches, _patches);

            var boc = new Boc(_resize, _cs, _palette, _boctree );
            foreach (var b in blocks)
            {
                double[] _boc = boc.ExtractHistogram(b);

                int indx = _lBoctree?.SearchTree(_boc) 
                    ?? ClusterIndexOf(_dictionary, _boc);

                vector[indx]++;
            }
            return vector;

        }

        public List<double[]> ExtractDescriptors(string input)
        {
            List<double[]> colors = new List<double[]>();
            var img = new BufferedImage(new Bitmap(input));
            img = ImageProcessor.resizeImage(img, _resize, _resize, false);
            BufferedImage[] blocks = ImageProcessor.splitImage(img, _patches, _patches);

            var boc = new Boc(_resize,_cs,_palette,_boctree);
            foreach (var b in blocks)
            {
                colors.Add(boc.ExtractHistogram(b));
            }
           return colors;
        }

        public static int ClusterIndexOf(List<double[]> clusters, double[] p)
        {
            int ret = 0;
            if (clusters.Count > 0)
            {
                double distance = DistanceFunctions.getL2Distance(clusters[0], p);
                double tmp;
                for (int i = 1; i < clusters.Count; i++)
                {
                    tmp = DistanceFunctions.getL2Distance(clusters[i], p);
                    if (tmp < distance)
                    {
                        distance = tmp;
                        ret = i;
                    }
                }
            }
            return ret;
        }


    }
}
