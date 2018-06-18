using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using static AutomaticImageClassification.Utilities.ColorConversion;

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
            SaveImage(new Bitmap(image), fileToSave);
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


        //get dominant color of image into colorspace cs
        public static List<double[]> GetDominantColors(Bitmap img, int cols, int rows, ColorSpace cs)
        {
            List<double[]> ret = new List<double[]>();
            List<Bitmap> imgs = SplitImage(img, cols, rows);
            for (int i = 0; i < imgs.Count; i++)
            {
                ret.Add(GetDominantColor<double>(imgs[i], cs));
            }
            return ret;
        }

        public static List<Bitmap> SplitImage(Bitmap img, int cols, int rows)
        {
            List<Bitmap> res = new List<Bitmap>();
            int w = img.Width / cols;
            int h = img.Height / rows;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Bitmap bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                    
                    using (Graphics grp = Graphics.FromImage(bmp))
                    {
                        Rectangle dest = new Rectangle(0, 0, w, h);
                        Rectangle source = new Rectangle(w * x, h * y, w, h);
                   
                        grp.DrawImage(img, dest, source, GraphicsUnit.Pixel);
                      }
                   
                    res.Add(bmp);
                }
            }


            return res;

        }
        
        public static T[] GetDominantColor<T>(Bitmap img, ColorSpace cs)
        {
            T[] ret = new T[3];
            Dictionary<string, int> domC = new Dictionary<string, int>();
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color cl = img.GetPixel(x, y);
                    
                    int[] color = ColorConversion.ConvertFromRGB(cs, cl.R, cl.G, cl.B);
                    string key = string.Join(";", color);

                    if (domC.ContainsKey(key))
                    {
                        domC[key] += 1;
                    }
                    else
                    {
                        domC.Add(key, 1);
                    }
                }
            }
         
            string keyMax = "";
            int max = -1;
            foreach (var entry in domC.ToArray())
            {
                if (max < entry.Value)
                {
                    max = entry.Value;
                    keyMax = entry.Key;
                }
            }
          
            string[] scolors = keyMax.Split(';');

            return new T[] { (T)Convert.ChangeType(scolors[0], typeof(T)),
                             (T)Convert.ChangeType(scolors[1], typeof(T)),
                             (T)Convert.ChangeType(scolors[2], typeof(T))
                        };
        }
        

    }
}
