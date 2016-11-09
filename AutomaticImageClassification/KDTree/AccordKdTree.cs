using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.KDTree;

namespace AutomaticImageClassification.KDTree
{
    public class AccordKdTree : IKdTree
    {
        private static Accord.Collections.KDTree<double[]> _kdtree;
        private List<double[]> _centers;

        public AccordKdTree(List<double[]> centers)
        {
            _centers = centers;
        }

        public void CreateTree(List<double[]> centers)
        {
            // To create a tree from a set of points, we use
            _kdtree = Accord.Collections.KDTree.FromData<double[]>(centers.ToArray());
        }

        //returns nearest object of array centroid in tree
        public int SearchTree(double[] centroid)
        {
            //find nearest object to query (centroid)
            var a = _kdtree.Nearest(centroid, 1);
            return _centers.FindIndex(da => da[0] == a.Nearest.Position[0] && da[1] == a.Nearest.Position[1] && da[2] == a.Nearest.Position[2]);
        }

        public List<int> SearchTree(List<double[]> centers)
        {
            return centers.Select(center => SearchTree(center)).ToList();
        }

        public override string ToString()
        {
            return "AccordKdTree";
        }

    }
}
