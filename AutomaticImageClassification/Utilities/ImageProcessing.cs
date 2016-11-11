using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AutomaticImageClassification.Utilities
{
    public class ImageProcessing
    {

        public static Bitmap ResizeImage(string image, int height)
        {
            return ResizeImage(new Bitmap(image), height);
        }

        public static Bitmap ResizeImage(Bitmap image, int height)
        {
            return (Bitmap)ResizeImageFixedHeight(image, height);
        }

        public static Bitmap ResizeImage(string image, int width, int height)
        {
            return ResizeImage(new Bitmap(image), width, height);
        }

        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            return new Bitmap(image, new Size(width, height));
        }

        public static void SaveImage(string image, string fileToSave)
        {
            SaveImage(new Bitmap(image),fileToSave );
        }

        public static void SaveImage(Bitmap image, string fileToSave)
        {
            image.Save(fileToSave);
        }

        public static Image ResizeImageFixedHeight(Image imgToResize, int height)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = (height / (float)sourceHeight);

            int destWidth = (int)Math.Ceiling(sourceWidth * nPercent);
            int destHeight = (int)Math.Ceiling(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        public static Image ResizeImageFixedWidth(Image imgToResize, int width)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = (width / (float)sourceWidth);

            int destWidth = (int)Math.Ceiling(sourceWidth * nPercent);
            int destHeight = (int)Math.Ceiling(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }


    }
}
