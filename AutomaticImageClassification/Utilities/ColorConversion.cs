﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class ColorConversion
    {
  
        public enum ColorSpace { RGB, HSV, XYZ, CIELab, YCbCr }

        public static Bitmap ConvertImage(Bitmap im, ColorSpace cs)
        {
        //    if (cs == ColorSpace.RGB) { return im; }

        //    Bitmap ret = new Bitmap(im.Width, im.Height, BufferedImage.TYPE_INT_RGB);
        //    WritableRaster wr = ret.getRaster();
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            int r = im.getRaster().getSample(i, j, 0);
        //            int g = 0, b = 0;
        //            if (im.getRaster().getNumBands() == 3)
        //            {
        //                g = im.getRaster().getSample(i, j, 1);
        //                b = im.getRaster().getSample(i, j, 2);
        //            }
        //            else
        //            {
        //                g = im.getRaster().getSample(i, j, 0);
        //                b = im.getRaster().getSample(i, j, 0);
        //            }
        //            float[] c = ColorConversion.convertFromRGBFloat(cs, r, g, b);
        //            wr.setSample(i, j, 0, c[0]);
        //            wr.setSample(i, j, 1, c[1]);
        //            wr.setSample(i, j, 2, c[2]);
        //        }
        //    }
            return im;
        }

        public static int[] ConvertFromRGB(ColorSpace cs, int r, int g, int b)
        {
            switch (cs)
            {
                case ColorSpace.YCbCr:
                    return rgb2ycbcr(r, g, b);
                case ColorSpace.XYZ:
                    return rgb2xyz(r, g, b);
                case ColorSpace.HSV:
                    return rgb2hsv(r, g, b);
                case ColorSpace.CIELab:
                    return rgb2CIELab(r, g, b);
                default:
                    return new int[] { r, g, b };
            }
        }

        public static float[] ConvertFromRGBFloat(ColorSpace cs, float r, float g, float b)
        {
            int ir = (int)Math.Round(r, MidpointRounding.AwayFromZero), 
                ig = (int)Math.Round(g, MidpointRounding.AwayFromZero), 
                ib = (int)Math.Round(b, MidpointRounding.AwayFromZero);
            switch (cs)
            {
                case ColorSpace.YCbCr:
                    return rgb2fycbcr(ir, ig, ib);
                case ColorSpace.XYZ:
                    return rgb2fxyz(ir, ig, ib);
                case ColorSpace.HSV:
                    return rgb2fhsv(ir, ig, ib);
                case ColorSpace.CIELab:
                    return rgb2fCIELab(ir, ig, ib);
                default:
                    return new float[] { r, g, b };
            }
        }

        public static int[] ConvertToRGB(ColorSpace cs, int c1, int c2, int c3)
        {
            int[] ret;
            switch (cs)
            {
                case ColorSpace.YCbCr:
                    ret = ycbcr2rgb(c1, c2, c3);
                    break;
                case ColorSpace.XYZ:
                    ret = xyz2rgb(c1, c2, c3);
                    break;
                case ColorSpace.HSV:
                    ret = hsv2rgb(c1, c2, c3);
                    break;
                case ColorSpace.CIELab:
                    ret = CIELab2rgb(c1, c2, c3);
                    break;
                default:
                    ret = new int[] { c1, c2, c3 };
                    break;
            }
            for (int c = 0; c < ret.Length; c++)
            {
                if (ret[c] < 0) { ret[c] = 0; }
                if (ret[c] > 255) { ret[c] = 255; }
            }
            return ret;
        }

        public static float[] ConvertToRGBFloat(ColorSpace cs, float c1, float c2, float c3)
        {
            int ic1 = (int)Math.Round(c1, MidpointRounding.AwayFromZero), 
                ic2 = (int)Math.Round(c2, MidpointRounding.AwayFromZero), 
                ic3 = (int)Math.Round(c3, MidpointRounding.AwayFromZero);
            int[] ret;
            switch (cs)
            {
                case ColorSpace.YCbCr:
                    ret = ycbcr2rgb(ic1, ic2, ic3);
                    break;
                case ColorSpace.XYZ:
                    ret = xyz2rgb(ic1, ic2, ic3);
                    break;
                case ColorSpace.HSV:
                    ret = hsv2rgb(ic1, ic2, ic3);
                    break;
                case ColorSpace.CIELab:
                    ret = CIELab2rgb(ic1, ic2, ic3);
                    break;
                default:
                    ret = new int[] { ic1, ic2, ic3 };
                    break;
            }
            for (int c = 0; c < ret.Length; c++)
            {
                if (ret[c] < 0) { ret[c] = 0; }
                if (ret[c] > 255) { ret[c] = 255; }
            }
            return new float[] { ret[0], ret[1], ret[0] };
        }

        /***************************************************************
            * Color Conversion routines Adapted from www.easyrgb.com
            ***************************************************************/

        //---- From RGB ----\\
        public static float[] rgb2fhsv(int r, int g, int b)
        {
            float H = 0f, S = 0f, V = 0f;
            //RGB from 0 to 255
            float var_R = (float)r / 255f;
            float var_G = (float)g / 255f;
            float var_B = (float)b / 255f;
            //Min. value of RGB
            float var_Min = Math.Min(var_R, var_G);
            var_Min = Math.Min(var_Min, var_B);
            //Max. value of RGB
            float var_Max = Math.Max(var_R, var_G);
            var_Max = Math.Max(var_Max, var_B);
            //Delta RGB value
            float del_Max = var_Max - var_Min;

            V = var_Max;

            if (del_Max == 0)
            {
                //This is gray, no chroma...            
                H = 0f; S = 0f; //HSV results from 0 to 1
            }
            else
            {
                //Cromatic data
                S = del_Max / var_Max;

                float del_R = (((var_Max - var_R) / 6f) + (del_Max / 2f)) / del_Max;
                float del_G = (((var_Max - var_G) / 6f) + (del_Max / 2f)) / del_Max;
                float del_B = (((var_Max - var_B) / 6f) + (del_Max / 2f)) / del_Max;

                if (var_R == var_Max)
                {
                    H = del_B - del_G;
                }
                else if (var_G == var_Max)
                {
                    H = (1f / 3f) + del_R - del_B;
                }
                else if (var_B == var_Max)
                {
                    H = (2f / 3f) + del_G - del_R;
                }
                if (H < 0) { H++; }
                if (H > 1) { H--; }
            }

            //int hsv[] = new int[]{Math.Round(360*H),Math.Round(100*S),Math.Round(100*V)};        
            return new float[] { H, S, V };
        }

        public static int[] rgb2hsv(int r, int g, int b)
        {
            float H = 0f, S = 0f, V = 0f;
            //RGB from 0 to 255
            float var_R = (float)r / 255f;
            float var_G = (float)g / 255f;
            float var_B = (float)b / 255f;
            //Min. value of RGB
            float var_Min = Math.Min(var_R, var_G);
            var_Min = Math.Min(var_Min, var_B);
            //Max. value of RGB
            float var_Max = Math.Max(var_R, var_G);
            var_Max = Math.Max(var_Max, var_B);
            //Delta RGB value
            float del_Max = var_Max - var_Min;

            V = var_Max;

            if (del_Max == 0)
            {
                //This is gray, no chroma...            
                H = 0f; S = 0f; //HSV results from 0 to 1
            }
            else
            {
                //Cromatic data
                S = del_Max / var_Max;

                float del_R = (((var_Max - var_R) / 6f) + (del_Max / 2f)) / del_Max;
                float del_G = (((var_Max - var_G) / 6f) + (del_Max / 2f)) / del_Max;
                float del_B = (((var_Max - var_B) / 6f) + (del_Max / 2f)) / del_Max;

                if (var_R == var_Max)
                {
                    H = del_B - del_G;
                }
                else if (var_G == var_Max)
                {
                    H = (1f / 3f) + del_R - del_B;
                }
                else if (var_B == var_Max)
                {
                    H = (2f / 3f) + del_G - del_R;
                }
                if (H < 0) { H++; }
                if (H > 1) { H--; }
            }

            int[] hsv = new int[] { (int)Math.Round(360 * H, MidpointRounding.AwayFromZero),
                                    (int)Math.Round(100 * S, MidpointRounding.AwayFromZero),
                                    (int)Math.Round(100 * V, MidpointRounding.AwayFromZero) };
            return hsv;
        }

        public static float[] rgb2fxyz(int r, int g, int b)
        {
            //RGB from 0 to 255
            float var_R = (float)r / 255f;
            float var_G = (float)g / 255f;
            float var_B = (float)b / 255f;

            if (var_R > 0.04045f)
            {
                var_R = (float)Math.Pow(((var_R + 0.055f) / 1.055f), 2.4f);
            }
            else
            {
                var_R = var_R / 12.92f;
            }
            if (var_G > 0.04045f)
            {
                var_G = (float)Math.Pow(((var_G + 0.055) / 1.055), 2.4);
            }
            else
            {
                var_G = var_G / 12.92f;
            }
            if (var_B > 0.04045f)
            {
                var_B = (float)Math.Pow(((var_B + 0.055) / 1.055), 2.4f);
            }
            else
            {
                var_B = var_B / 12.92f;
            }

            var_R = var_R * 100;
            var_G = var_G * 100;
            var_B = var_B * 100;

            //Observer. = 2Â°, IlluMinant = D65
            float X = var_R * 0.4124f + var_G * 0.3576f + var_B * 0.1805f;
            float Y = var_R * 0.2126f + var_G * 0.7152f + var_B * 0.0722f;
            float Z = var_R * 0.0193f + var_G * 0.1192f + var_B * 0.9505f;
            return new float[] { X, Y, Z };
        }

        public static int[] rgb2xyz(int r, int g, int b)
        {
            float[] fxyz = rgb2fxyz(r, g, b);
            return new int[] {  (int)Math.Round(fxyz[0], MidpointRounding.AwayFromZero),
                                (int)Math.Round(fxyz[1], MidpointRounding.AwayFromZero),
                                (int)Math.Round(fxyz[2], MidpointRounding.AwayFromZero) };
        }

        public static float[] xyz2fCIELab(float x, float y, float z)
        {
            float var_X = x / 95.047f;      //ref_X =  95.047   Observer= 2Â°, IlluMinant= D65
            float var_Y = y / 100f;         //ref_Y = 100.000
            float var_Z = z / 108.883f;     //ref_Z = 108.883

            if (var_X > 0.008856)
            {
                var_X = (float)Math.Pow(var_X, 1f / 3f);
            }
            else
            {
                var_X = (7.787f * var_X) + (16f / 116f);
            }
            if (var_Y > 0.008856)
            {
                var_Y = (float)Math.Pow(var_Y, 1f / 3f);
            }
            else
            {
                var_Y = (7.787f * var_Y) + (16f / 116f);
            }
            if (var_Z > 0.008856)
            {
                var_Z = (float)Math.Pow(var_Z, 1f / 3f);
            }
            else
            {
                var_Z = (7.787f * var_Z) + (16f / 116f);
            }

            float CIE_l = (116f * var_Y) - 16f;
            float CIE_a = 500f * (var_X - var_Y);
            float CIE_b = 200f * (var_Y - var_Z);
            return new float[] { CIE_l, CIE_a, CIE_b };
        }

        public static float[] rgb2fCIELab(int r, int g, int b)
        {
            float[] fxyz = rgb2fxyz(r, g, b);
            return xyz2fCIELab(fxyz[0], fxyz[1], fxyz[2]);
        }

        public static int[] rgb2CIELab(int r, int g, int b)
        {
            float[] lab = rgb2fCIELab(r, g, b);
            return new int[] {  (int)Math.Round(lab[0], MidpointRounding.AwayFromZero),
                                (int)Math.Round(lab[1], MidpointRounding.AwayFromZero),
                                (int)Math.Round(lab[2], MidpointRounding.AwayFromZero) };
        }

        public static int[] rgb2ycbcr(int r, int g, int b)
        {
            int y = (int)(0.299 * r + 0.587 * g + 0.114 * b);
            int cb = (int)(128 - (0.168736 * r) - 0.331264 * g + 0.5 * b);
            int cr = (int)(128 + (0.5 * r) - 0.418688 * g - 0.081312 * b);
            return new int[] { y, cb, cr };
        }

        public static float[] rgb2fycbcr(int r, int g, int b)
        {
            float y = (float)(0.299 * r + 0.587 * g + 0.114 * b);
            float cb = (float)(128 - (0.168736 * r) - 0.331264 * g + 0.5 * b);
            float cr = (float)(128 + (0.5 * r) - 0.418688 * g - 0.081312 * b);
            return new float[] { y, cb, cr };
        }

        //---- To RGB ----\\
        public static int[] xyz2rgb(int x, int y, int z)
        {
            float var_X = (float)x / 100; //X from 0 to  95.047      (Observer = 2Â°, IlluMinant = D65)
            float var_Y = (float)y / 100; //Y from 0 to 100.000
            float var_Z = (float)z / 100; //Z from 0 to 108.883

            float var_R = var_X * 3.2406f + var_Y * -1.5372f + var_Z * -0.4986f;
            float var_G = var_X * -0.9689f + var_Y * 1.8758f + var_Z * 0.0415f;
            float var_B = var_X * 0.0557f + var_Y * -0.2040f + var_Z * 1.0570f;

            if (var_R > 0.0031308f)
            {
                var_R = 1.055f * ((float)Math.Pow(var_R, (1 / 2.4))) - 0.055f;
            }
            else
            {
                var_R = 12.92f * var_R;
            }
            if (var_G > 0.0031308f)
            {
                var_G = 1.055f * ((float)Math.Pow(var_G, (1 / 2.4))) - 0.055f;
            }
            else
            {
                var_G = 12.92f * var_G;
            }
            if (var_B > 0.0031308f)
            {
                var_B = 1.055f * ((float)Math.Pow(var_B, (1 / 2.4))) - 0.055f;
            }
            else
            {
                var_B = 12.92f * var_B;
            }

            float R = var_R * 255;
            float G = var_G * 255;
            float B = var_B * 255;
            return new int[] {  (int)Math.Round(R, MidpointRounding.AwayFromZero),
                                (int)Math.Round(G, MidpointRounding.AwayFromZero),
                                (int)Math.Round(B, MidpointRounding.AwayFromZero) };
        }

        public static int[] hsv2rgb(int h, int s, int v)
        {
            float H = (float)h / 360;
            float S = (float)s / 100;
            float V = (float)v / 100;

            int R = 0, G = 0, B = 0;
            if (S == 0) //HSV from 0 to 1
            {
                R = (int)Math.Round(V * 255f, MidpointRounding.AwayFromZero);
                G = (int)Math.Round(V * 255f, MidpointRounding.AwayFromZero);
                B = (int)Math.Round(V * 255f, MidpointRounding.AwayFromZero);
            }
            else
            {
                float var_h = H * 6f;
                if (var_h == 6f) { var_h = 0; } //H must be < 1
                int var_i = (int)var_h;             //Or ... var_i = floor( var_h )
                float var_1 = V * (1 - S);
                float var_2 = V * (1 - S * (var_h - var_i));
                float var_3 = V * (1 - S * (1 - (var_h - var_i)));

                float var_r = 0, var_g = 0, var_b = 0;
                if (var_i == 0)
                {
                    var_r = V;
                    var_g = var_3;
                    var_b = var_1;
                }
                else if (var_i == 1)
                {
                    var_r = var_2;
                    var_g = V;
                    var_b = var_1;
                }
                else if (var_i == 2)
                {
                    var_r = var_1;
                    var_g = V;
                    var_b = var_3;
                }
                else if (var_i == 3)
                {
                    var_r = var_1;
                    var_g = var_2;
                    var_b = V;
                }
                else if (var_i == 4)
                {
                    var_r = var_3;
                    var_g = var_1;
                    var_b = V;
                }
                else
                {
                    var_r = V;
                    var_g = var_1;
                    var_b = var_2;
                }

                R = (int)Math.Round(var_r * 255, MidpointRounding.AwayFromZero);                  //RGB results from 0 to 255;
                G = (int)Math.Round(var_g * 255, MidpointRounding.AwayFromZero);
                B = (int)Math.Round(var_b * 255, MidpointRounding.AwayFromZero);
            }
            return new int[] { R, G, B };
        }

        public static int[] ycbcr2rgb(int y, int cb, int cr)
        {
            double R = (y + 1.402 * (cr - 128));
            double G = (y - 0.34414 * (cb - 128) - 0.71414 * (cr - 128));
            double B = (y + 1.772 * (cb - 128));
            return new int[] {  (int)Math.Round(R, MidpointRounding.AwayFromZero),
                                (int)Math.Round(G, MidpointRounding.AwayFromZero),
                                (int)Math.Round(B, MidpointRounding.AwayFromZero) };
        }

        public static float[] CIELab2fxyz(int l, int a, int b)
        {
            float var_Y = ((float)l + 16f) / 116f;
            float var_X = (float)a / 500f + var_Y;
            float var_Z = var_Y - (float)b / 200f;

            if (Math.Pow(var_Y, 3) > 0.008856)
            {
                var_Y = (float)Math.Pow(var_Y, 3);
            }
            else
            {
                var_Y = (var_Y - 16f / 116f) / 7.787f;
            }
            if (Math.Pow(var_X, 3) > 0.008856)
            {
                var_X = (float)Math.Pow(var_X, 3);
            }
            else
            {
                var_X = (var_X - 16f / 116f) / 7.787f;
            }
            if (Math.Pow(var_Z, 3) > 0.008856)
            {
                var_Z = (float)Math.Pow(var_Z, 3);
            }
            else
            {
                var_Z = (var_Z - 16f / 116f) / 7.787f;
            }

            float X = 95.047f * var_X;     //ref_X =  95.047     Observer= 2Â°, IlluMinant= D65
            float Y = 100f * var_Y;     //ref_Y = 100.000
            float Z = 108.883f * var_Z;     //ref_Z = 108.883
            return new float[] { X, Y, Z };
        }

        public static int[] CIELab2rgb(int l, int a, int b)
        {
            float[] xyz = CIELab2fxyz(l, a, b);
            return xyz2rgb( (int)Math.Round(xyz[0], MidpointRounding.AwayFromZero), 
                            (int)Math.Round(xyz[1], MidpointRounding.AwayFromZero), 
                            (int)Math.Round(xyz[2], MidpointRounding.AwayFromZero));
        }
        



    }
}
