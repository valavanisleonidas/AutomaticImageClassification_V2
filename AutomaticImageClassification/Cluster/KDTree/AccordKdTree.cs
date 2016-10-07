using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Collections;
using Accord.MachineLearning;

namespace AutomaticImageClassification.Cluster.KDTree
{
    public class AccordKdTree : IKdTree
    {
        private static Accord.Collections.KDTree<double[]> _kdtree;
        private List<double[]> _features;

        public AccordKdTree(List<double[]> features)
        {
            _features = features;
        }

        public void CreateTree(List<double[]> centers)
        {
            // To create a tree from a set of points, we use
            _kdtree = Accord.Collections.KDTree.FromData<double[]>(centers.ToArray());
        }

        //returns nearest object of array centroid in tree
        public int SearchTree(double[] centroid)
        {
            var a = _kdtree.Nearest(centroid, 1);
            return _features.FindIndex(da => da[0] == a.Nearest.Position[0] && da[1] == a.Nearest.Position[1] && da[2] == a.Nearest.Position[2]);

        }


    }
}
