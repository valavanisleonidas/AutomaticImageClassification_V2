using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;
using java.awt.image;

namespace AutomaticImageClassification.Feature.Global
{
    public class FCTH : IGlobalFeatures
    {
        private net.semanticmetadata.lire.imageanalysis.FCTH fcth = new net.semanticmetadata.lire.imageanalysis.FCTH();

        public FCTH()
        {
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            var image = new BufferedImage(input.Bitmap);
            return fcth.Apply(image);
        }


        public override string ToString()
        {
            return "FCTH";
        }


    }
}
