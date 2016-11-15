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
        private IKdTree _tree;
        private KdTreeParameters _params;

        public KdTreeManager(KdTreeParameters kdTreeParameters)
        {
            _params = kdTreeParameters;
            InitKdTree();
        }

        private void InitKdTree()
        {
            switch (_params.Kdtree)
            {
                case KdTreeMethod.AccordKdTree:
                    _tree = new AccordKdTree(_params.Model.Means);
                    break;
                case KdTreeMethod.JavaMlKdTree:
                    _tree = new JavaMlKdTree();
                    break;
                case KdTreeMethod.VlFeatKdTree:
                    _tree = new VlFeatKdTree();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_params.Kdtree), _params.Kdtree, null);
            }
        }

        public IKdTree CreateTree()
        {
            _tree.CreateTree(_params.Model.Means);
            return _tree;
        }
        
    }

    public class KdTreeParameters
    {
        public ClusterModel Model;
        public KdTreeMethod Kdtree;
    }

}
