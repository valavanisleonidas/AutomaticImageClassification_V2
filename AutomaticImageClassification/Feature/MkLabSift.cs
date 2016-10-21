﻿using AutomaticImageClassification.Cluster.KDTree;
using gr.iti.mklab.visual.extraction;
using java.awt.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Feature
{
    public class MkLabSift : IFeatures
    {
        private AbstractFeatureExtractor _sift;
        private IKdTree _tree;
        private ExtractionMethod _extractionMethod;
        private int _clusterNum;

        public MkLabSift(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }

        public MkLabSift()
        {
            _extractionMethod = ExtractionMethod.RootSift;
            _sift = new RootSIFTExtractor();
        }

        public MkLabSift(ExtractionMethod extractionMethod)
        {
            _extractionMethod = extractionMethod;
            switch (_extractionMethod)
            {
                case ExtractionMethod.Sift:
                    _sift = new SIFTExtractor();
                    break;
                case ExtractionMethod.RootSift:
                    _sift = new RootSIFTExtractor();
                    break;
                default:
                    _sift = new RootSIFTExtractor();
                    break;
            }
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            var bimage = new BufferedImage(new Bitmap(input));
            double[][] siftFeatures = _sift.extractFeatures(bimage);
            return siftFeatures.ToList();
        }

        public double[] ExtractHistogram(string input)
        {
            List<double[]> features = ExtractDescriptors(input);
            double[] imgVocVector = new double[_clusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            foreach (var feature in features)
            {
                int positionofMin = _tree.SearchTree(feature);
                imgVocVector[positionofMin]++;
            }

            return imgVocVector;
        }

        public enum ExtractionMethod
        {
            Sift,
            RootSift
        }

        public override string ToString()
        {
            return _extractionMethod.ToString();
        }

    }
}