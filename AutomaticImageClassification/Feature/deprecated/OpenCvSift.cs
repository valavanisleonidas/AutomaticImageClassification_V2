//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutomaticImageClassification.Cluster.ClusterModels;
//using AutomaticImageClassification.KDTree;
//using AutomaticImageClassification.Utilities;
//using OpenCvSharp.CPlusPlus;

//namespace AutomaticImageClassification.Feature.Local
//{
//    //deprecated
//    public class OpenCvSift : IFeatures
//    {
//        private readonly SIFT _sift = new SIFT();
//        private readonly ClusterModel _clusterModel;

//        public bool CanCluster
//        {
//            get { return true; }
//        }

//        public OpenCvSift(ClusterModel clusterModel)
//        {
//            _clusterModel = clusterModel;
//        }

//        public OpenCvSift() { }

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

//            Mat src = new Mat(input.Path);

//            KeyPoint[] keuPoints;
//            MatOfFloat descriptors = new MatOfFloat();

//            _sift.Run(src, null, out keuPoints, descriptors);
//            float[,] arr = descriptors.ToRectangularArray();
//            //convert to list<double[]>
//            return Arrays.ToJaggedArray(ref arr)
//                .ToList()
//                .ConvertAll(
//                        des => Array.ConvertAll(des, x => (double)x));
//        }

//        public override string ToString()
//        {
//            return "OpenCvSift";
//        }

//    }
//}
