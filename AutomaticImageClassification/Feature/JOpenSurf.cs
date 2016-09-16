using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using OpenSURFcs;

namespace AutomaticImageClassification.Feature
{
    public class JOpenSurf : IFeatures
    {
        public double[] ExtractHistogram(string input)
        {
            throw new NotImplementedException();
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            try
            {
                // Load an Image
                Bitmap img = new Bitmap(input);

                // Create Integral Image
                IntegralImage iimg = IntegralImage.FromImage(img);

                // Extract the interest points
                List<IPoint> ipts = FastHessian.getIpoints(0.0002f, 5, 2, iimg);

                // Describe the interest points
                SurfDescriptor.DecribeInterestPoints(ipts, false, false, iimg);

                return ipts.ConvertAll(
                            new Converter<IPoint, double[]>(
                                des => Array.ConvertAll(des.descriptor, x => (double)x)));
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
