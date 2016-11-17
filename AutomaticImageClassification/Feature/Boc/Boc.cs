﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using java.awt.image;
using Color = java.awt.Color;

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


        

        public double[] ExtractHistogram(LocalBitmap input)
        {
            var img = new BufferedImage(input.Bitmap);
            //img = ImageProcessor.resizeImage(img, _resize, _resize, false);
            return ExtractHistogram(img);
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            var img = new BufferedImage(input.Bitmap);
            //img = ImageProcessor.resizeImage(img, _resize, _resize, false);
            int[][] domColors = ImageProcessor.getDominantColors(img, _patches, _patches, _cs);
            return domColors.Select(domColor => new double[] { domColor[0], domColor[1], domColor[2] }).ToList();
        }


        public double[] ExtractHistogram(BufferedImage img)
        {
            var vector = new double[_clusterModel.ClusterNum];
            for (var x = 0; x < img.getWidth(); x++)
            {
                for (var y = 0; y < img.getHeight(); y++)
                {
                    var color = new Color(img.getRGB(x, y));
                    int[] cl = ColorConversion.convertFromRGB(_cs, color.getRed(), color.getGreen(), color.getBlue());

                    int indx = _clusterModel.Tree?.SearchTree(new double[] { cl[0], cl[1], cl[2] })
                        ?? DistanceMetrics.ComputeNearestCentroidL2(ref _clusterModel.Means, cl.Select(a => (double)a).ToArray());

                    vector[indx]++;
                }
            }
            return vector;
        }

        public override string ToString()
        {
            //return "Boc_" + (_clusterModel.Tree?.ToString() ?? "L2") + "_" + _clusterModel.ClusterNum + "_" + _cs.toString();
            return "Boc" + "_" + _cs.toString();
        }

    }
}
