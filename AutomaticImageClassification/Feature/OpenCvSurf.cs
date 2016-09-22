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
    public class OpenCvSurf : IFeatures
    {
        private SURF _surf = new SURF();


        public double[] ExtractHistogram(string input)
        {
            throw new NotImplementedException();
        }


        public List<double[]> ExtractDescriptors(string input)
        {

            Mat src1 = new Mat(input);
            KeyPoint[] keypoints1;
            MatOfFloat descriptors1 = new MatOfFloat();

            _surf.Run(src1, null, out keypoints1, descriptors1);

            float[,] arr = descriptors1.ToRectangularArray();
            //convert to list<double[]>
            return Arrays.ToJaggedArray(ref arr)
                    .ToList()
                    .ConvertAll(
                            des => Array.ConvertAll(des, x => (double)x));
        }



    }
}
