using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class DistanceMetrics
    {

        public static int ComputeNearestCentroidL2(List<double[]> clusters, double[] p)
        {
            int index = 0;
            if (clusters.Count <= 0) return index;
            double distance = DistanceFunctions.getL2Distance(clusters[0], p);
            for (int i = 1; i < clusters.Count; i++)
            {
                double tmpDistance = DistanceFunctions.getL2Distance(clusters[i], p);
                if (!(tmpDistance < distance)) continue;
                distance = tmpDistance;
                index = i;
            }
            return index;
        }

        public static int ComputeNearestCentroidL2(int[][] clusters, int[] p)
        {
            return ComputeNearestCentroidL2(clusters.ToList(),p);
        }

        public static int ComputeNearestCentroidL2(List<int[]> clusters, int[] p)
        {
            int index = 0;
            if (clusters.Count <= 0) return index;
            double distance = DistanceFunctions.getL2Distance(clusters[0], p);
            for (int i = 1; i < clusters.Count; i++)
            {
                double tmpDistance = DistanceFunctions.getL2Distance(clusters[i], p);
                if (!(tmpDistance < distance)) continue;
                distance = tmpDistance;
                index = i;
            }
            return index;
        }
        
    }
}
