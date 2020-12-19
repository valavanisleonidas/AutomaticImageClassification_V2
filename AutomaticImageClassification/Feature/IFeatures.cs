using AutomaticImageClassification.Utilities;


namespace AutomaticImageClassification.Feature
{
    public interface IFeatures
    {
        double[] ExtractHistogram(LocalBitmap input);
    }

    public enum ImageRepresentationMethod
    {

        //boc model
        Lboc,
        Boc,
        GBoC,

        Vlad,
        FisherVector,
        DenseSift,
        Phow,
        Sift,
        Surf,

        //textual model
        TfIdf,
        WordEmbeddings,


        BTDH,
        CEDD,
        Correlogram,
        EdgeHistogram,
        FCTH,
        JCD,
        PHOG
    }

}
