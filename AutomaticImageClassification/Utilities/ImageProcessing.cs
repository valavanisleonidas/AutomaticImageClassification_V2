using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class ImageProcessing
    {
        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            return new Bitmap(image, new Size(width, height));
        }

        public static void SaveImage(Bitmap image, string fileToSave)
        {
            image.Save(fileToSave);
        }

    }
}
