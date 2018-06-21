using gr.iti.mklab.visual.extraction;
using java.awt.image;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Local
{
    //from mklab
    public class Surf : IFeatures
    {
        private AbstractFeatureExtractor _surf;
        private readonly SurfExtractionMethod _SurfExtractionMethod;
        private readonly ClusterModel _clusterModel;

        public bool CanCluster
        {
            get { return true; }
        }

        public Surf(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
            _SurfExtractionMethod = SurfExtractionMethod.ColorSurf;
            GetType();
        }

        public Surf(ClusterModel clusterModel, SurfExtractionMethod mkLabSurfExtractionMethod)
        {
            _clusterModel = clusterModel;
            _SurfExtractionMethod = mkLabSurfExtractionMethod;
            GetType();
        }

        public Surf()
        {
            _SurfExtractionMethod = SurfExtractionMethod.ColorSurf;
            GetType();
        }

        public Surf(SurfExtractionMethod mkLabSurfExtractionMethod)
        {
            _SurfExtractionMethod = mkLabSurfExtractionMethod;
            GetType();
        }

        public new void GetType()
        {
            switch (_SurfExtractionMethod)
            {
                case SurfExtractionMethod.Surf:
                    _surf = new SURFExtractor();
                    break;
                case SurfExtractionMethod.ColorSurf:
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

        public enum SurfExtractionMethod
        {
            Surf,
            ColorSurf
        }

        public override string ToString()
        {
            return "MkLab" + _SurfExtractionMethod;
        }
    }
}
