using System.Collections.Generic;
using System.Linq;

namespace AutomaticImageClassification.KDTree
{
    public class AccordKdTree : IKdTree
    {
        private static Accord.Collections.KDTree<double[]> _kdtree;
        private static List<double[]> _centers;
        
        public void CreateTree(List<double[]> centers)
        {
            // To create a tree from a set of points, we use
            _kdtree = Accord.Collections.KDTree.FromData<double[]>(centers.ToArray());
            _centers = centers;
        }

        //returns nearest object of array centroid in tree
        public int SearchTree(double[] centroid)
        {
            //find nearest object to query (centroid)
            var a = _kdtree.Nearest(centroid, 1);
            //it returns position with is the center and then we find by index to find the actual index
            return _centers.FindIndex(da => da == a.Nearest.Position);
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
