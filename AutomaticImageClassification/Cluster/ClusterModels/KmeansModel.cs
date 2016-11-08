using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Cluster.ClusterModels
{
    public class KmeansModel : ClusterModel
    {
        public List<double[]> Clusters;

        public KmeansModel(List<double[]> means) : base(means) { }

    }
}
