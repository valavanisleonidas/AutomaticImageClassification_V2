using AutomaticImageClassification.Cluster.KDTree;
using gr.iti.mklab.visual.extraction;
using java.awt.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class MkLabSurf : IFeatures
    {
        private AbstractFeatureExtractor _surf;
        private IKdTree _tree;
        private ExtractionMethod _extractionMethod;
        private int _clusterNum;

        public MkLabSurf(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }

        public MkLabSurf()
        {
            _extractionMethod = ExtractionMethod.ColorSurf;
            _surf = new ColorSURFExtractor();
        }

        public MkLabSurf(ExtractionMethod extractionMethod)
        {
            _extractionMethod = extractionMethod;
            switch (_extractionMethod)
            {
                case ExtractionMethod.Surf:
                    _surf = new SURFExtractor();
                    break;
                case ExtractionMethod.ColorSurf:
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
            double[] imgVocVector = new double[_clusterNum];//num of clusters

            //for each centroid find min position in tree and increase corresponding index
            List<int> indexes = _tree.SearchTree(features);
            foreach (var index in indexes)
            {
                imgVocVector[index]++;
            }
            return imgVocVector;
        }

        public enum ExtractionMethod
        {
            Surf,
            ColorSurf
        }

        public override string ToString()
        {
            return _extractionMethod.ToString();
        }
    }
}
