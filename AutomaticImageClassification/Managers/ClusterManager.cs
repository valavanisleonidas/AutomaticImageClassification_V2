using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Cluster.EM;
using AutomaticImageClassification.Cluster.GaussianMixtureModel;
using AutomaticImageClassification.Cluster.Kmeans;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Managers
{
    public class ClusterManager
    {
        private readonly ClusterParameters _clusterParameters;
        public ICluster ClusterInstance;

        public ClusterManager(ClusterParameters clusterParameters)
        {
            _clusterParameters = clusterParameters;
            GetClusterType();
        }

        private void GetClusterType()
        {
            switch (_clusterParameters.ClusterMethod)
            {
                case ClusterMethod.VlFeatEm:
                    ClusterInstance = new VlFeatEm(_clusterParameters.IsRandomInit);
                    break;
                case ClusterMethod.VlFeatGmm:
                    ClusterInstance = new VlFeatGmm();
                    break;
                case ClusterMethod.VlFeatKmeans:
                    ClusterInstance = new VlFeatKmeans();
                    break;
                case ClusterMethod.AccordKmeans:
                    ClusterInstance = new AccordKmeans();
                    break;
                case ClusterMethod.LireKmeans:
                    ClusterInstance = new LireKmeans();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_clusterParameters.ClusterMethod), _clusterParameters.ClusterMethod, null);
            }
        }

        public ClusterModel Cluster(ImageRepresentationManager manager)
        {
            var sampleImgs = Files.GetFilesFrom(_clusterParameters.BaseFolder, _clusterParameters.SampleImages);
            var descriptors = new List<double[]>();
            var featuresPerImage = _clusterParameters.MaxNumberClusterFeatures / sampleImgs.Length;

            var feature = manager.InitBeforeCluster();
            foreach (var image in sampleImgs)
            {
                var bitmap = new LocalBitmap(image, _clusterParameters.Height, _clusterParameters.Width);

                descriptors.AddRange(
                    feature.ExtractDescriptors(bitmap)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(featuresPerImage));
            }

            if (_clusterParameters.IsDistinctDescriptors)
            {
                Arrays.GetDistinctObjects(ref descriptors);
            }
            if (_clusterParameters.OrderByDescending)
            {
                descriptors = descriptors.OrderByDescending(b => b[0]).ToList();
            }

            return ClusterInstance.CreateClusters(descriptors, _clusterParameters.ClusterNum);
        }
    }

    public class ClusterParameters
    {
        public ClusterMethod ClusterMethod;

        public int SampleImages, ClusterNum, Height, Width, MaxNumberClusterFeatures;
        public bool IsRandomInit = false, IsDistinctDescriptors = false, OrderByDescending = false;
        public string BaseFolder;
    }

}
