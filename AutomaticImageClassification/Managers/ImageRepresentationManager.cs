using System;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Feature.Boc;
using AutomaticImageClassification.Feature.Bovw;
using AutomaticImageClassification.Feature.Textual;
using AutomaticImageClassification.KDTree;

namespace AutomaticImageClassification.Managers
{
    public class ImageRepresentationManager
    {
        public IFeatures Feature;
        public readonly ImageRepresentationParameters IrmParameters;

        public ImageRepresentationManager(ImageRepresentationParameters imageRepresentationParameters)
        {
            IrmParameters = imageRepresentationParameters;
        }

        public IFeatures InitBeforeCluster()
        {
            switch (IrmParameters.ImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    Feature = new Boc(IrmParameters.ColorSpace);
                    break;
                case ImageRepresentationMethod.Lboc:
                    //ClusterModel bocClusterModel = IrmParameters.ClusterManager.Cluster();
                    Feature = new Lboc(IrmParameters.ColorSpace, IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    Feature = new AccordSurf();
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    //extraction method
                    //throw new ArgumentException("Auto color correlogram returns the final histogram and not descriptors of an image!");
                    Feature = new ColorCorrelogram(IrmParameters.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    Feature = new JOpenSurf();
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    Feature = new MkLabSift(IrmParameters.MkLabSiftExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    Feature = new MkLabSurf(IrmParameters.MkLabSurfExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    Feature = new MkLabVlad(IrmParameters.Feature);
                    break;
                case ImageRepresentationMethod.OpenCvSift:
                    Feature = new OpenCvSift();
                    break;
                case ImageRepresentationMethod.OpenCvSurf:
                    Feature = new OpenCvSurf();
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    Feature = new VlFeatDenseSift(IrmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    Feature = new VlFeatFisherVector(IrmParameters.Feature);
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    Feature = new VlFeatPhow();
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    Feature = new VlFeatSift(IrmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    Feature = new TfIdf();
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    Feature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(IrmParameters.ImageRepresentationMethod), IrmParameters.ImageRepresentationMethod, null);
            }
            IrmParameters.Feature = Feature;
            return Feature;
        }

        public IFeatures InitAfterCluster()
        {
            switch (IrmParameters.ImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    Feature = new Boc(IrmParameters.ColorSpace, IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.Lboc:
                    Feature = new Lboc(IrmParameters.ColorSpace, IrmParameters.BocClusterModel, IrmParameters.LbocClusterModel);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    Feature = new AccordSurf(IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    Feature = new ColorCorrelogram(IrmParameters.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    Feature = new JOpenSurf(IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    Feature = new MkLabSift(IrmParameters.ClusterModel, IrmParameters.MkLabSiftExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    Feature = new MkLabSurf(IrmParameters.ClusterModel, IrmParameters.MkLabSurfExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    Feature = new MkLabVlad(IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.OpenCvSift:
                    Feature = new OpenCvSift(IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.OpenCvSurf:
                    Feature = new OpenCvSurf(IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    Feature = new VlFeatDenseSift(IrmParameters.ClusterModel, IrmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    Feature = new VlFeatFisherVector(IrmParameters.ClusterModel, IrmParameters.Feature);
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    Feature = new VlFeatPhow(IrmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    Feature = new VlFeatSift(IrmParameters.ClusterModel, IrmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    Feature = new TfIdf(IrmParameters.TfidfApproach);
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    Feature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(IrmParameters.ImageRepresentationMethod), IrmParameters.ImageRepresentationMethod, null);
            }
            IrmParameters.Feature = Feature;
            return Feature;
        }
    }

    public class ImageRepresentationParameters : BaseParameters
    {
        //todo lusimo thematos mkvlad , fisher
        //todo lusimo thematos lboc model me 2 cluster models

        public ImageRepresentationMethod ImageRepresentationMethod;
        public ImageRepresentationMethod IrmToUseDescriptors;

        public ColorCorrelogram.ColorCorrelogramExtractionMethod ColorCorrelogramExtractionMethod;
        public MkLabSift.MkLabSiftExtractionMethod MkLabSiftExtractionMethod;
        public MkLabSurf.MkLabSurfExtractionMethod MkLabSurfExtractionMethod;

        //some extraction methods need another feature to extract features
        public IFeatures Feature { get; set; }

        public ClusterModel ClusterModel;

        public bool UseCombinedQuantization = true;
        public ColorConversion.ColorSpace ColorSpace = ColorConversion.ColorSpace.RGB;


        //tfidf
        public TfidfApproach TfidfApproach;


        public ClusterManager ClusterManager;
        public ClusterModel BocClusterModel;
        public ClusterModel LbocClusterModel;


        public ImageRepresentationParameters(IFeatures extractionFeature, int imageHeight, int imageWidth)
            : base(extractionFeature, imageHeight, imageWidth) { }

        public ImageRepresentationParameters(int imageHeight, int imageWidth)
            : base(imageHeight, imageWidth) { }

        public int GetWidth()
        {
            return ImageWidth;
        }

        public int GetHeight()
        {
            return ImageHeight;
        }

        public IFeatures GetExtractionFeature()
        {
            return ExtractionFeature;
        }
        public void SetExtractionFeature(IFeatures feature)
        {
            ExtractionFeature = feature;
        }


    }


    public abstract class BaseParameters
    {
        protected IFeatures ExtractionFeature { get; set; }
        protected int ImageHeight, ImageWidth;

        protected BaseParameters(IFeatures extractionFeature, int imageHeight, int imageWidth)
        {
            ExtractionFeature = extractionFeature;
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
        }
        protected BaseParameters(  int imageHeight, int imageWidth)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
        }
        protected BaseParameters(IFeatures extractionFeature)
        {
            ExtractionFeature = extractionFeature;
        }



    }



}
