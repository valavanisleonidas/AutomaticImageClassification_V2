using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Models.Fields.Features;
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
        private readonly ImageRepresentationParameters _params;


        public ImageRepresentationManager(ImageRepresentationParameters imageRepresentationParameters)
        {
            _params = imageRepresentationParameters;
        }

        public IFeatures InitBeforeCluster()
        {
            switch (_params.ImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    _feature = new Boc(_params.ColorSpace);
                    break;
                case ImageRepresentationMethod.Lboc:
                    _feature = new Lboc(_params.ColorSpace, _params.BocClusterModel);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    _feature = new AccordSurf();
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    //extraction method
                    _feature = new ColorCorrelogram(_params.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    _feature = new JOpenSurf();
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    _feature = new MkLabSift(_params.MkLabSiftExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    _feature = new MkLabSurf(_params.MkLabSurfExtractionMethod);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    _feature = new MkLabVlad(_params.MkLabVladFeatureExtractor);
                    break;
                case ImageRepresentationMethod.OpenCvSift:
                    _feature = new OpenCvSift();
                    break;
                case ImageRepresentationMethod.OpenCvSurf:
                    _feature = new OpenCvSurf();
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    _feature = new VlFeatDenseSift(_params.Step, _params.IsRootSift, _params.IsNormalizedSift,
                        _params.UseCombinedQuantization, _params.ImageHeight, _params.ImageWidth);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    _feature = new VlFeatFisherVector(_params.VlFeatFisherVectorFeatureExtractor);
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    _feature = new VlFeatPhow();
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    _feature = new VlFeatSift(_params.ImageWidth, _params.ImageHeight);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    _feature = new TfIdf();
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    _feature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_params.ImageRepresentationMethod), _params.ImageRepresentationMethod, null);
            }
            _params.Feature = _feature;
            return _feature;
        }

        public IFeatures InitAfterCluster()
        {
            switch (_params.ImageRepresentationMethod)
            {
                case ImageRepresentationMethod.Boc:
                    _feature = new Boc(_params.ColorSpace,_params.ClusterModel);
                    break;
                case ImageRepresentationMethod.Lboc:
                    _feature = new Lboc(_params.ColorSpace, _params.BocClusterModel,_params.LbocClusterModel);
                    break;
                case ImageRepresentationMethod.AccordSurf:
                    _feature = new AccordSurf(_params.ClusterModel);
                    break;
                case ImageRepresentationMethod.ColorCorrelogram:
                    _feature = new ColorCorrelogram(_params.ColorCorrelogramExtractionMethod);
                    break;
                case ImageRepresentationMethod.JOpenSurf:
                    _feature = new JOpenSurf(_params.ClusterModel);
                    break;
                case ImageRepresentationMethod.MkLabSift:
                    //extraction method
                    _feature = new MkLabSift(_params.ClusterModel);
                    break;
                case ImageRepresentationMethod.MkLabSurf:
                    //extraction method
                    _feature = new MkLabSurf(_params.ClusterModel);
                    break;
                case ImageRepresentationMethod.MkLabVlad:
                    //ifeatures
                    _feature = new MkLabVlad(_params.MkLabVladFeatureExtractor);
                    break;
                case ImageRepresentationMethod.OpenCvSift:
                    _feature = new OpenCvSift();
                    break;
                case ImageRepresentationMethod.OpenCvSurf:
                    _feature = new OpenCvSurf();
                    break;
                case ImageRepresentationMethod.VlFeatDenseSift:
                    _feature = new VlFeatDenseSift(_params.Step, _params.IsRootSift, _params.IsNormalizedSift,
                        _params.UseCombinedQuantization, _params.ImageHeight, _params.ImageWidth);
                    break;
                case ImageRepresentationMethod.VlFeatFisherVector:
                    _feature = new VlFeatFisherVector(_params.VlFeatFisherVectorFeatureExtractor);
                    break;
                case ImageRepresentationMethod.VlFeatPhow:
                    _feature = new VlFeatPhow();
                    break;
                case ImageRepresentationMethod.VlFeatSift:
                    _feature = new VlFeatSift(_params.ImageWidth, _params.ImageHeight);
                    break;
                case ImageRepresentationMethod.TfIdf:
                    _feature = new TfIdf(_params.TfidfApproach);
                    break;
                case ImageRepresentationMethod.WordEmbeddings:
                    _feature = new WordEmbedding();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_params.ImageRepresentationMethod), _params.ImageRepresentationMethod, null);
            }
            _params.Feature = _feature;
            return _feature;
        }
    }

    public class ImageRepresentationParameters
    {
        public ImageRepresentationMethod ImageRepresentationMethod;
        public IFeatures Feature;

        public int ImageWidth, ImageHeight, Step, ClusterNum;
        public bool IsRootSift, IsNormalizedSift, UseCombinedQuantization;

        public ColorCorrelogram.ColorCorrelogramExtractionMethod ColorCorrelogramExtractionMethod;
        public MkLabSift.MkLabSiftExtractionMethod MkLabSiftExtractionMethod;
        public MkLabSurf.MkLabSurfExtractionMethod MkLabSurfExtractionMethod;

        public IFeatures MkLabVladFeatureExtractor;
        public IFeatures VlFeatFisherVectorFeatureExtractor;

        public TfidfApproach TfidfApproach;

        public ClusterModel ClusterModel;

        //boc , lboc
        public ColorConversion.ColorSpace ColorSpace;
        public ClusterModel BocClusterModel;
        public ClusterModel LbocClusterModel;


    }

}
