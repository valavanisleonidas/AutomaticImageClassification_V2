using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;


namespace AutomaticImageClassification.Feature.Boc
{
    public class Lboc : ILocalFeatures
    {

        private readonly int _patches = 64;
        private readonly ColorConversion.ColorSpace _cs;
        private readonly ClusterModel _lbocClusterModel;
        private readonly ClusterModel _bocClusterModel;

       
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

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            List<Bitmap> blocks = ImageProcessing.SplitImage(input.Bitmap, _patches, _patches);

            var boc = new Boc(_cs, _bocClusterModel);
            return blocks.Select(b => boc.ExtractHistogram(b)).ToList();
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            var vector = new double[_lbocClusterModel.Means.Count];

            List<Bitmap> blocks = ImageProcessing.SplitImage(input.Bitmap, _patches, _patches);
            List<double[]> bocColors = new List<double[]>();

            var boc = new Boc(_cs, _bocClusterModel);
            foreach (var b in blocks)
            {
                bocColors.Add(boc.ExtractHistogram(b));
            }

            //get images
            List<int> indices = _lbocClusterModel.Tree?.SearchTree(bocColors)
                                ?? DistanceMetrics.ComputeNearestCentroidL2(ref _lbocClusterModel.Means, bocColors);

            //fix histogram
            foreach (var index in indices)
            {
                vector[index]++;
            }
 
            return vector;
        }
       
        
        public override string ToString()
        {
            return "LBoc" + (_lbocClusterModel?.Tree?.ToString() ?? "L2") + "_" + _bocClusterModel?.Means.Count
                + "_Palette" + _lbocClusterModel?.Means.Count + "VWords_" + _cs.ToString();
        }

        
    }
}
