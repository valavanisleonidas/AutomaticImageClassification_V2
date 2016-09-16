using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;

namespace AutomaticImageClassification.Cluster.KDTree
{
    public class AccordKdTree : IKdTree
    {
        private static Accord.Collections.KDTree _kdtree;

        public void CreateTree(List<double[]> centers)
        {
            int dimensions = centers[0].Length;
            _kdtree = new Accord.Collections.KDTree(dimensions);
            foreach (double[] center in centers)
            {
                _kdtree.Add(center);
            }
        }

        //returns nearest object of array centroid in tree
        public int SearchTree(double[] centroid)
        {
            return _kdtree.Nearest(centroid).Axis;
        }


    }
}
