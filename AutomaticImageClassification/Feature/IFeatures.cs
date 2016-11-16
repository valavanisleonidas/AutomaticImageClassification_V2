using System.Collections.Generic;
using System.Drawing;
using AutomaticImageClassification.Utilities;


namespace AutomaticImageClassification.Feature
{
    public interface IFeatures
    {
        double[] ExtractHistogram(LocalBitmap input);
        List<double[]> ExtractDescriptors(LocalBitmap input);   
    }

    public enum ImageRepresentationMethod
    {
        Boc,
        Lboc,
        AccordSurf,
        ColorCorrelogram,
        JOpenSurf,
        MkLabSift,
        MkLabSurf,
        MkLabVlad,
        OpenCvSift,
        OpenCvSurf,
        VlFeatDenseSift,
        VlFeatFisherVector,
        VlFeatPhow,
        VlFeatSift,
        TfIdf,
        WordEmbeddings
    }
}
