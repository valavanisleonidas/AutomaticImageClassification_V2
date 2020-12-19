using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Boc
{
    public class Boc : IFeatures
    {
        private readonly int _patches = 64;
        private readonly ColorConversion.ColorSpace _cs;
        private readonly ClusterModel _clusterModel;

        public bool CanCluster
        {
            get { return true; }
        }

        public Boc(ColorConversion.ColorSpace cs)
        {
            _cs = cs;
        }

        public Boc(ColorConversion.ColorSpace cs, ClusterModel clusterModel)
        {
            _cs = cs;
            _clusterModel = clusterModel;
        }

        public Boc(int patches, ColorConversion.ColorSpace cs)
        {
            _patches = patches;
            _cs = cs;
        }

        #region c# boc implementation


        public double[] ExtractHistogram(LocalBitmap input)
        {
            return ExtractHistogram(input.Bitmap);
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            return ImageProcessing.GetDominantColors(input.Bitmap, _patches, _patches, _cs);
        }

        public double[] ExtractHistogram(Bitmap img)
        {
            
            List<double[]> colors = new List<double[]>();
            var vector = new double[_clusterModel.ClusterNum];

            //get colors of image
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    var color = img.GetPixel(x, y);
                    int[] cl = ColorConversion.ConvertFromRGB(_cs, color.R, color.G, color.B);

                    colors.Add(new double[] { cl[0], cl[1], cl[2] });
                }
            }
            
            //get images
            List<int> indices = _clusterModel.Tree?.SearchTree(colors)
                                ?? DistanceMetrics.ComputeNearestCentroidL2(ref _clusterModel.Means, colors);
            
            //fix histogram
            foreach (var index in indices)
            {
                vector[index]++;
            }

            return vector;
        }

        #endregion

        public override string ToString()
        {
            //return "Boc_" + (_clusterModel.Tree?.ToString() ?? "L2") + "_" + _clusterModel.ClusterNum + "_" + _cs.toString();
            return "Boc" + "_" + _cs.ToString();
        }

    }
}
