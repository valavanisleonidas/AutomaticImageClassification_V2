using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;
using java.awt.image;

namespace AutomaticImageClassification.Feature.Global
{
    public class CEDD: IGlobalFeatures
    {
        private net.semanticmetadata.lire.imageanalysis.CEDD cedd = new net.semanticmetadata.lire.imageanalysis.CEDD();
      
        public CEDD()
        {
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            var image = new BufferedImage(input.Bitmap);
            cedd.extract(image);
            return cedd.getDoubleHistogram();
        }
        
      
        public override string ToString()
        {
            return "CEDD";
        }

        
    }
}
