using System.Collections.Generic;
using System.Linq;

using AutomaticImageClassification.Utilities;
using Accord.MachineLearning;
using AutomaticImageClassification.Cluster.ClusterModels;

namespace AutomaticImageClassification.Cluster.Kmeans
{
    public class AccordKmeans : ICluster
    {
        private int _numberOfFeatures = int.MaxValue;

        public AccordKmeans() { }

        public AccordKmeans(int numberOfFeatures)
        {
            _numberOfFeatures = numberOfFeatures;
        }

        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            // Create a new K-Means algorithm with 3 clusters 
            KMeans kmeans = new KMeans(clustersNum);
            if (descriptorFeatures.Count > _numberOfFeatures)
            {
                //TODO check results because vl_colSubset was removed
                Arrays.GetSubsetOfFeatures(ref descriptorFeatures, _numberOfFeatures);
            }
            // Compute the algorithm, retrieving an integer array
            //  containing the labels for each of the observations
            kmeans.Learn(descriptorFeatures.ToArray());
            
            return new KmeansModel(kmeans.Clusters.Centroids.ToList());
        }

        public static void ClusterpixelImages()
        {
            //        int k = 5;

            //        // Load a test image (shown below)
            //        Bitmap image = ...

            //        // Create converters
            //        ImageToArray imageToArray = new ImageToArray(min: -1, max: +1);
            //        ArrayToImage arrayToImage = new ArrayToImage(image.Width, image.Height, min: -1, max: +1);

            //        // Transform the image into an array of pixel values
            //        double[][] pixels; imageToArray.Convert(image, out pixels);


            //// Create a K-Means algorithm using given k and a
            ////  square Euclidean distance as distance metric.
            //KMeans kmeans = new KMeans(k, Distance.SquareEuclidean);

            //        // Compute the K-Means algorithm until the difference in
            //        //  cluster centroids between two iterations is below 0.05
            //        int[] idx = kmeans.Compute(pixels, 0.05);


            //        // Replace every pixel with its corresponding centroid
            //        pixels.ApplyInPlace((x, i) => kmeans.Clusters.Centroids[idx[i]]);

            //// Retrieve the resulting image in a picture box
            //Bitmap result; arrayToImage.Convert(pixels, out result);
        }

        public override string ToString()
        {
            return "AccordKmeans";
        }

    }
}
