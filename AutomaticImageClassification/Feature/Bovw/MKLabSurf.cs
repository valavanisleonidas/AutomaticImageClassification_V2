using gr.iti.mklab.visual.extraction;
using java.awt.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.KDTree;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class MkLabSurf : IFeatures
    {
        private readonly AbstractFeatureExtractor _surf;
        private readonly MkLabSurfExtractionMethod _mkLabSurfExtractionMethod;
        private readonly ClusterModel _clusterModel;

        public MkLabSurf(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }

        public MkLabSurf()
        {
            _mkLabSurfExtractionMethod = MkLabSurfExtractionMethod.ColorSurf;
            _surf = new ColorSURFExtractor();
        }

        public MkLabSurf(MkLabSurfExtractionMethod mkLabSurfExtractionMethod)
        {
            _mkLabSurfExtractionMethod = mkLabSurfExtractionMethod;
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

        public List<double[]> ExtractDescriptors(string input)
        {
            var bimage = new BufferedImage(new Bitmap(input));
            double[][] surfFeatures = _surf.extractFeatures(bimage);
            return surfFeatures.ToList();
        }

        public double[] ExtractHistogram(string input)
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
            return _mkLabSurfExtractionMethod.ToString();
        }
    }
}
