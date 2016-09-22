using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using OpenCvSharp.CPlusPlus;


namespace AutomaticImageClassification.Feature
{
    public class OpenCvSift : IFeatures
    {
        private SIFT _sift = new SIFT();


        public double[] ExtractHistogram(string input)
        {
            throw new NotImplementedException();
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            Mat src = new Mat(input);
            KeyPoint[] keuPoints;
            MatOfFloat descriptors = new MatOfFloat();

            _sift.Run(src, null, out keuPoints, descriptors);
            float[,] arr = descriptors.ToRectangularArray();
            //convert to list<double[]>
            return Arrays.ToJaggedArray(ref arr)
                .ToList()
                .ConvertAll(
                        des => Array.ConvertAll(des, x => (double)x));
        }



    }
}
