using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using OpenCvSharp.CPlusPlus;
using AutomaticImageClassification.Cluster.KDTree;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class OpenCvSurf : IFeatures
    {
        private SURF _surf = new SURF();
        private IKdTree _tree;
        private int _clusterNum;

        public OpenCvSurf(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }
        public OpenCvSurf() { }

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
            return "OpenCvSurf";
        }

    }
}
