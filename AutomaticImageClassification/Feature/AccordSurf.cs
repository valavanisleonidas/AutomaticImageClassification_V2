using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Accord.Imaging;
using AutomaticImageClassification.Cluster.KDTree;

namespace AutomaticImageClassification.Feature
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
                var map = new Bitmap(input);

                return _surf
                        .ProcessImage(map)
                        .ConvertAll(
                                new Converter<SpeededUpRobustFeaturePoint, double[]>(descriptor => descriptor.Descriptor));
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
