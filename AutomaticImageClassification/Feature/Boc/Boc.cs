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

            int[][] domColors =
                ImageProcessing.GetDominantColors(input.Bitmap, _patches, _patches, _cs);

            return domColors.Select(domColor => new double[] { domColor[0], domColor[1], domColor[2] }).ToList();
        }

        public double[] ExtractHistogram(Bitmap img)
        {
            var vector = new double[_clusterModel.ClusterNum];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    var color = img.GetPixel(x, y);
                    int[] cl = Utilities.ColorConversion.ConvertFromRGB(_cs, color.R, color.G, color.B);

                    int indx = _clusterModel.Tree?.SearchTree(new double[] { cl[0], cl[1], cl[2] })
                        ?? DistanceMetrics.ComputeNearestCentroidL2(ref _clusterModel.Means, cl.Select(a => (double)a).ToArray());

                    vector[indx]++;
                }
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
