using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gr.iti.mklab.visual.aggregation;

namespace AutomaticImageClassification.Feature
{
    public class MkLabVlad : IFeatures
    {
        private IFeatures _featureExtractor;
        private VladAggregator _vlad;

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
            _vlad = new VladAggregator(codebook.ToArray());
        }

        public MkLabVlad(List<double[]> codebook, IFeatures extractor)
        {
            _featureExtractor = extractor;
            _vlad = new VladAggregator(codebook.ToArray());
        }

        public double[] ExtractHistogram(string input)
        {
            return _vlad.aggregate(_featureExtractor.ExtractDescriptors(input).ToArray());
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
