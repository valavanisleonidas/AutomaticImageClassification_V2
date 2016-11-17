using System.Collections.Generic;
using System.Drawing;
using AutomaticImageClassification.Utilities;


namespace AutomaticImageClassification.Feature
{
    public interface IFeatures
    {
        bool CanCluster { get; }
        double[] ExtractHistogram(LocalBitmap input);
        List<double[]> ExtractDescriptors(LocalBitmap input);   
    }

    public enum ImageRepresentationMethod
    {

        //boc model
        Boc,
        Lboc,

        //bovw model
        VlFeatSift,
        VlFeatDenseSift,
        VlFeatPhow,
        VlFeatFisherVector,
        AccordSurf,
        ColorCorrelogram,
        JOpenSurf,
        MkLabSift,
        MkLabSurf,
        MkLabVlad,
        OpenCvSift,
        OpenCvSurf,
        


        
        //textual model
        TfIdf,
        WordEmbeddings,
        
    }
}
