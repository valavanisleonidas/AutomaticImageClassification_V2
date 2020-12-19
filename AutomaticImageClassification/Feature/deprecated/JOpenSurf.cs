
//deprecated not used surf implementation. just keep it to have it if needed

//using System;
//using System.Collections.Generic;
//using AutomaticImageClassification.Cluster.ClusterModels;
//using AutomaticImageClassification.Utilities;
//using OpenSURFcs;

//namespace AutomaticImageClassification.Feature.Local
//{
//    public class JOpenSurf : IFeatures
//    {

//        private readonly ClusterModel _clusterModel;

//        public bool CanCluster
//        {
//            get { return true; }
//        }

//        public JOpenSurf() { }

//        public JOpenSurf(ClusterModel clusterModel)
//        {
//            _clusterModel = clusterModel;
//        }

//        public double[] ExtractHistogram(LocalBitmap input)
//        {
//            List<double[]> features = ExtractDescriptors(input);
//            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

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
//                // Create Integral Image
//                IntegralImage iimg = IntegralImage.FromImage(input.Bitmap);

//                // Extract the interest points
//                List<IPoint> ipts = FastHessian.getIpoints(0.0002f, 5, 2, iimg);

//                // Describe the interest points
//                SurfDescriptor.DecribeInterestPoints(ipts, false, false, iimg);

//               // List<double[]> aaa = ipts.Select(a => a.descriptor.Select( b => Convert.ToDouble(b) ).ToArray() ).ToList();

//                return ipts.ConvertAll(des => Array.ConvertAll(des.descriptor, x => (double)x));
//            }
//            catch (Exception e)
//            {
//                throw e;
//            }
//        }

//        public override string ToString()
//        {
//            return "JOpenSurf";
//        }

//    }
//}
