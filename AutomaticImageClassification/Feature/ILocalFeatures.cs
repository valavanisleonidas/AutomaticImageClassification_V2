using System.Collections.Generic;
using AutomaticImageClassification.Utilities;


namespace AutomaticImageClassification.Feature
{
    public interface ILocalFeatures : IFeatures
    {
        List<double[]> ExtractDescriptors(LocalBitmap input);   
    }
    
}
