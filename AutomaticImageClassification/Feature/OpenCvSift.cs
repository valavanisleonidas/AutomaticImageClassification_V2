using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using OpenCvSharp.CPlusPlus;
using AutomaticImageClassification.Cluster.KDTree;

namespace AutomaticImageClassification.Feature
{
    public class OpenCvSift : IFeatures
    {
        private SIFT _sift = new SIFT();
        private IKdTree _tree;
        private int _clusterNum;

        public OpenCvSift(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }
        public OpenCvSift() { }

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

        public List<double[]> ExtractDescriptors(string input)
        {
            Mat src = new Mat(input);
            KeyPoint[] keuPoints;
            MatOfFloat descriptors = new MatOfFloat();

            _sift.Run(src, null, out keuPoints, descriptors);
            float[,] arr = descriptors.ToRectangularArray();
            //convert to list<double[]>
            return Arrays.ToJaggedArray(ref arr)
                .ToList()
                .ConvertAll(
                        des => Array.ConvertAll(des, x => (double)x));
        }



    }
}
