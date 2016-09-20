using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.CPlusPlus;


namespace AutomaticImageClassification.Feature
{
    public class OpenCvSift : IFeatures
    {
        private SIFT sift = new SIFT();


        public double[] ExtractHistogram(string input)
        {
            throw new NotImplementedException();
        }


        public List<double[]> ExtractDescriptors(string input)
        {
            Mat src1 = new Mat(input);
            KeyPoint[] keypoints1;
            MatOfFloat descriptors1 = new MatOfFloat();

            sift.Run(src1, null, out keypoints1, descriptors1);
            
            float[,] arr = descriptors1.ToRectangularArray();
            

            Console.WriteLine();

            return null;
        }



    }
}
