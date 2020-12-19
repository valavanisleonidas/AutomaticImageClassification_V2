using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;
using java.awt.image;

namespace AutomaticImageClassification.Feature.Global
{
    public class PHOG: IGlobalFeatures
    {
        private net.semanticmetadata.lire.imageanalysis.PHOG phog = new net.semanticmetadata.lire.imageanalysis.PHOG();
    
        public PHOG()
        {
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            var image = new BufferedImage(input.Bitmap);
            phog.extract(image);
            return phog.getDoubleHistogram();
        }
        

        public override string ToString()
        {
            return "PHOG";
        }


    }
}
