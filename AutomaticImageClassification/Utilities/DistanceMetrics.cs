using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomaticImageClassification.Utilities
{
    public class DistanceMetrics
    {

        public static List<int> ComputeNearestCentroidL2(ref List<double[]> clusters, List<double[]> points)
        {
            List<int> indices = new List<int>();
            foreach(var point in points)
            {
                indices.Add(ComputeNearestCentroidL2(ref clusters, point));
            }

            return indices;
        }

        public static int ComputeNearestCentroidL2(ref List<double[]> clusters, double[] p)
        {
            int index = 0;
            if (clusters.Count <= 0) return index;
            double distance = GetL2Distance(clusters[0], p);
            for (int i = 1; i < clusters.Count; i++)
            {
                double tmpDistance = GetL2Distance(clusters[i], p);
                if (!(tmpDistance < distance)) continue;
                distance = tmpDistance;
                index = i;
            }
            return index;
        }


        public static List<int> ComputeNearestCentroidL2NotSquare(ref List<double[]> clusters, List<double[]> points)
        {
            List<int> indices = new List<int>();
            foreach (var point in points)
            {
                indices.Add(ComputeNearestCentroidL2NotSquare(ref clusters, point));
            }

            return indices;
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
            double distance = GetL2Distance(clusters[0], p);
            for (int i = 1; i < clusters.Count; i++)
            {
                double tmpDistance = GetL2Distance(clusters[i], p);
                if (!(tmpDistance < distance)) continue;
                distance = tmpDistance;
                index = i;
            }
            return index;
        }
        
        public static double GetL2Distance(double[] point1, double[] point2)
        {
            double dist = 0;
            for (int i = 0; i < point1.Length; i++)
            {
                dist += (point1[i] - point2[i]) * (point1[i] - point2[i]);
            }
            return Math.Sqrt(dist);
        }
        public static double GetL2Distance(float[] point1, float[] point2)
        {
            double dist = 0;
            for (int i = 0; i < point1.Length; i++)
            {
                dist += (point1[i] - point2[i]) * (point1[i] - point2[i]);
            }
            return Math.Sqrt(dist);
        }
        public static double GetL2Distance(int[] point1, int[] point2)
        {
            double dist = 0;
            for (int i = 0; i < point1.Length; i++)
            {
                dist += (point1[i] - point2[i]) * (point1[i] - point2[i]);
            }
            return Math.Sqrt(dist);
        }


    }
}
