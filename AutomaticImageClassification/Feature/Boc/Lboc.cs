using System.Collections.Generic;
using System.Drawing;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using java.awt.image;

namespace AutomaticImageClassification.Feature.Boc
{
    public class Lboc : IFeatures
    {

        private readonly int _resize = 256;
        private readonly int _patches = 64;
        private readonly ColorConversion.ColorSpace _cs;
        private readonly ClusterModel _lbocClusterModel;
        private readonly ClusterModel _bocClusterModel;

        public Lboc(int resize, int patches, ColorConversion.ColorSpace cs)
        {
            _resize = resize;
            _cs = cs;
            _patches = patches;

        }

        public Lboc(ColorConversion.ColorSpace cs, ClusterModel bocClusterModel)
        {
            _cs = cs;
            _bocClusterModel = bocClusterModel;
        }

        public Lboc(ColorConversion.ColorSpace cs, ClusterModel bocClusterModel, ClusterModel lbocClusterModel)
        {
            _cs = cs;
            _bocClusterModel = bocClusterModel;
            _lbocClusterModel = lbocClusterModel;
        }




        public double[] ExtractHistogram(string input)
        {
            var bimg = new BufferedImage(new Bitmap(input));
            return ExtractHistogram(bimg);
        }

        public double[] ExtractHistogram(BufferedImage input)
        {
            input = ImageProcessor.resizeImage(input, _resize, _resize, false);
            var vector = new double[_lbocClusterModel.Means.Count];
            BufferedImage[] blocks = ImageProcessor.splitImage(input, _patches, _patches);

            var boc = new Boc(_resize, _cs, _bocClusterModel);
            foreach (var b in blocks)
            {
                double[] _boc = boc.ExtractHistogram(b);

                int indx = _lbocClusterModel.Tree?.SearchTree(_boc)
                    ?? DistanceMetrics.ComputeNearestCentroidL2(ref _lbocClusterModel.Means, _boc);

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

            var boc = new Boc(_resize, _cs, _bocClusterModel);
            foreach (var b in blocks)
            {
                colors.Add(boc.ExtractHistogram(b));
            }
            return colors;
        }

        public override string ToString()
        {
            return "LBoc" + (_lbocClusterModel.Tree?.ToString() ?? "L2") + "_" + _bocClusterModel.Means.Count
                + "_Palette" + _lbocClusterModel.Means.Count + "VWords_" + _cs.toString();
        }

    }
}
