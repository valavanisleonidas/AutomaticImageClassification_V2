using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Feature
{
    public class ColorCorrelogram : IFeatures
    {
        public double[] ExtractHistogram(string input)
        {
            throw new NotImplementedException();
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            throw new NotImplementedException("Auto color correlogram returns the final histogram and not descriptors of an image!");
        }
    }
}
