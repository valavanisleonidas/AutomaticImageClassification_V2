using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using OpenSURFcs;
using AutomaticImageClassification.Cluster.KDTree;

namespace AutomaticImageClassification.Feature
{
    public class JOpenSurf : IFeatures
    {

        private IKdTree _tree;
        private int _clusterNum;

        public JOpenSurf(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }
        public JOpenSurf() { }

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
            try
            {
                // Load an Image
                Bitmap img = new Bitmap(input);

                // Create Integral Image
                IntegralImage iimg = IntegralImage.FromImage(img);

                // Extract the interest points
                List<IPoint> ipts = FastHessian.getIpoints(0.0002f, 5, 2, iimg);

                // Describe the interest points
                SurfDescriptor.DecribeInterestPoints(ipts, false, false, iimg);

               // List<double[]> aaa = ipts.Select(a => a.descriptor.Select( b => Convert.ToDouble(b) ).ToArray() ).ToList();

                return ipts.ConvertAll(
                            new Converter<IPoint, double[]>(
                                des => Array.ConvertAll(des.descriptor, x => (double)x)));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
