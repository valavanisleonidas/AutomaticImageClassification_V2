using System.Collections.Generic;
using gr.iti.mklab.visual.aggregation;

namespace AutomaticImageClassification.Feature.Bovw
{
    public class MkLabVlad : IFeatures
    {
        private IFeatures _featureExtractor;
        private VladAggregator _vlad;
        private List<double[]> _codebook;
        private int _dimensionsNumber;
        public MkLabVlad()
        {
            _featureExtractor = new MkLabSurf();
        }

        public MkLabVlad(IFeatures extractor)
        {
            _featureExtractor = extractor;
        }

        public MkLabVlad(List<double[]> codebook)
        {
            _featureExtractor = new MkLabSurf();
            _codebook = codebook;
            _dimensionsNumber = codebook[0].Length;
            // _vlad = new VladAggregator(codebook.ToArray());
        }

        public MkLabVlad(List<double[]> codebook, IFeatures extractor)
        {
            _featureExtractor = extractor;
            _codebook = codebook;
            _dimensionsNumber = codebook[0].Length;
            //_vlad = new VladAggregator(codebook.ToArray());
        }

        public double[] ExtractHistogram(string input)
        {
            //return _vlad.aggregate(_featureExtractor.ExtractDescriptors(input).ToArray());

            List<double[]> descriptors = _featureExtractor.ExtractDescriptors(input);

            double[] vlad = new double[_codebook.Count * _dimensionsNumber];

            if (descriptors.Count == 0)
            {
                // when there are 0 local descriptors extracted
                return vlad;
            }

            foreach (var descriptor in descriptors)
            {
                //int indx = _lBoctree?.SearchTree(_boc)
                //    ?? ClusterIndexOf(_dictionary, _boc);

                int index = Utilities.DistanceMetrics.ComputeNearestCentroidL2(_codebook,descriptor);
                for (int i = 0; i < _dimensionsNumber; i++)
                {
                    vlad[index * _dimensionsNumber + i] += descriptor[i] - _codebook[index][i];
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
