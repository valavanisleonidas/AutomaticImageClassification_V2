using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;
using AutomaticImageClassification.Feature.Global.BrightnessTextureDirectionalityHistogram;

namespace AutomaticImageClassification.Feature.Global
{
    public class BTDH : IGlobalFeatures
    {
        private BTDH_Impl btdh;
       
        public BTDH()
        {
            btdh = new BTDH_Impl(16,8,false);
        }
        
        public double[] ExtractHistogram(LocalBitmap input)
        {
            return btdh.extract(input.Bitmap);
        }
        
        public override string ToString()
        {
            return "BTDH";
        }

        
    }
}
