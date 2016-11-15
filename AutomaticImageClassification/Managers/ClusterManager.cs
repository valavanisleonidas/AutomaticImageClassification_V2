using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ClusterParameters _clusterParameters;


        public ClusterManager(ClusterParameters clusterParameters)
        {
            _clusterParameters = clusterParameters;
       
            //get cluster type
            GetClusterType();

        }

        private void GetClusterType()
        {
            switch (_clusterParameters.ClusterMethod)
            {
                case ClusterMethod.VlFeatEm:
                    _clusterParameters.Cluster = new VlFeatEm(_clusterParameters.IsRandomInit, _clusterParameters.NumberOfFeatures);
                    break;
                case ClusterMethod.VlFeatGmm:
                    _clusterParameters.Cluster = new VlFeatGmm(_clusterParameters.NumberOfFeatures);
                    break;
                case ClusterMethod.VlFeatKmeans:
                    _clusterParameters.Cluster = new VlFeatKmeans(_clusterParameters.NumberOfFeatures);
                    break;
                case ClusterMethod.AccordKmeans:
                    _clusterParameters.Cluster = new AccordKmeans(_clusterParameters.NumberOfFeatures);
                    break;
                case ClusterMethod.LireKmeans:
                    _clusterParameters.Cluster = new LireKmeans(_clusterParameters.NumberOfFeatures);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_clusterParameters.ClusterMethod), _clusterParameters.ClusterMethod, null);
            }
        }

        public ClusterModel Cluster()
        {
            var sampleImgs = Files.GetFilesFrom(_clusterParameters.BaseFolder, _clusterParameters.SampleImages);
            var descriptors = new List<double[]>();
            foreach (var image in sampleImgs)
            {
                descriptors.AddRange(_clusterParameters.Feature.ExtractDescriptors(image));
            }
            return _clusterParameters.Cluster.CreateClusters(descriptors, _clusterParameters.ClusterNum);
            
        }
    }

    public class ClusterParameters
    {
        public ClusterMethod ClusterMethod;
        public ICluster Cluster;
        public IFeatures Feature;
        public int SampleImages, ClusterNum, NumberOfFeatures = int.MaxValue;
        public bool IsRandomInit = false;
        public string BaseFolder;
    }

}
