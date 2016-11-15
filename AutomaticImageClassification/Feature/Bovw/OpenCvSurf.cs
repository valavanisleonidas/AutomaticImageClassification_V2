using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using OpenCvSharp.CPlusPlus;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class OpenCvSurf : IFeatures
    {
        private readonly SURF _surf = new SURF();
        private readonly ClusterModel _clusterModel;

        public OpenCvSurf(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }

        public OpenCvSurf() { }

        public double[] ExtractHistogram(string input)
        {
            List<double[]> features = ExtractDescriptors(input);
            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            List<int> indexes = _clusterModel.Tree.SearchTree(features);
            foreach (var index in indexes)
            {
                imgVocVector[index]++;
            }

            return imgVocVector;
        }


        public List<double[]> ExtractDescriptors(string input)
        {

            Mat src1 = new Mat(input);
            KeyPoint[] keypoints1;
            MatOfFloat descriptors1 = new MatOfFloat();

            _surf.Run(src1, null, out keypoints1, descriptors1);

            float[,] arr = descriptors1.ToRectangularArray();
            //convert to list<double[]>
            return Arrays.ToJaggedArray(ref arr)
                    .ToList()
                    .ConvertAll(
                            des => Array.ConvertAll(des, x => (double)x));
        }

        public override string ToString()
        {
            return "OpenCvSurf_" + _clusterModel.Tree + "_" + _clusterModel.ClusterNum;
        }

    }
}
