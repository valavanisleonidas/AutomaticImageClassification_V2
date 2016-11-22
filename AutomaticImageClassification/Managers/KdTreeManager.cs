using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;

namespace AutomaticImageClassification.Managers
{
    public class KdTreeManager
    {

        private static void InitKdTree(ref BaseParameters baseParameters)
        {
            var model = baseParameters.IrmParameters.ClusterModels.Last();
            switch (baseParameters.KdTreeParameters.Kdtree)
            {
                case KdTreeMethod.AccordKdTree:
                    model.Tree = new AccordKdTree(model.Means);
                    break;
                case KdTreeMethod.JavaMlKdTree:
                    model.Tree = new JavaMlKdTree();
                    break;
                case KdTreeMethod.VlFeatKdTree:
                    model.Tree = new VlFeatKdTree();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(baseParameters.KdTreeParameters.Kdtree), baseParameters.KdTreeParameters.Kdtree, null);
            }
            baseParameters.IrmParameters.ClusterModels.Last().Tree = model.Tree;
        }

        public static void CreateTree(ref BaseParameters baseParameters)
        {
            InitKdTree(ref baseParameters);

            var lastClusterModel = baseParameters.IrmParameters.ClusterModels.Last();
            lastClusterModel.Tree.CreateTree(lastClusterModel.Means);
            
        }
        
    }

    public class KdTreeParameters
    {
        public KdTreeMethod Kdtree;
    }

}
