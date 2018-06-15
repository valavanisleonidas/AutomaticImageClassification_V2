

//deprecated not used surf implementation. just keep it to have it if needed

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using Accord.Imaging;
//using AutomaticImageClassification.Cluster.ClusterModels;
//using AutomaticImageClassification.KDTree;
//using AutomaticImageClassification.Utilities;

//namespace AutomaticImageClassification.Feature.Bovw
//{
//    public class AccordSurf : IFeatures
//    {
//        // Create a new SURF Features Detector using default parameters
//        private readonly SpeededUpRobustFeaturesDetector _surf = new SpeededUpRobustFeaturesDetector();
//        private readonly ClusterModel _clusterModel;

//        public bool CanCluster
//        {
//            get { return true; }
//        }

//        public AccordSurf() { }

//        public AccordSurf(ClusterModel clusterModel)
//        {
//            _clusterModel = clusterModel;
//        }

//        public double[] ExtractHistogram(LocalBitmap input)
//        {
//            List<double[]> features = ExtractDescriptors(input);
//            var imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

//            //for each centroid find min position in tree and increase corresponding index
//            List<int> indexes = _clusterModel.Tree.SearchTree(features);
//            foreach (var index in indexes)
//            {
//                imgVocVector[index]++;
//            }
//            return imgVocVector;
//        }

//        public List<double[]> ExtractDescriptors(LocalBitmap input)
//        {
//            try
//            {
//                return _surf.ProcessImage(input.Bitmap).ConvertAll(descriptor => descriptor.Descriptor);
//            }
//            catch (Exception e)
//            {
//                throw e;
//            }
//        }

//        public double[] ExtractHistogram(string input)
//        {
//            List<double[]> features = ExtractDescriptors(input);
//            var imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

//            //for each centroid find min position in tree and increase corresponding index
//            List<int> indexes = _clusterModel.Tree.SearchTree(features);
//            foreach (var index in indexes)
//            {
//                imgVocVector[index]++;
//            }
//            return imgVocVector;
//        }

//        public List<double[]> ExtractDescriptors(string input)
//        {
//            try
//            {
//                return _surf.ProcessImage(new Bitmap(input)).ConvertAll(descriptor => descriptor.Descriptor);
//            }
//            catch (Exception e)
//            {
//                throw e;
//            }
//        }

//        public override string ToString()
//        {
//            return "AccordSurf";
//        }
//    }
//}
