using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ikvm.extensions;
using java.lang;
using net.sf.javaml.core.kdtree;

namespace AutomaticImageClassification.Utilities
{
    //Comment
    //Exists in boclibrary.dll
    //But wanted java.util.List and thats why i created this CS project
    public class KDTreeImplementation
    {
        //create kdTree with given centers
        public static KDTree createTree(List<double[]> Centers)
        {
            var tree = new KDTree(Centers[0].Length);
            for (int i = 0; i < Centers.Count; i++)
                tree.insert(Centers[i], i);
            return tree;

        }

        //returns nearest object of array centroid in tree
        public static int SearchTree(double[] centroid, KDTree tree)
        {
            var nearestObject = tree.nearest(centroid);
            return Integer.parseInt(nearestObject.toString());
        }


    }
}
