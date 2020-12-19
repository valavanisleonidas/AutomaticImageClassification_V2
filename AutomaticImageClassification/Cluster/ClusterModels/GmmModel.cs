using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Cluster.ClusterModels
{
    public class GmmModel : ClusterModel
    {
        public GmmModel(List<double[]> means, List<double[]> covariances, double[] priors) : base(means, covariances, priors) { }
    }
}
