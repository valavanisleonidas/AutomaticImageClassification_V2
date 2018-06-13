using System;
using System.Collections.Generic;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Feature.Boc;
using AutomaticImageClassification.Feature.Bovw;
using AutomaticImageClassification.Feature.Textual;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Managers
{
    public class ImageRepresentationManager
    {
        public static void InitBeforeCluster(ref BaseParameters baseParameters)
        {
            
            var irmParameters = baseParameters.IrmParameters;
            switch (irmParameters.CurrentImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    baseParameters.ExtractionFeature = new Boc(irmParameters.ColorSpace);
                    break;
                case ImageRepresentationMethod.Lboc:
                    //set extraction feature to boc so that we extract boc descriptors
                    baseParameters.ExtractionFeature = new Boc(irmParameters.ColorSpace);

                    ClusterManager.Cluster(ref baseParameters);
                    KdTreeManager.CreateTree(ref baseParameters);
                    var clustersFile = @"Data\Palettes\" + baseParameters.ExtractionFeature + "_" + baseParameters.ClusterParameters.ClusterNum + "_clusters.txt";
                    Files.WriteFile(clustersFile, baseParameters.IrmParameters.ClusterModels[0].Means);


                    baseParameters.ExtractionFeature = new Lboc(irmParameters.ColorSpace, irmParameters.ClusterModels[0]);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    baseParameters.ExtractionFeature = new AccordSurf();
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    //extraction method
                    //throw new ArgumentException("Auto color correlogram returns the final histogram and not descriptors of an image!");
                    baseParameters.ExtractionFeature = new ColorCorrelogram(irmParameters.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    baseParameters.ExtractionFeature = new JOpenSurf();
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    baseParameters.ExtractionFeature = new MkLabSift(irmParameters.MkLabSiftExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    baseParameters.ExtractionFeature = new MkLabSurf(irmParameters.MkLabSurfExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.IrmToUseDescriptors;
                    InitBeforeCluster(ref baseParameters);

                    baseParameters.ExtractionFeature = new MkLabVlad(baseParameters.ExtractionFeature);

                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.BasicImageRepresentationMethod;
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    baseParameters.ExtractionFeature = new VlFeatDenseSift(irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.IrmToUseDescriptors;
                    InitBeforeCluster(ref baseParameters);

                    baseParameters.ExtractionFeature = new VlFeatFisherVector(baseParameters.ExtractionFeature);

                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.BasicImageRepresentationMethod;
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    baseParameters.ExtractionFeature = new VlFeatPhow();
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    baseParameters.ExtractionFeature = new VlFeatSift(irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    baseParameters.ExtractionFeature = new TfIdf();
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    baseParameters.ExtractionFeature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(irmParameters.CurrentImageRepresentationMethod), irmParameters.CurrentImageRepresentationMethod, null);
            }
        }

        public static void InitAfterCluster(ref BaseParameters baseParameters)
        {
            var irmParameters = baseParameters.IrmParameters;
            switch (irmParameters.CurrentImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    baseParameters.ExtractionFeature = new Boc(irmParameters.ColorSpace, irmParameters.ClusterModels[0]);
                    break;
                case ImageRepresentationMethod.Lboc:
                    baseParameters.ExtractionFeature = new Lboc(irmParameters.ColorSpace, irmParameters.ClusterModels[0], irmParameters.ClusterModels[1]);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    baseParameters.ExtractionFeature = new AccordSurf(irmParameters.ClusterModels[0]);
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    baseParameters.ExtractionFeature = new ColorCorrelogram(irmParameters.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    baseParameters.ExtractionFeature = new JOpenSurf(irmParameters.ClusterModels[0]);
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    baseParameters.ExtractionFeature = new MkLabSift(irmParameters.ClusterModels[0], irmParameters.MkLabSiftExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    baseParameters.ExtractionFeature = new MkLabSurf(irmParameters.ClusterModels[0], irmParameters.MkLabSurfExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.IrmToUseDescriptors;
                    InitBeforeCluster(ref baseParameters);

                    baseParameters.ExtractionFeature = new MkLabVlad(irmParameters.ClusterModels[0], baseParameters.ExtractionFeature);

                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.BasicImageRepresentationMethod;
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    baseParameters.ExtractionFeature = new VlFeatDenseSift(irmParameters.ClusterModels[0], irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.IrmToUseDescriptors;
                    InitBeforeCluster(ref baseParameters);

                    baseParameters.ExtractionFeature = new VlFeatFisherVector(irmParameters.ClusterModels[0], baseParameters.ExtractionFeature);

                    irmParameters.CurrentImageRepresentationMethod = baseParameters.IrmParameters.BasicImageRepresentationMethod;
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    baseParameters.ExtractionFeature = new VlFeatPhow(irmParameters.ClusterModels[0]);
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    baseParameters.ExtractionFeature = new VlFeatSift(irmParameters.ClusterModels[0], irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    baseParameters.ExtractionFeature = new TfIdf(irmParameters.TfidfApproach);
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    baseParameters.ExtractionFeature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(irmParameters.CurrentImageRepresentationMethod), irmParameters.CurrentImageRepresentationMethod, null);
            }
        }
    }

    public class ImageRepresentationParameters
    {
        //todo lusimo thematos mkvlad , fisher
        //todo lusimo thematos lboc model me 2 cluster models

        public ImageRepresentationMethod CurrentImageRepresentationMethod;

        public ColorCorrelogram.ColorCorrelogramExtractionMethod ColorCorrelogramExtractionMethod;
        public MkLabSift.MkLabSiftExtractionMethod MkLabSiftExtractionMethod;
        public MkLabSurf.MkLabSurfExtractionMethod MkLabSurfExtractionMethod;

        //some extraction methods need another feature to extract features
        public ImageRepresentationMethod BasicImageRepresentationMethod;
        public ImageRepresentationMethod IrmToUseDescriptors;

        public List<ClusterModel> ClusterModels = new List<ClusterModel>();


        public bool UseCombinedQuantization = true;
        public ColorConversion.ColorSpace ColorSpace = ColorConversion.ColorSpace.RGB;

        //tfidf
        public TfidfApproach TfidfApproach;

    }


    public class BaseParameters
    {
        public IFeatures ExtractionFeature { get; set; }
        public int ImageHeight, ImageWidth;

        public ImageRepresentationParameters IrmParameters;
        public ClusterParameters ClusterParameters;
        public KdTreeParameters KdTreeParameters;
        public ClassifierParameters ClassifierParameters;


        public BaseParameters(IFeatures extractionFeature, int imageHeight, int imageWidth)
        {
            ExtractionFeature = extractionFeature;
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
        }
        public BaseParameters(int imageHeight, int imageWidth)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
        }
        public BaseParameters(IFeatures extractionFeature)
        {
            ExtractionFeature = extractionFeature;
        }



    }



}
