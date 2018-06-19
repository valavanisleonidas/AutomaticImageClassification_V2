using gr.iti.mklab.visual.extraction;
using java.awt.image;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Local
{
    public class MkLabSurf : IFeatures
    {
        private AbstractFeatureExtractor _surf;
        private readonly MkLabSurfExtractionMethod _mkLabSurfExtractionMethod;
        private readonly ClusterModel _clusterModel;

        public bool CanCluster
        {
            get { return true; }
        }

        public MkLabSurf(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
            _mkLabSurfExtractionMethod = MkLabSurfExtractionMethod.ColorSurf;
            GetType();
        }

        public MkLabSurf(ClusterModel clusterModel, MkLabSurfExtractionMethod mkLabSurfExtractionMethod)
        {
            _clusterModel = clusterModel;
            _mkLabSurfExtractionMethod = mkLabSurfExtractionMethod;
            GetType();
        }

        public MkLabSurf()
        {
            _mkLabSurfExtractionMethod = MkLabSurfExtractionMethod.ColorSurf;
            GetType();
        }

        public MkLabSurf(MkLabSurfExtractionMethod mkLabSurfExtractionMethod)
        {
            _mkLabSurfExtractionMethod = mkLabSurfExtractionMethod;
            GetType();
        }

        public new void GetType()
        {
            switch (_mkLabSurfExtractionMethod)
            {
                case MkLabSurfExtractionMethod.Surf:
                    _surf = new SURFExtractor();
                    break;
                case MkLabSurfExtractionMethod.ColorSurf:
                    _surf = new ColorSURFExtractor();
                    break;
                default:
                    _surf = new ColorSURFExtractor();
                    break;
            }
        }


        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            var bimage = new BufferedImage(input.Bitmap);
            double[][] surfFeatures = _surf.extractFeatures(bimage);
            return surfFeatures.ToList();
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            List<double[]> features = ExtractDescriptors(input);
            double[] imgVocVector = new double[_clusterModel.ClusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            List<int> indexes = _clusterModel.Tree.SearchTree(features);
            foreach (var index in indexes)
            {
                imgVocVector[index]++;
            }
            return imgVocVector;
        }

        public enum MkLabSurfExtractionMethod
        {
            Surf,
            ColorSurf
        }

        public override string ToString()
        {
            return "MkLab" + _mkLabSurfExtractionMethod;
        }
    }
}
