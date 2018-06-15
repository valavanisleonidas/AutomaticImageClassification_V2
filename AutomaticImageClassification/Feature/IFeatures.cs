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
        VlFeatFisherVector,

        //boc model
        Lboc,
        Boc,


        //bovw model
        MkLabVlad,
        VlFeatSift,
        VlFeatDenseSift,
        VlFeatPhow,
        //AccordSurf,
        ColorCorrelogram,
        //JOpenSurf,
        MkLabSift,
        MkLabSurf,
        

        //textual model
        TfIdf,
        WordEmbeddings,
        
    }
}
