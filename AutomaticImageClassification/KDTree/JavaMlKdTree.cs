using System.Collections.Generic;
using AutomaticImageClassification.Cluster.KDTree;
using ikvm.extensions;
using java.lang;

namespace AutomaticImageClassification.KDTree
{
    
    public class JavaMlKdTree : IKdTree
    {

        private net.sf.javaml.core.kdtree.KDTree _tree;
        
        public void CreateTree(List<double[]> centers)
        {
            _tree = new net.sf.javaml.core.kdtree.KDTree(centers[0].Length);
            for (var i = 0; i < centers.Count; i++)
            {
                _tree.insert(centers[i], i);
            }
        }

        public int SearchTree(double[] centroid)
        {
            var nearestObject = _tree.nearest(centroid);
            return Integer.parseInt(nearestObject.toString());
        }

        public override string ToString()
        {
            return "JavaMlKdTree";
        }

    }
}
