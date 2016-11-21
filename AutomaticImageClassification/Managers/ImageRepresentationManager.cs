using System;
using System.Collections.Generic;
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
        private IFeatures _feature;
        public BaseParameters BaseParameters;
        private readonly ImageRepresentationParameters _irmParameters;

        public ImageRepresentationManager(BaseParameters baseParameters)
        {
            BaseParameters = baseParameters;
            _irmParameters = baseParameters.IrmParameters;
        }

        public IFeatures InitBeforeCluster()
        {
            switch (_irmParameters.ImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    _feature = new Boc(_irmParameters.ColorSpace);
                    break;
                case ImageRepresentationMethod.Lboc:
                    var lbocClusters = BaseParameters.ClusterParameters.ClusterNum;
                    BaseParameters.ClusterParameters.ClusterNum = BaseParameters.ClusterParameters.PaletteSize;
                    ClusterManager manager = new ClusterManager(BaseParameters);
                    BaseParameters.ClusterParameters.ClusterNum = lbocClusters;

                    _irmParameters.BocClusterModel = manager.Cluster();

                    BaseParameters.KdTreeParameters.Model = _irmParameters.BocClusterModel;
                    var kdTreeManager = new KdTreeManager(BaseParameters.KdTreeParameters);
                    BaseParameters.IrmParameters.BocClusterModel.Tree = kdTreeManager.CreateTree();


                    _feature = new Lboc(_irmParameters.ColorSpace, _irmParameters.BocClusterModel);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    _feature = new AccordSurf();
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    //extraction method
                    //throw new ArgumentException("Auto color correlogram returns the final histogram and not descriptors of an image!");
                    _feature = new ColorCorrelogram(_irmParameters.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    _feature = new JOpenSurf();
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    _feature = new MkLabSift(_irmParameters.MkLabSiftExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    _feature = new MkLabSurf(_irmParameters.MkLabSurfExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    _feature = new MkLabVlad(_irmParameters.Feature);
                    break;
                case ImageRepresentationMethod.OpenCvSift:
                    _feature = new OpenCvSift();
                    break;
                case ImageRepresentationMethod.OpenCvSurf:
                    _feature = new OpenCvSurf();
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    _feature = new VlFeatDenseSift(_irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    _feature = new VlFeatFisherVector(_irmParameters.Feature);
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    _feature = new VlFeatPhow();
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    _feature = new VlFeatSift(_irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    _feature = new TfIdf();
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    _feature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_irmParameters.ImageRepresentationMethod), _irmParameters.ImageRepresentationMethod, null);
            }
            BaseParameters.ExtractionFeature = _feature;
            return _feature;
        }

        public IFeatures InitAfterCluster()
        {
            switch (_irmParameters.ImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    _feature = new Boc(_irmParameters.ColorSpace, _irmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.Lboc:
                    _feature = new Lboc(_irmParameters.ColorSpace, _irmParameters.BocClusterModel, _irmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    _feature = new AccordSurf(_irmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    _feature = new ColorCorrelogram(_irmParameters.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    _feature = new JOpenSurf(_irmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    _feature = new MkLabSift(_irmParameters.ClusterModel, _irmParameters.MkLabSiftExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    _feature = new MkLabSurf(_irmParameters.ClusterModel, _irmParameters.MkLabSurfExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    _feature = new MkLabVlad(_irmParameters.ClusterModel, _irmParameters.Feature);
                    break;
                case ImageRepresentationMethod.OpenCvSift:
                    _feature = new OpenCvSift(_irmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.OpenCvSurf:
                    _feature = new OpenCvSurf(_irmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    _feature = new VlFeatDenseSift(_irmParameters.ClusterModel, _irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    _feature = new VlFeatFisherVector(_irmParameters.ClusterModel, _irmParameters.Feature);
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    _feature = new VlFeatPhow(_irmParameters.ClusterModel);
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    _feature = new VlFeatSift(_irmParameters.ClusterModel, _irmParameters.UseCombinedQuantization);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    _feature = new TfIdf(_irmParameters.TfidfApproach);
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    _feature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_irmParameters.ImageRepresentationMethod), _irmParameters.ImageRepresentationMethod, null);
            }
            BaseParameters.ExtractionFeature = _feature;
            return _feature;
        }
    }

    public class ImageRepresentationParameters
    {
        //todo lusimo thematos mkvlad , fisher
        //todo lusimo thematos lboc model me 2 cluster models

        public ImageRepresentationMethod ImageRepresentationMethod;

        public ColorCorrelogram.ColorCorrelogramExtractionMethod ColorCorrelogramExtractionMethod;
        public MkLabSift.MkLabSiftExtractionMethod MkLabSiftExtractionMethod;
        public MkLabSurf.MkLabSurfExtractionMethod MkLabSurfExtractionMethod;

        //some extraction methods need another feature to extract features
        public ImageRepresentationMethod IrmToUseDescriptors;
        public IFeatures Feature { get; set; }

        public ClusterModel ClusterModel;
        public ClusterModel BocClusterModel;
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
