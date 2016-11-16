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
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class MkLabSift : IFeatures
    {
        private AbstractFeatureExtractor _sift;
        private MkLabSiftExtractionMethod _mkLabSiftExtractionMethod;
        private readonly ClusterModel _clusterModel;

        public MkLabSift(ClusterModel clusterModel)
        {
            _clusterModel = clusterModel;
        }

        public MkLabSift()
        {
            _mkLabSiftExtractionMethod = MkLabSiftExtractionMethod.RootSift;
            _sift = new RootSIFTExtractor();
        }

        public MkLabSift(MkLabSiftExtractionMethod mkLabSiftExtractionMethod)
        {
            _mkLabSiftExtractionMethod = mkLabSiftExtractionMethod;
            switch (_mkLabSiftExtractionMethod)
            {
                case MkLabSiftExtractionMethod.Sift:
                    _sift = new SIFTExtractor();
                    break;
                case MkLabSiftExtractionMethod.RootSift:
                    _sift = new RootSIFTExtractor();
                    break;
                default:
                    _sift = new RootSIFTExtractor();
                    break;
            }
        }

        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            var bimage = new BufferedImage(input.Bitmap);
            double[][] siftFeatures = _sift.extractFeatures(bimage);
            return siftFeatures.ToList();
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

        public enum MkLabSiftExtractionMethod
        {
            Sift,
            RootSift
        }

        public override string ToString()
        {
            return _mkLabSiftExtractionMethod.ToString();
        }

    }
}
