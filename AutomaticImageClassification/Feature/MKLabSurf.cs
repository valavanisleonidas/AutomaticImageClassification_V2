using AutomaticImageClassification.Cluster.KDTree;
using gr.iti.mklab.visual.extraction;
using java.awt.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Feature
{
    public class MkLabSurf : IFeatures
    {
        private AbstractFeatureExtractor _surf = new ColorSURFExtractor();
        private IKdTree _tree;
        private int _clusterNum;

        public MkLabSurf(IKdTree tree, int clusterNum)
        {
            _tree = tree;
            _clusterNum = clusterNum;
        }
        public MkLabSurf() { }

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
            foreach (var feature in features)
            {
                int positionofMin = _tree.SearchTree(feature);
                imgVocVector[positionofMin]++;
            }

            return imgVocVector;
        }

        public override string ToString()
        {
            return "MkLabColorSurf";
        }
    }
}
