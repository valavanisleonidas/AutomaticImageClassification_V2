using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class DistanceMetrics
    {

        public static int ComputeNearestCentroidL2(ref List<double[]> clusters, double[] p)
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

        public static int ComputeNearestCentroidL2NotSquare(ref List<double[]> clusters, double[] p)
        {
            int centroidIndex = -1;
            double minDistance = double.MaxValue;
            for (int i = 0; i < clusters.Count; i++)
            {
                double distance = 0;
                for (int j = 0; j < clusters[i].Length; j++)
                {
                    distance += (clusters[i][j] - p[j]) * (clusters[i][j] - p[j]);
                    // when distance becomes greater than minDistance
                    // break the inner loop and check the next centroid!!!
                    if (distance >= minDistance)
                    {
                        break;
                    }
                }
                if (!(distance < minDistance)) continue;

                minDistance = distance;
                centroidIndex = i;
            }
            return centroidIndex;
        }


        public static int ComputeNearestCentroidL2(int[][] clusters, int[] p)
        {
            return ComputeNearestCentroidL2(clusters.ToList(), p);
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
