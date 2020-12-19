using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutomaticImageClassification.FusionTypes;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class UtilitiesTest
    {

        [TestMethod]
        public void UseHomogeneousKernelMapChi2KernelRectangular()
        {
            HomogeneousKernelMap.KernelType type = HomogeneousKernelMap.KernelType.Chi2;
            HomogeneousKernelMap.WindowType wintype = HomogeneousKernelMap.WindowType.Rectangular;
            int gamma = 1;

            HomogeneousKernelMap map = new HomogeneousKernelMap(type, gamma, wintype);

            double[] arr = { 1, 2, 3 };
            var results = map.Evaluate(arr);

            double[] correct = { 0.809014194474343, 0.5820673411764811, 0.0,
                1.1441188459779605, 0.7019632059576546, 0.42994469040815864,
                1.4012536888739706, 0.6493091695582675, 0.7712358726876802 };


            CollectionAssert.AreEqual(results, correct);


            double[] arr1 = { 10.5, 3, 7, 4.3, 5, 8.6 };
            results = map.Evaluate(arr1);

            double[] correct1 = { 2.621505607859763, -0.5455856866515175, 1.8054809193108916,
                1.4012536888739706, 0.6493091695582675, 0.7712358726876802,
                2.1404503657003566, 0.04312825624040376, 1.5394014035213393,
                1.6774960443700697, 0.4858256223132969, 1.104600984010011,
                1.8090107336068657, 0.37813144222832745, 1.2454029712183985,
                2.372337656775372, -0.23001740047587285, 1.6909854327646632 };


            CollectionAssert.AreEqual(results, correct1);




        }

        [TestMethod]
        public void UseHomogeneousKernelMapKernelIntersectionRectangular()
        {
            HomogeneousKernelMap.KernelType type = HomogeneousKernelMap.KernelType.Intersection;
            HomogeneousKernelMap.WindowType wintype = HomogeneousKernelMap.WindowType.Rectangular;
            int gamma = 1;

            HomogeneousKernelMap map = new HomogeneousKernelMap(type, gamma, wintype);

            double[] arr = { 1, 2, 3 };
            var results = map.Evaluate(arr);

            double[] correct = { 0.7594333530689384, 0.5278956804833388, 0.0,
                1.074000947628568, 0.4701891332918913, 0.5798878149091896,
                1.3153771524777953, 0.14656617139247488, 0.9025186456922946 };


            CollectionAssert.AreEqual(results, correct);


            double[] arr1 = { 10.5, 3, 7, 4.3, 5, 8.6 };
            results = map.Evaluate(arr1);

            double[] correct1 = { 2.4608453194811077, -1.6974079880723023, 0.21185264099013534,
                1.3153771524777953, 0.14656617139247488, 0.9025186456922946,
                2.0092717895483223, -1.1165199417805645, 0.8391067666972395,
                1.5746898564166596, -0.32435582326452495, 1.04493204253342,
                1.698144601842745, -0.5602447414010686, 1.0389875249959868,
                2.2269477514757816, -1.4367458767569214, 0.5744045898313581 };


            CollectionAssert.AreEqual(results, correct1);




        }

        [TestMethod]
        public void UseHomogeneousKernelMapKernelJensonRectangular()
        {
            HomogeneousKernelMap.KernelType type = HomogeneousKernelMap.KernelType.JensonShannon;
            HomogeneousKernelMap.WindowType wintype = HomogeneousKernelMap.WindowType.Rectangular;
            int gamma = 1;

            HomogeneousKernelMap map = new HomogeneousKernelMap(type, gamma, wintype);

            double[] arr = { 1, 2, 3 };
            var results = map.Evaluate(arr);

            double[] correct = { 0.8320999532621108, 0.5599542343920604, 0.0,
                1.1767670391532956, 0.7270661505069388, 0.31380296686650105,
                1.441239396025664, 0.7745605434425236, 0.5836970090576256};


            CollectionAssert.AreEqual(results, correct);


            double[] arr1 = { 10.5, 3, 7, 4.3, 5, 8.6 };
            results = map.Evaluate(arr1);

            double[] correct1 = { 2.6963120161245198, 0.3401817951343008, 1.78228453528105,
                1.441239396025664, 0.7745605434425236, 0.5836970090576256,
                2.2015295422800145, 0.6134735308779227, 1.3485145305881445,
                1.7253645110944693, 0.759732328035786, 0.8778607707617091,
                1.8606320595684778, 0.7323226231396441, 1.0156019391010744,
                2.440033891627023, 0.4945053145290444, 1.5656085015458256};


            CollectionAssert.AreEqual(results, correct1);




        }


        [TestMethod]
        public void UseHomogeneousKernelMapChi2KernelUniform()
        {
            HomogeneousKernelMap.KernelType type = HomogeneousKernelMap.KernelType.Chi2;
            HomogeneousKernelMap.WindowType wintype = HomogeneousKernelMap.WindowType.Uniform;
            int gamma = 1;

            HomogeneousKernelMap map = new HomogeneousKernelMap(type, gamma, wintype);

            double[] arr = { 1, 2, 3 };
            var results = map.Evaluate(arr);

            double[] correct = {0.8128299092201114, 0.5713744403269218, 0.0,
                 1.1495150815215733, 0.7247825055466717, 0.3572503628707671,
                 1.407862700680831, 0.7401974041346449, 0.6568972949320483 };


            CollectionAssert.AreEqual(results, correct);


            double[] arr1 = { 10.5, 3, 7, 4.3, 5, 8.6 };
            results = map.Evaluate(arr1);

            double[] correct1 = {2.6338699367829714, 0.031955474406161546, 1.8511890054162377,
                             1.407862700680831, 0.7401974041346449, 0.6568972949320483,
                              2.150545797991622, 0.42524478247961806, 1.4506716142484433,
                               1.6854079530067654, 0.6758081557086156, 0.9729268397876399,
                                1.8175429311611522, 0.6208686840535625, 1.1166314667141137,
                                 2.3835267852736437, 0.24893563968514265, 1.6566947953975826
                         };


            CollectionAssert.AreEqual(results, correct1);




        }

        [TestMethod]
        public void UseHomogeneousKernelMapKernelIntersectionUniform()
        {
            HomogeneousKernelMap.KernelType type = HomogeneousKernelMap.KernelType.Intersection;
            HomogeneousKernelMap.WindowType wintype = HomogeneousKernelMap.WindowType.Uniform;
            int gamma = 1;

            HomogeneousKernelMap map = new HomogeneousKernelMap(type, gamma, wintype);

            double[] arr = { 1, 2, 3 };
            var results = map.Evaluate(arr);

            double[] correct = {0.7559866050224623, 0.5202139372062631, 0.0,
                            1.0691265097951583, 0.597796562965758,
                            .4288173856475622, 1.30940720974041,
                            0.4972017952599525, 0.7514372869250006 };


            CollectionAssert.AreEqual(results, correct);


            double[] arr1 = { 10.5, 3, 7, 4.3, 5, 8.6 };
            results = map.Evaluate(arr1);

            double[] correct1 = { 2.4496765793101303, -0.8668382366583888, 1.4457275491375317,
                    1.30940720974041, 0.4972017952599525, 0.7514372869250006,
                    2.0001525513854483, -0.24114595453040763, 1.3550669399919366,
                    1.567543002562432, 0.27865120386720454, 1.0418144171097976,
                     1.6904374389095098, 0.14611975363604385, 1.154019809152144,
                      2.2168405738268344, -0.5385695820482985, 1.4268813375755047 };


            CollectionAssert.AreEqual(results, correct1);




        }

        [TestMethod]
        public void UseHomogeneousKernelMapKernelJensonUniform()
        {
            HomogeneousKernelMap.KernelType type = HomogeneousKernelMap.KernelType.JensonShannon;
            HomogeneousKernelMap.WindowType wintype = HomogeneousKernelMap.WindowType.Uniform;
            int gamma = 1;

            HomogeneousKernelMap map = new HomogeneousKernelMap(type, gamma, wintype);

            double[] arr = { 1, 2, 3 };
            var results = map.Evaluate(arr);

            double[] correct = { 0.808132309856787, 0.5720372476838785, 0.0,
                1.1428716727913646, 0.7694848476742163, 0.24969240011518581,
                 1.39972621990995, 0.8707771675490831, 0.47268061382760046};


            CollectionAssert.AreEqual(results, correct);


            double[] arr1 = { 10.5, 3, 7, 4.3, 5, 8.6 };
            results = map.Evaluate(arr1);

            double[] correct1 = { 2.6186479750936154, 0.8990261125309554, 1.6209970643816174,
            1.39972621990995, 0.8707771675490831, 0.47268061382760046,
            2.13811711831725, 0.9632896133363562, 1.1673300347384719,
            1.6756674510429745, 0.936770333488011, 0.727458226976224,
            1.807038779653699, 0.9543914643209078, 0.8516278509537651,
            2.3697516352921286, 0.9425781081138359, 1.3874490150999435};

            CollectionAssert.AreEqual(results, correct1);

        }





        [TestMethod]
        public void CanCreatePaletteImage()
        {
            string paletteFile = @"Data\palettes\Boc.txt";
            var palette = Files.ReadFileToListArrayList<double>(paletteFile);
            int[][] intPalette = Arrays.ConvertDoubleListToIntArray(ref palette);

            Bitmap paletteIm = ImageProcessing.getPaletteImg(intPalette, 512, ColorConversion.ColorSpace.RGB);

            ImageProcessing.SaveImage(paletteIm, @"Data\paletteIm.jpg");
        }



        //[TestMethod]
        //public void CanGetImagePixelRGBSameAsJava()
        //{
        //    string sourceImage = @"Data\database\dogInNeed.jpg";
        //    Bitmap image = new Bitmap(sourceImage);

        //    var img = new java.awt.image.BufferedImage(image);

        //    List<int[]> list1 = new List<int[]>();
        //    List<int[]> list2 = new List<int[]>();

        //    //java code
        //    for (var x = 0; x < img.getWidth(); x++)
        //    {
        //        for (var y = 0; y < img.getHeight(); y++)
        //        {
        //            var color = new java.awt.Color(img.getRGB(x, y));
        //            list1.Add(new int[] { color.getRed(), color.getGreen(), color.getBlue() });
        //        }
        //    }

        //    //c# code
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            System.Drawing.Color pxl = image.GetPixel(x, y);
        //            int red = pxl.R;
        //            int green = pxl.G;
        //            int blue = pxl.B;
        //            list2.Add(new int[] { red, green, blue });
        //        }
        //    }

        //    for (var i = 0; i < list1.Count; i++)
        //    {
        //        CollectionAssert.AreEqual(list1[i].ToArray(), list2[i].ToArray());
        //    }

        //}

        //[TestMethod]
        //public void CanConvertRGBtoHSVColorSpace()
        //{
        //    string sourceImage = @"Data\database\einstein.jpg";
        //    Bitmap image = new Bitmap(sourceImage);

        //    ColorConversion.ColorSpace _cs = ColorConversion.ColorSpace.HSV;

        //    AutomaticImageClassification.Utilities.ColorConversion.ColorSpace _cs1 = 
        //        AutomaticImageClassification.Utilities.ColorConversion.ColorSpace.HSV;

        //    var img = new java.awt.image.BufferedImage(image);

        //    List<int[]> list1 = new List<int[]>();
        //    List<int[]> list2 = new List<int[]>();

        //    //java code
        //    for (var x = 0; x < img.getWidth(); x++)
        //    {
        //        for (var y = 0; y < img.getHeight(); y++)
        //        {
        //            var color = new java.awt.Color(img.getRGB(x, y));
        //            int[] cl = ColorConversion.ConvertFromRGB(_cs, color.getRed(), color.getGreen(), color.getBlue());

        //            list1.Add(cl);
        //        }
        //    }

        //    //c# code
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {

        //            System.Drawing.Color pxl = image.GetPixel(x, y);

        //            int red = pxl.R;
        //            int green = pxl.G;
        //            int blue = pxl.B;

        //            int[] hsv = AutomaticImageClassification.Utilities.ColorConversion.ConvertFromRGB(_cs1, red, green, blue);

        //            list2.Add(hsv);
        //        }
        //    }

        //    for (var i = 0; i < list1.Count; i++)
        //    {
        //        CollectionAssert.AreEqual(list1[i].ToArray(), list2[i].ToArray());
        //    }

        //}

        //[TestMethod]
        //public void CanConvertRGBtoCieLabColorSpace()
        //{
        //    string sourceImage = @"Data\database\einstein.jpg";
        //    Bitmap image = new Bitmap(sourceImage);

        //    ColorConversion.ColorSpace _cs = ColorConversion.ColorSpace.CIELab;

        //    AutomaticImageClassification.Utilities.ColorConversion.ColorSpace _cs1 =
        //        AutomaticImageClassification.Utilities.ColorConversion.ColorSpace.CIELab;

        //    var img = new java.awt.image.BufferedImage(image);

        //    List<int[]> list1 = new List<int[]>();
        //    List<int[]> list2 = new List<int[]>();



        //    //c# code
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {

        //            System.Drawing.Color pxl = image.GetPixel(x, y);

        //            int red = pxl.R;
        //            int green = pxl.G;
        //            int blue = pxl.B;

        //            int[] hsv = AutomaticImageClassification.Utilities.ColorConversion.ConvertFromRGB(_cs1, red, green, blue);

        //            list2.Add(hsv);
        //        }
        //    }

        //    //for (var i = 0; i < list1.Count; i++)
        //    //{
        //    //    CollectionAssert.AreEqual(list1[i].ToArray(), list2[i].ToArray());
        //    //}

        //}

        //einstein image has a color in a subimage that has the same max value but its ok
        [TestMethod]
        public void CanGetDominantColors()
        {
            string sourceImage = @"Data\database\03432.png";
            Bitmap image = new Bitmap(sourceImage);
            //image = ImageProcessing.ResizeImage(image, 256, 256);

            ColorConversion.ColorSpace _cs = ColorConversion.ColorSpace.RGB;
            AutomaticImageClassification.Utilities.ColorConversion.ColorSpace _cs1 =
                AutomaticImageClassification.Utilities.ColorConversion.ColorSpace.RGB;

            int patches = 4;

            List<Bitmap> imgs = ImageProcessing.SplitImage(image, patches, patches);


            List<double[]> domColors = ImageProcessing.GetDominantColors(image, patches, patches, _cs1);

            //for (var i = 0; i < domColors.Length; i++)
            //{
            //    CollectionAssert.AreEqual(domColorsJava[i].ToArray(), domColors[i].ToArray());
            //}

        }

        [TestMethod]
        public void CanSplitImageIntoBlocks()
        {
            string sourceImage = @"Data\database\einstein.jpg";
            Bitmap image = new Bitmap(sourceImage);
            //image = ImageProcessing.ResizeImage(image, 256, 256);

            ColorConversion.ColorSpace _cs = ColorConversion.ColorSpace.RGB;
            AutomaticImageClassification.Utilities.ColorConversion.ColorSpace _cs1 =
                AutomaticImageClassification.Utilities.ColorConversion.ColorSpace.RGB;

            int patches = 2;

            List<Bitmap> imgs = ImageProcessing.SplitImage(image, patches, patches);

            for (int i = 0; i < imgs.Count; i++)
            {
                ImageProcessing.SaveImage(imgs[i], @"C:\Users\l.valavanis\Desktop\images\im_" + i + ".jpg");
            }
            List<double[]> domColors = ImageProcessing.GetDominantColors(image, patches, patches, _cs1);



        }




        [TestMethod]
        public void CanNormalize()
        {
            var list = new List<double[]> { new double[] { 0, 1, 1 }, new double[] { 0, 2, 2 }, new double[] { 0, 4, 5 } };
            Normalization.Normalize(ref list);

            var results = new List<double[]> {
                new double[] { 0, 0.2182, 0.1826 },
                new double[] { 0, 0.4364, 0.3651 },
                new double[] { 0, 0.8729, 0.9129 }
            };

            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Select(d => Math.Round(d, 4)).ToArray();
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }

        }

        [TestMethod]
        public void CanRenormalize()
        {
            var list = new List<double[]> { new double[] { 0, 1, 1 }, new double[] { 0, 2, 2 }, new double[] { 0, 4, 5 } };
            var sigmoid = 0.2;
            Normalization.ReNormalize(ref list, sigmoid);

            var results = new List<double[]> {
                new double[] { 0.5000, 0.5498, 0.5498 },
                new double[] { 0.5000, 0.5987, 0.5987 },
                new double[] { 0.5000, 0.6900, 0.7311 }
            };

            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Select(d => Math.Round(d, 4)).ToArray();
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }
        }

        [TestMethod]
        public void CanUseHellKernel()
        {
            var list = new List<double[]> { new double[] { 1, 2, 3 }, new double[] { 0, 1, 2 }, new double[] { 1, 2, -2 } };
            Normalization.HellKernelMapping(ref list);

            var results = new List<double[]>
            {
                new[] {1, 1.4142135623730951, 1.7320508075688772},
                new[] {0, 1, 1.4142135623730951},
                new[] {1, 1.4142135623730951, -1.4142135623730951}
            };

            for (var i = 0; i < list.Count; i++)
            {
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }
        }

        [TestMethod]
        public void CanReadAllImagesFromFolder()
        {
            var searchFolder = @"Data\Database";
            string[] files = Files.GetFilesFrom(searchFolder);

            Assert.AreEqual(files.Length, 5);

        }

        [TestMethod]
        public void CanGetSubfoldersOfPath()
        {
            var searchFolder = @"Data\Database";
            string[] subfolders = Files.GetSubFolders(searchFolder);

            Assert.AreEqual(subfolders.Length, 0);
        }

        [TestMethod]
        public void CanMapCategoriesToNumbers()
        {
            var searchFolder = @"Data";
            Dictionary<string, int> mapping = Files.MapCategoriesToNumbers(searchFolder);

            Dictionary<string, int> results = new Dictionary<string, int>();
            results.Add("Comparison", 1);
            results.Add("database", 2);
            results.Add("Dictionaries", 3);
            results.Add("Features", 4);
            results.Add("Palettes", 5);
            results.Add("test results", 6);
            results.Add("textData", 7);

            CollectionAssert.AreEquivalent(mapping, results);

        }

        [TestMethod]
        public void CanWriteFileToFolder()
        {
            List<double[]> features = new List<double[]>();
            double[] lines = { 12, 1.1, 2.2 };
            double[] lines2 = { 0.0, 5.1, 2.1 };
            features.Add(lines);
            features.Add(lines2);

            string fileToWrite = @"Data\test.txt";
            Files.WriteFile(fileToWrite, features);

            string fileToWrite_2 = @"Data\test2.txt";
            //string fileToWrite_2Binary = @"Data\test2binary.txt";

            Files.WriteFile(fileToWrite, features);

            foreach (var doublese in features)
            {
                Files.WriteAppendFile(fileToWrite_2, doublese);
                //Files.WriteAppendBinaryFile(fileToWrite_2Binary, doublese);   
            }

            List<double> categories = new List<double>();
            categories.Add(1);
            categories.Add(3);
            string fileToWrite2 = @"Data\categories.txt";
            Files.WriteFile(fileToWrite2, categories);

            //TODO
        }

        [TestMethod]
        public void CanReadFile()
        {
            List<double[]> list = new List<double[]>();

            list.Add(new double[] { 5, 2, 3 });
            list.Add(new double[] { 5, 2, 3 });
            list.Add(new double[] { 5, 1, 3 });
            list.Add(new double[] { 5, 4, 3 });
            list.Add(new double[] { 5, 4, 3 });
            list.Add(new double[] { 2, 4, 1 });
            list.Add(new double[] { 3, 2, 3 });

            string path = @"Data\test_listFile.txt";
            Files.WriteFile(path, list);

            var result = Files.ReadFileToListArrayList<double>(path);

            for (int i = 0; i < result.Count; i++)
            {
                CollectionAssert.AreEqual(list[i], result[i]);
            }
        }

        [TestMethod]
        public void TestNormalizationSquareArray()
        {
            double[,] arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            double[][] arr = Arrays.ToJaggedArray(ref arr1);

            var a = Normalization.SqrtArray(ref arr[1]);

            Assert.AreEqual(a, new double[] { 2, 2.23, 2.44 });

        }


        [TestMethod]
        public void TestNormalization()
        {
            double[,] arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            double[][] arr = Arrays.ToJaggedArray(ref arr1);

            Normalization.SqrtArray(ref arr[1]);

            var norm1 = Normalization.ComputeL1Norm(ref arr[0]);
            var norm2 = Normalization.ComputeL2Norm(ref arr[0]);

            Normalization.NormalizeArray(ref arr[0], ref norm1);
            Normalization.Tfidf(ref arr);

            Console.WriteLine(arr);
            //TODO add assert

        }

        [TestMethod]
        public void TestDistinctColors()
        {
            List<double[]> list = new List<double[]>();
            list.Add(new double[] { 5, 2, 3 });
            list.Add(new double[] { 5, 2, 3 });

            list.Add(new double[] { 5, 1, 3 });

            list.Add(new double[] { 5, 4, 3 });
            list.Add(new double[] { 5, 4, 3 });

            list.Add(new double[] { 2, 4, 1 });

            list.Add(new double[] { 3, 2, 3 });

            Arrays.GetDistinctObjects(ref list);

            Assert.AreEqual(list.Count, 5);
        }

        [TestMethod]
        public void CanReadXml()
        {
            string file = @"Data\textData\test_figures.xml";
            Figures images = XmlFiguresReader.ReadXml<Figures>(file);
        }

        [TestMethod]
        public void CanPerformEarlyFusion()
        {
            List<double[]> resultsModel1 = new List<double[]>();
            List<double[]> resultsModel2 = new List<double[]>();

            resultsModel1.Add(new double[] { 1, 2, 3 });
            resultsModel1.Add(new double[] { 1, 2, 3 });

            resultsModel2.Add(new double[] { 0, 1, 1 });
            resultsModel2.Add(new double[] { 1, 200, 1 });
            var concat = EarlyFusion.ConcatArrays(ref resultsModel1, ref resultsModel2);

            List<double[]> concatResultList = new List<double[]>();

            concatResultList.Add(new double[] { 1, 2, 3, 0, 1, 1 });
            concatResultList.Add(new double[] { 1, 2, 3, 1, 200, 1 });

            for (int i = 0; i < concatResultList.Count; i++)
            {
                CollectionAssert.AreEqual(concat[i], concatResultList[i]);
            }

        }

        [TestMethod]
        public void CanPerformLateFusion()
        {
            List<double[]> resultsModel1 = new List<double[]>();
            List<double[]> resultsModel2 = new List<double[]>();

            resultsModel1.Add(new double[] { 1, 2, 3 });
            resultsModel1.Add(new double[] { 1, 2, 3 });

            resultsModel2.Add(new double[] { 0, 1, 1 });
            resultsModel2.Add(new double[] { 1, 200, 1 });

            double weight = 0.5;
            double sigmoid = 0.2;
            var lateFusion = LateFusion.PerformLateFusion(ref resultsModel1, ref resultsModel2, weight, sigmoid);

            //var arrayElementsNum = 100;
            //double[] weights = Enumerable.Range(0, arrayElementsNum).Select(v => (double)v / arrayElementsNum).ToArray();
            //double[] sigmoids = Enumerable.Range(0, arrayElementsNum).Select(v => (double)v / arrayElementsNum).ToArray();
            //LateFusion.PerformLateFusion(ref resultsModel1, ref resultsModel2, weight, sigmoid);

            Dictionary<double, int> results = new Dictionary<double, int> { { 2, 3 }, { 101, 2 } };

            CollectionAssert.AreEqual(lateFusion, results);
        }

        [TestMethod]
        public void CanRemoveKMostFrequent()
        {
            var train = new List<double[]>
            {
                new double[] {0, 9, 1},
                new double[] {0, 0, 0},
                new double[] {0, 0, 1},
                new double[] {0, 4, 1}
            };

            var test = new List<double[]>
            {
                new double[] {0, 1, 1},
                new double[] {9, 1, 0},
                new double[] {7, 0, 1}
            };

            var kMostFrequent = 1;
            FeatureSelection.RemoveKMostFrequentFeatures(ref train, ref test, kMostFrequent);

            var correctResultTrain = new List<double[]>
            {
                new double[] {0, 9},
                new double[] {0, 0},
                new double[] {0, 0},
                new double[] {0, 4}
            };

            var correctResultTest = new List<double[]>
            {
                new double[] {0, 1},
                new double[] {9, 1},
                new double[] {7, 0}
            };
            for (int i = 0; i < correctResultTrain.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], correctResultTrain[i]);
            }
            for (int i = 0; i < correctResultTest.Count; i++)
            {
                CollectionAssert.AreEqual(test[i], correctResultTest[i]);
            }
        }

        [TestMethod]
        public void CanRemoveMostFrequestUsingThreshold()
        {
            var train = new List<double[]>
            {
                new double[] {1, 9, 1, 1, 0},
                new double[] {1, 0, 0, 1, 3},
                new double[] {1, 0, 1, 1, 4},
                new double[] {0, 4, 1, 1, 5}
            };

            var test = new List<double[]>
            {
                new double[] {0, 1, 1, 1, 2},
                new double[] {9, 1, 0, 1, 2},
                new double[] {7, 0, 1, 1, 2}
            };

            var threshold = 0.5;
            //all features ( columns ) that have more than half non zero elements are removed with threshold 0.5
            FeatureSelection.RemoveMostFrequentFeaturesUsingThreshold(ref train, ref test, threshold);

            var correctResultTrain = new List<double[]>
            {
                new double[] {9},
                new double[] {0},
                new double[] {0},
                new double[] {4}
            };

            var correctResultTest = new List<double[]>
            {
                new double[] {1},
                new double[] {1},
                new double[] {0}
            };
            for (int i = 0; i < correctResultTrain.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], correctResultTrain[i]);
            }
            for (int i = 0; i < correctResultTest.Count; i++)
            {
                CollectionAssert.AreEqual(test[i], correctResultTest[i]);
            }
        }

        [TestMethod]
        public void CanRemoveFeaturesWithInformationGainLessThanThreshold()
        {
            var train = new List<double[]>
            {
                new double[] {1,  2,  1,  0,  3,  9,  0,  1,  2},
                new double[] {0,  5,  2,  7,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1 , 0,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  0,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  1,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  0,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  4,  0,  1,  1,  1,  1 },
                new double[] {0,  0,  0,  3,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  2,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  7,  4,  0,  0,  2,  2},
                new double[] {0,  0,  0,  1,  1,  2,  4,  5,  0 }
            };


            int[] labels = { 2, 1, 3, 4, 2, 1, 2, 4, 3, 4, 1 };
            int[] categories = { 1, 2, 3, 4 };
            var threshold = 0.1;

            FeatureSelection.InformationGainThreshold(ref train, ref train, ref labels, threshold);

            var results = new List<double[]>
            {
                new double[] {1,  2,  1,  3,  9,  0,  1,  2},
                new double[] {0,  5,  2,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  0,  1,  1,  1,  1 },
                new double[] {0,  0,  0,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  4,  0,  0,  2,  2},
                new double[] {0,  0,  0,  1,  2,  4,  5,  0 }
            };

            for (int i = 0; i < train.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], results[i]);
            }
        }


        [TestMethod]
        public void CanRemoveKFeaturesWithTheLeastInformationGain()
        {
            var train = new List<double[]>
            {
                new double[] {1,  2,  1,  0,  3,  9,  0,  1,  2},
                new double[] {0,  5,  2,  7,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1 , 0,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  0,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  1,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  0,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  4,  0,  1,  1,  1,  1 },
                new double[] {0,  0,  0,  3,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  2,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  7,  4,  0,  0,  2,  2},
                new double[] {0,  0,  0,  1,  1,  2,  4,  5,  0 }
            };


            int[] labels = { 2, 1, 3, 4, 2, 1, 2, 4, 3, 4, 1 };
            int[] categories = { 1, 2, 3, 4 };
            var kFirst = 2;

            FeatureSelection.InformationGainKFirst(ref train, ref train, ref labels, kFirst);

            var results = new List<double[]>
            {
                new double[] {1,  2,  1,  3,  9,  1,  2},
                new double[] {0,  5,  2,  1,  0,  0,  1  },
                new double[] {0,  1,  1,  2,  0,  0,  1  },
                new double[] {0,  0,  1,  1,  0,  0,  1  },
                new double[] {3,  2,  5,  0,  0,  0,  2  },
                new double[] {0,  0,  2,  2,  2,  5,  1  },
                new double[] {2,  3,  0,  0,  1,  1,  1 },
                new double[] {0,  0,  0,  2,  0,  1,  2  },
                new double[] {0,  4,  3,  5,  6,  0,  3  },
                new double[] {0,  1,  0,  4,  0,  2,  2},
                new double[] {0,  0,  0,  1,  2,  5,  0 }
            };

            for (int i = 0; i < train.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], results[i]);
            }

        }

        [TestMethod]
        public void CanComputeInformationGain()
        {
            var train = new List<double[]>
            {
                new double[] {1,  2,  1,  0,  3,  9,  0,  1,  2  },
                new double[] {0,  5,  2,  7,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1 , 0,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  0,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  1,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  0,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  4,  0,  1,  1,  1,  1  },
                new double[] {0,  0,  0,  3,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  2,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  7,  4,  0,  0,  2,  2  },
                new double[] {0,  0,  0,  1,  1,  2,  4,  5,  0  }
            };

            int[] labels = { 2, 1, 3, 4, 2, 1, 2, 4, 3, 4, 1 };
            int[] categories = { 1, 2, 3, 4 };

            var ig = FeatureSelection.InformationGain(ref train, ref labels, categories);

            //correct results
            double[] resultIg = { 0.845, 0.444, 0.194, 0.012, 0.433, 0.311, 0.183, 0.242, 0.189 };

            //keep only 3 digits 
            ig = ig.Select(d => Math.Truncate(d * 1000d) / 1000d).ToArray();

            CollectionAssert.AreEqual(ig, resultIg);
        }

        [TestMethod]
        public void CanComputePearsonCorrelation()
        {
            //A_1 = [10 200 7 150];
            //A_2 = [0.001 0.450 0.007 0.200];
            //R = corrcoef(A_1, A_2)
            //matlab result = 0.9568


            var a = new double[] { 10, 200, 7, 150 };
            var b = new double[] { 0.001, 0.450, 0.007, 0.200 };

            var result = PearsonCorrelationCoefficient.Compute(ref a, ref b);

            Assert.AreEqual(result, 0.956, 0.001);
        }

        [TestMethod]
        public void CanComputePcaDimensionalityReduction()
        {
            const string trainDataPath = @"Data\Features\MkLabSurf_VlFeatEm_RandomInit_512_VlFeatKdTree_Randomized_train.txt";
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToArray();

            var pca = new PcaDimensionalityReduction(PcaDimensionalityReduction.PcaMethod.Center, true, 0.8);
            pca.ComputePca(ref trainFeat);
        }

        [TestMethod]
        public void CanResizeSaveImage()
        {
            string imagePath = @"Data\database\einstein.jpg";
            string savedImagePath = @"Data\test_ResizedImage.jpg";

            int width = 256;
            int height = 200;

            Bitmap image = new Bitmap(imagePath);
            Bitmap im = ImageProcessing.ResizeImage(image, width, height);
            ImageProcessing.SaveImage(im, savedImagePath);

            Bitmap savedImage = new Bitmap(savedImagePath);

            Assert.AreEqual(savedImage.Height, height);
            Assert.AreEqual(savedImage.Width, width);
        }

        [TestMethod]
        public void CanResizeImageFixedHeight()
        {
            string imagePath = @"Data\database\einstein.jpg";
            string savedImagePath = @"Data\test_ResizedImageFixedHeight.jpg";

            int height = 550;

            Bitmap ima = new Bitmap(imagePath);
            Bitmap resizedImage = (Bitmap)ImageProcessing.ResizeImageFixedHeight(ima, 550);
            ImageProcessing.SaveImage(resizedImage, savedImagePath);

            Bitmap savedImage = new Bitmap(savedImagePath);

            Assert.AreEqual(savedImage.Height, height);

        }

        [TestMethod]
        public void CanSplitImage()
        {
            int blokX = 1;
            int blokY = 1;
            string imagePath = @"Data\database\einstein.jpg";
            Bitmap img = new Bitmap(imagePath);
            img = ImageProcessing.ResizeImage(img, 256, 256);

            List<Image> res = new List<Image>();
            int pembagiLebar = (int)Math.Ceiling((float)img.Width / (float)blokX);
            int pembagiTinggi = (int)Math.Ceiling((float)img.Height / (float)blokY);


            for (int i = 0; i < pembagiLebar; i++)//baris
            {
                for (int j = 0; j < pembagiTinggi; j++)//kolom
                {
                    Bitmap bmp = new Bitmap(blokX, blokY);

                    using (Graphics grp = Graphics.FromImage(bmp))
                    {
                        grp.DrawImage(img, 0, 0, new Rectangle(i * blokX, j * blokY, blokX, blokY), GraphicsUnit.Pixel);
                    }

                    res.Add(bmp);
                }
            }


        }

        [TestMethod]
        public void CanGetFileNameWithoutExtension()
        {
            var name = @"C:\Users\leonidas\Downloads\test.1.2-2017.jpg";
            name = Files.GetFileNameWithoutExtension(name);
            Assert.AreEqual(name, "test.1.2-2017");
        }

        [TestMethod]
        public void CanGetExtension()
        {
            var name = @"C:\Users\leonidas\Downloads\test.1.2-2017.jpg";
            name = Files.GetExtension(name);
            Assert.AreEqual(name, ".jpg");

        }

    }
}
