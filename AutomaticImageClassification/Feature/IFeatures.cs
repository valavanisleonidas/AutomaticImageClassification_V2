using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Feature
{
    public interface IFeatures
    {
        double[] ExtractHistogram(string input);
        List<double[]> ExtractDescriptors(string input);
        
    }
}
