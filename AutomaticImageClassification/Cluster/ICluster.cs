using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Cluster
{
    public interface ICluster
    {
        List<double[]> CreateClusters(List<double[]> descriptorFeatures, int clustersNum);
    }
}
