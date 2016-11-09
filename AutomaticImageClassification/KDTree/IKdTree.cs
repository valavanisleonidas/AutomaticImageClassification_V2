using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Cluster.KDTree
{
    public interface IKdTree
    {
        void CreateTree(List<double[]> centers);
        int SearchTree(double[] centroid);
        List<int> SearchTree(List<double[]> centers);
    }
}
