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
        
        //boc model
        Boc,
        Lboc,



        //textual model
        TfIdf,
        WordEmbeddings,
        
    }
}
