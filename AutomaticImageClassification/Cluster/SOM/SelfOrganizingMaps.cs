using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;

namespace AutomaticImageClassification.Cluster.SOM
{
    public class SelfOrganizingMaps : ICluster
    {

        public ClusterModel CreateClusters(List<double[]> descriptorFeatures, int clustersNum)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "SOM";
        }
    }
}
