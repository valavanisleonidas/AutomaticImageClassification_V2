using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AutomaticImageClassification.Feature
{
    public class EmguSift : IFeatures
    {

        public double[] ExtractHistogram(string input)
        {
            throw new NotImplementedException();
        }

        //emguCV
        public List<double[]> ExtractDescriptors(string input)
        {

            //Bitmap image = new Bitmap(input);
            //Image<Gray, Byte> modelImage = new Image<Gray, byte>(image);
            //SIFT siftCPU = new SIFT();
            //VectorOfKeyPoint modelKeyPoints = new VectorOfKeyPoint();
            //MKeyPoint[] mKeyPoints = siftCPU.Detect(modelImage);
            //modelKeyPoints.Push(mKeyPoints);

            //IOutputArray ar = new VectorOfDouble();
            //siftCPU.Compute(modelImage, modelKeyPoints, ar);


            //Image<Bgr, Byte> result =
            //    Features2DToolbox.DrawKeypoints(modelImage, modelKeyPoints, new Bgr(Color.Red), Features2DToolbox.KeypointDrawType.DEFAULT);


            return null;
        }



    }
}
