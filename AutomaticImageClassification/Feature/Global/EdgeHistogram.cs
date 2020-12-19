using System;
using System.Collections.Generic;
using AutomaticImageClassification.Utilities;
using java.awt.image;

namespace AutomaticImageClassification.Feature.Global
{
    public class EdgeHistogram : IGlobalFeatures
    {
        private net.semanticmetadata.lire.imageanalysis.EdgeHistogram edge = new net.semanticmetadata.lire.imageanalysis.EdgeHistogram();
        
        public EdgeHistogram()
        {
        }

        public double[] ExtractHistogram(LocalBitmap input)
        {
            
            var image = new BufferedImage(input.Bitmap);
            edge.extract(image);
            return edge.getDoubleHistogram();
        }
        

        public override string ToString()
        {
            return "EdgeHistogram";
        }


    }
}
