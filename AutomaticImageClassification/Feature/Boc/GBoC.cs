using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;
using AutomaticImageClassification.Cluster.ClusterModels;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Feature.Boc
{
    public class GBoC : ILocalFeatures
    {
        private readonly ColorConversion.ColorSpace _cs;
        private readonly ClusterModel _clusterModel;
        private readonly int _levels = 4;
        private readonly Boc _boc;

       
        public GBoC(ColorConversion.ColorSpace cs)
        {
            _cs = cs;
            _boc = new Boc(_cs);
        }

        public GBoC(ColorConversion.ColorSpace cs, int levels)
        {
            _cs = cs;
            _levels = levels;
            _boc = new Boc(_cs);
        }

        public GBoC(ColorConversion.ColorSpace cs, ClusterModel clusterModel)
        {
            _cs = cs;
            _clusterModel = clusterModel;
        }

        public GBoC(ClusterModel clusterModel, ColorConversion.ColorSpace cs, int levels )
        {
            _cs = cs;
            _levels = levels;
            _clusterModel = clusterModel;
        }

        
        public List<double[]> ExtractDescriptors(LocalBitmap input)
        {
            return _boc.ExtractDescriptors(input);
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            try
            {
                var gboc = new GBOC.GBoC();
                
                MWArray[] result = gboc.ExtractGGBOC(1,
                    new MWCharArray(input.Path),
                    new MWNumericArray(_clusterModel.Means.ToArray()),
                    new MWCharArray(_cs.ToString()), 
                    new MWNumericArray(_levels));
                
                gboc.Dispose();
                return (double[])((MWNumericArray)result[0]).ToVector(MWArrayComponent.Real);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "GBoC" + "_" + _cs.ToString() + "_levels_" + _levels.ToString();
        }

    }
}
