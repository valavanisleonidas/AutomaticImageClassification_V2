﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.KDTree;
using AutomaticImageClassification.Utilities;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class VlFeatDenseSift : IFeatures
    {
        private static int _scale;
        private static bool _rootSift;
        private static bool _normalizeSift;
        private IKdTree _tree;
        private int _clusterNum;

        public VlFeatDenseSift()
        {
            _scale = 4;
            _rootSift = false;
            _normalizeSift = true;
        }

        public VlFeatDenseSift(int scale,bool isRootSift, bool isNormalizedSift)
        {
            _scale = scale;
            _rootSift = isRootSift;
            _normalizeSift = isNormalizedSift;
        }


        public VlFeatDenseSift(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }

        public double[] ExtractHistogram(string input)
        {
            List<double[]> features = ExtractDescriptors(input);
            double[] imgVocVector = new double[_clusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            List<int> indexes = _tree.SearchTree(features);
            foreach (var index in indexes)
            {
                imgVocVector[index]++;
            }
            return imgVocVector;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            try
            {
                var phow = new MatlabAPI.DenseSift();

                //return frames, descriptors( features ), contrast
                MWArray[] result = phow.GetDenseSIFT(3, new MWCharArray(input), 
                    new MWLogicalArray(_rootSift),
                    new MWLogicalArray(_normalizeSift), 
                    _scale);
                var features = (double[,])result[1].ToArray();

                phow.Dispose();

                return Arrays.ToJaggedArray(ref features).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "DenseSift" + (_rootSift ? "_root" : "") + (_normalizeSift ? "_normalized" : "");
        }

    }
}
