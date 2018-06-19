using System.Collections.Generic;

namespace AutomaticImageClassification.KDTree
{
    public interface IKdTree
    {
        void CreateTree(List<double[]> centers);
        int SearchTree(double[] centroid);
        List<int> SearchTree(List<double[]> centers);
    }

    public enum KdTreeMethod
    {
        AccordKdTree,
        VlFeatKdTree
    }
}
