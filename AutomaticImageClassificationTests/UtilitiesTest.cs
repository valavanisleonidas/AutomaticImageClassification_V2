﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.FusionTypes;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void CanDivideWithSqrtSumPower()
        {
            var list = new List<double[]>();
            list.Add(new double[] { 0, 1, 1 });
            list.Add(new double[] { 0, 2, 2 });
            list.Add(new double[] { 0, 4, 5 });

            Normalization.array(ref list);

        }

        [TestMethod]
        public void CanUseHellKernel()
        {
            var list = new List<double[]>();
            list.Add(new double[] { 1, 2, 3 });
            list.Add(new double[] { 0, 1, 2 });
            list.Add(new double[] { 1, 2, -2 });

            Normalization.HellKernelMapping(ref list);

            var results = new List<double[]>();
            results.Add(new[] { 1, 1.4142135623730951, 1.7320508075688772 });
            results.Add(new[] { 0, 1, 1.4142135623730951 });
            results.Add(new[] { 1, 1.4142135623730951, -1.4142135623730951 });


            for (var i = 0; i < list.Count; i++)
            {
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }
        }

        [TestMethod]
        public void CanReadAllImagesFromFolder()
        {
            var searchFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";

            string[] files = Files.GetFilesFrom(searchFolder);

            Console.WriteLine(files.Length);
            foreach (var file in files)
            {
                //double category = Convert.ToDouble(file.Split('\\')[file.Split('\\').Length - 2]);
                string cat = file.Split('\\')[file.Split('\\').Length - 2] + "\\" + file.Split('\\')[file.Split('\\').Length - 1];
                Console.WriteLine(cat);
            }

        }

        [TestMethod]
        public void CanGetSubfoldersOfPath()
        {
            var searchFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound\Test";
            string[] subfolders = Files.GetSubFolders(searchFolder);
        }

        [TestMethod]
        public void CanMapCategoriesToNumbers()
        {
            var searchFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound\Test";
            Dictionary<string, int> mapping = Files.MapCategoriesToNumbers(searchFolder);
        }

        [TestMethod]
        public void CanWriteFileToFolder()
        {
            List<double[]> features = new List<double[]>();
            double[] lines = { 012, 1.1, 2.2 };
            double[] lines2 = { 0.0, 5.1, 2.1 };
            features.Add(lines);
            features.Add(lines2);

            string fileToWrite = @"Data\test.txt";
            Files.WriteFile(fileToWrite, features);

            string fileToWrite_2 = @"Data\test2.txt";
            string fileToWrite_2Binary = @"Data\test2binary.txt";

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



        }

        [TestMethod]
        public void CanReadFile()
        {
            string path = @"Data\train1.txt";
            var result = Files.ReadFileToListArrayList<double[]>(path);
            string fileToWrite = @"Data\testpalete.txt";
            Files.WriteFile(fileToWrite, result.ToList());

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
            string file = @"Data\test_figures.xml";
            Figures images = XmlFiguresReader.ReadXml<Figures>(file);
        }

        [TestMethod]
        public void CanConcatArrays()
        {
            double[] array1 = { 1, 2, 3 };
            double[] array2 = { 0, 1, 2, 3 };
            var concat = EarlyFusion.ConcatArrays(ref array1, ref array2);
            CollectionAssert.AreEqual(concat, new double[] { 1, 2, 3, 0, 1, 2, 3 });
        }


    }
}
