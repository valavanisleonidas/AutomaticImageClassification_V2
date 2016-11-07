using System.Collections.Generic;
using AutomaticImageClassification.Cluster.KDTree;
using AutomaticImageClassification.Utilities;
using gr.iti.mklab.visual.aggregation;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class MkLabVlad : IFeatures
    {
        private IFeatures _featureExtractor;
        private VladAggregator _vlad;
        private List<double[]> _codebook;
        private IKdTree _tree;
        private int _clusterNum;

        public MkLabVlad()
        {
            _featureExtractor = new MkLabSurf();
        }

        public MkLabVlad(IFeatures extractor)
        {
            _featureExtractor = extractor;
        }

        public MkLabVlad(IKdTree tree, List<double[]> codebook)
        {
            _featureExtractor = new MkLabSurf();
            _tree = tree;
            _codebook = codebook;
            _clusterNum = codebook[0].Length;
        }

        public MkLabVlad(List<double[]> codebook)
        {
            _featureExtractor = new MkLabSurf();
            _codebook = codebook;
            _clusterNum = codebook[0].Length;
            // _vlad = new VladAggregator(codebook.ToArray());
        }

        public MkLabVlad(List<double[]> codebook, IFeatures extractor)
        {
            _featureExtractor = extractor;
            _codebook = codebook;
            _clusterNum = codebook[0].Length;
            //_vlad = new VladAggregator(codebook.ToArray());
        }

        public double[] ExtractHistogram(string input)
        {
            //return _vlad.aggregate(_featureExtractor.ExtractDescriptors(input).ToArray());

            List<double[]> descriptors = _featureExtractor.ExtractDescriptors(input);

            double[] vlad = new double[_codebook.Count * _clusterNum];

            if (descriptors.Count == 0)
            {
                // when there are 0 local descriptors extracted
                return vlad;
            }

            foreach (var descriptor in descriptors)
            {
                int index = _tree?.SearchTree(descriptor)
                    ?? DistanceMetrics.ComputeNearestCentroidL2NotSquare(ref _codebook, descriptor);

                for (int i = 0; i < _clusterNum; i++)
                {
                    vlad[index * _clusterNum + i] += descriptor[i] - _codebook[index][i];
                }
            }
            return vlad;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            return _featureExtractor.ExtractDescriptors(input);
        }

        public override string ToString()
        {
            return "Vlad_" + _featureExtractor;
        }




    }
}
