using System.Drawing;
using System.IO;

namespace AutomaticImageClassification.Utilities
{
    public class LocalBitmap
    {
        public Bitmap Bitmap { get; set; }
        public string Path { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }

        public LocalBitmap(string path)
        {
            Path = path;
            
            //if path does not exist then it is textual method
            if (!File.Exists(path)) return;

            Bitmap = new Bitmap(path);
            if (Bitmap.Height > 480)
            {
                Bitmap = ImageProcessing.ResizeImage(Bitmap, 480);
            }

            ImageHeight = Bitmap.Height;
            ImageWidth = Bitmap.Width;
        }

        public LocalBitmap(string path, Bitmap bitmap)
        {
            Path = path;
            Bitmap = bitmap;
            ImageHeight = Bitmap.Height;
            ImageWidth = Bitmap.Width;
        }

        public LocalBitmap(string path, Bitmap bitmap,int height, int width)
        {
            Path = path;
            Bitmap = ImageProcessing.ResizeImage(bitmap,height,width);
            ImageHeight = height;
            ImageWidth = width;
        }

        public LocalBitmap(string path, int height, int width)
        {
            Path = path;
            Bitmap = ImageProcessing.ResizeImage(new Bitmap(path), height, width);
            ImageHeight = height;
            ImageWidth = width;
        }

        public LocalBitmap(string path, int height)
        {
            Path = path;
            Bitmap = ImageProcessing.ResizeImage(new Bitmap(path), height);
            ImageHeight = Bitmap.Height;
            ImageWidth = Bitmap.Width;
        }

        public LocalBitmap(string path, Bitmap bitmap, int height)
        {
            Path = path;
            Bitmap = ImageProcessing.ResizeImage(bitmap, height);
            ImageHeight = Bitmap.Height;
            ImageWidth = Bitmap.Width;
        }


    }
}
