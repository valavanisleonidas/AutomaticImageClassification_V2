using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.EM;
using AutomaticImageClassification.Cluster.GaussianMixtureModel;
using AutomaticImageClassification.Cluster.Kmeans;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Managers
{
    public class ClusterManager
    {


        private static void GetClusterType(ref ClusterParameters clusterParameters)
        {
            switch (clusterParameters.ClusterMethod)
            {
                case ClusterMethod.VlFeatEm:
                    clusterParameters.Cluster = new VlFeatEm(clusterParameters.IsRandomInit);
                    break;
                case ClusterMethod.VlFeatGmm:
                    clusterParameters.Cluster = new VlFeatGmm();
                    break;
                case ClusterMethod.VlFeatKmeans:
                    clusterParameters.Cluster = new VlFeatKmeans();
                    break;
                case ClusterMethod.LireKmeans:
                    clusterParameters.Cluster = new LireKmeans();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clusterParameters.ClusterMethod), clusterParameters.ClusterMethod, null);
            }
        }

        public static void Cluster(ref BaseParameters baseParameters)
        {
            var clusterParameters = baseParameters.ClusterParameters;
            GetClusterType(ref clusterParameters);

            var sampleImgs = Files.GetFilesFrom(clusterParameters.BaseFolder);
            var descriptors = new List<double[]>();
            var featuresPerImage = clusterParameters.MaxNumberClusterFeatures / sampleImgs.Length;

            foreach (var image in sampleImgs)
            {
                var bitmap = new LocalBitmap(image, baseParameters.ImageHeight, baseParameters.ImageWidth);

                descriptors.AddRange(
                    baseParameters.ExtractionFeature.ExtractDescriptors(bitmap)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(featuresPerImage));
            }

            if (clusterParameters.IsDistinctDescriptors)
            {
                Arrays.GetDistinctObjects(ref descriptors);
            }
            if (clusterParameters.OrderByDescending)
            {
                descriptors = descriptors.OrderByDescending(b => b[0]).ToList();
            }
            baseParameters.
                IrmParameters.
                ClusterModels.
                Add(
                        baseParameters.
                        ClusterParameters.
                        Cluster.
                        CreateClusters(descriptors, clusterParameters.ClusterNum)
                    );
        }
    }

    public class ClusterParameters
    {
        public ClusterMethod ClusterMethod;
        public ICluster Cluster;
        //if Lboc cluster num is num of visual words and palette size is number of boc palette
        public int SampleImages, ClusterNum, PaletteSize, NumVWords, MaxNumberClusterFeatures;
        public bool IsRandomInit = false, IsDistinctDescriptors = false, OrderByDescending = false;
        public string BaseFolder;

    }

}
