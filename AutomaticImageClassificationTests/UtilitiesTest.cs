﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void CanReadAllImagesFromFolder()
        {
            string searchFolder = @"C:\Users\l.valavanis\Desktop\Leo Files\DBs\clef2016\subfigure\SubfigureClassificationTraining2016_Enriched";

            string[] files = Files.GetFilesFrom(searchFolder);

            Console.WriteLine(files.Length);
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }

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

        }

        [TestMethod]
        public void CanReadFile()
        {
            string path = @"Data\train1.txt";
            var result = Files.ReadFileToArray(path);
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

            Arrays.GetDistinctColors(ref list);

        }

        [TestMethod]
        public void CanReadXml()
        {
            string file = @"Data\test_figures.xml";
            Figures images = XmlReaderClass.ReadXml(file);
        }


    }
}
