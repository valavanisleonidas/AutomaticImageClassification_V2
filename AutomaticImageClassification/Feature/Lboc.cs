using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.KDTree;
using java.awt.image;

namespace AutomaticImageClassification.Feature
{
    public class Lboc : IFeatures
    {

        private int[][] palette;
        private int resize, patches;
        private ColorConversion.ColorSpace cs;
        private List<double[]> dictionary;
        private IKdTree Boctree;
        private IKdTree LBoctree;

        public Lboc(int resize, int patches, List<double[]> dictionary, ColorConversion.ColorSpace cs, int[][] palette, IKdTree boctree, IKdTree LBoctree)
        {
            this.resize = resize;
            this.cs = cs;
            this.palette = palette;
            this.patches = patches;
            this.dictionary = dictionary;
            this.Boctree = boctree;
            this.LBoctree = LBoctree;

        }

        public Lboc(int resize, int patches, ColorConversion.ColorSpace cs, int[][] palette, IKdTree Boctree)
        {
            this.resize = resize;
            this.cs = cs;
            this.palette = palette;
            this.patches = patches;
            this.Boctree = Boctree;
        }

        public double[] ExtractHistogram(string input)
        {
            BufferedImage bimg = ImageUtility.getImage(input);
            return ExtractHistogram(bimg);
        }

        public double[] ExtractHistogram(BufferedImage input)
        {
            input = ImageProcessor.resizeImage(input, resize, resize, false);
            var vector = new double[dictionary.Count];
            BufferedImage[] blocks = ImageProcessor.splitImage(input, patches, patches);

            var boc = new Boc(resize, cs, palette, Boctree);
            foreach (var b in blocks)
            {
                double[] _boc = boc.ExtractHistogram(b);

                int indx = LBoctree?.SearchTree(_boc) 
                    ?? ClusterIndexOf(dictionary, _boc);

                vector[indx]++;
            }
            return vector;

        }

        public List<double[]> ExtractDescriptors(string input)
        {
            List<double[]> colors = new List<double[]>();
            BufferedImage img = ImageUtility.getImage(input);
            img = ImageProcessor.resizeImage(img, resize, resize, false);
            BufferedImage[] blocks = ImageProcessor.splitImage(img, patches, patches);

            var boc = new Boc(resize,cs,palette,Boctree);
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
