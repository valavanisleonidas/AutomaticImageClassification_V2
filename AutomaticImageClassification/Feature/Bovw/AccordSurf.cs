using System;
using System.Collections.Generic;
using System.Drawing;
using Accord.Imaging;
using AutomaticImageClassification.Cluster.KDTree;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class AccordSurf : IFeatures
    {
        // Create a new SURF Features Detector using default parameters
        private SpeededUpRobustFeaturesDetector _surf = new SpeededUpRobustFeaturesDetector();
        private IKdTree _tree;
        private int _clusterNum;

        public AccordSurf(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }
        public AccordSurf() { }

        public double[] ExtractHistogram(string input)
        {
            List<double[]> features = ExtractDescriptors(input);
            var imgVocVector = new double[_clusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            foreach (var feature in features)
            {
                var positionofMin = _tree.SearchTree(feature);
                imgVocVector[positionofMin]++;
            }
            return imgVocVector;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            try
            {
                return _surf.ProcessImage(new Bitmap(input)).ConvertAll( descriptor => descriptor.Descriptor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "AccordSurf";
        }

    }
}
