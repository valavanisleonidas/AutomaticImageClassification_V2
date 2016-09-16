using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Accord.Imaging;

namespace AutomaticImageClassification.Feature
{
    public class AccordSurf : IFeatures
    {
        // Create a new SURF Features Detector using default parameters
        private SpeededUpRobustFeaturesDetector _surf =
            new SpeededUpRobustFeaturesDetector();


        public double[] ExtractHistogram(string input)
        {
            throw new NotImplementedException();
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            try
            {
                Bitmap map = new Bitmap(input);

                return _surf
                        .ProcessImage(map)
                        .ConvertAll(
                                new Converter<SpeededUpRobustFeaturePoint, double[]>(descriptor => descriptor.Descriptor));
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
