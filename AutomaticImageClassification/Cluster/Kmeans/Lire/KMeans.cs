/*
 * This file is part of the LIRE project: http://lire-project.net
 * LIRE is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * LIRE is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with LIRE; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 * We kindly ask you to refer the any or one of the following publications in
 * any publication mentioning or employing Lire:
 *
 * Lux Mathias, Savvas A. Chatzichristofis. Lire: Lucene Image Retrieval –
 * An Extensible Java CBIR Library. In proceedings of the 16th ACM International
 * Conference on Multimedia, pp. 1085-1088, Vancouver, Canada, 2008
 * URL: http://doi.acm.org/10.1145/1459359.1459577
 *
 * Lux Mathias. Content Based Image Retrieval with LIRE. In proceedings of the
 * 19th ACM International Conference on Multimedia, pp. 735-738, Scottsdale,
 * Arizona, USA, 2011
 * URL: http://dl.acm.org/citation.cfm?id=2072432
 *
 * Mathias Lux, Oge Marques. Visual Information Retrieval using Java and LIRE
 * Morgan & Claypool, 2013
 * URL: http://www.morganclaypool.com/doi/abs/10.2200/S00468ED1V01Y201301ICR025
 *
 * Copyright statement:
 * --------------------
 * (c) 2002-2013 by Mathias Lux (mathias@juggle.at)
 *     http://www.semanticmetadata.net/lire, http://www.lire-project.net
 */



using AutomaticImageClassification.Utilities;
using System;
using System.Collections.Generic;
/**
* Simple, re-usable and straight k-means implementation based on double[] feature vectors and L2 distance.
* User: mlux
* Date: 20.09.13
* Time: 10:56
*/
public class KMeans {
    private List<double[]> features;
    private Cluster[] clusters;
    Random rnd = new Random();

    public KMeans(ref List<double[]> featureList, int numberOfClusters) {
        features = new List<double[]>(featureList);
        clusters = new Cluster[numberOfClusters];
        
        HashSet<double[]> means = new HashSet<double[]>();
        while (means.Count < Math.Min(numberOfClusters, featureList.Count / 2)) {
            double[] e = features[(int) Math.Floor((double)rnd.Next(1, features.Count))];
            double[] tmp = new double[e.Length];
            Array.Copy(e, 0, tmp, 0, e.Length);
            means.Add(tmp);
        }
        // init cluster centers.
        

        int counter = 0;
        foreach (var mean in means)
        {
            clusters[counter] = new Cluster(mean);
            counter++;
        }
    }

    public double step() {
        // init clusters:
        for (int i = 0; i < clusters.Length; i++) {
            clusters[i].clearMembers();
            //assert (clusters[i].members.Count == 0);
        }
        // assign to new clusters:
        for (int id = 0; id < features.Count; id++) {
            double tmpDistance = double.MaxValue;
            int currentCluster = -1;
            for (int i = 0; i < clusters.Length; i++) {
                double distance = clusters[i].getDistance(features[id]);
                //assert (distance >= 0);
                if (distance < tmpDistance) {
                    tmpDistance = distance;
                    currentCluster = i;
                }
            }
            clusters[currentCluster].addMember(id);
        }
        // recompute means:
        for (int i = 0; i < clusters.Length; i++) {
            clusters[i].recomputeMeans(features);
//            System.out.println(Arrays.toString(clusters[i].center));
        }
        // calculate stress
        double stress = 0d;
        double num = 0;
        for (int i = 0; i < clusters.Length; i++) {
            stress += clusters[i].calculateStress(features);
            num += clusters[i].members.Count;
        }
        return stress;
    }

    public List<double[]> getMeans() {
        List<double[]> r = new List<double[]>(clusters.Length);
        for (int i = 0; i < clusters.Length; i++) {
            r.Add(clusters[i].center);
        }
        return r;
    }

    /**
     * Cluster implementation used in this k-means implementation.
     */
    public class Cluster {
        public double[] center;
        public HashSet<int> members;

        public Cluster(double[] center) {
            this.center = center;
            members = new HashSet<int>();
        }

        public double getDistance(double[] feature) {
            return DistanceMetrics.GetL2Distance(center, feature);
        }

        public void clearMembers() {
            members.Clear();
        }

        public void addMember(int id) {
            members.Add(id);
        }

        public void recomputeMeans(List<double[]> features) {
            if (members.Count > 0) {
                int counter = 0;
                Array.Clear(center, 0, center.Length);
                
                foreach (var member in members)
                {
                    double[] feature = features[counter];
                    counter++;
                    for (int i = 0; i < feature.Length; i++) {
//                        assert (feature[i] < 256);
                        center[i] += feature[i];
                    }
                }
                for (int i = 0; i < center.Length; i++) {
                    center[i] = center[i] / ((double) members.Count);
                }
            }
        }

        public double calculateStress(List<double[]> features) {
            double result = 0d;
            int counter = 0;
            foreach (var member in members)
            {
                
                double[] feature = features[counter];
                result += DistanceMetrics.GetL2Distance(center, feature);
                counter++;
            }
            return result;
        }
    }


}