using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using boclibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Utilities;
using java.awt.image;
using net.sf.javaml.core.kdtree;


namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class UnitTest1
    {
        //TODO THEMA OTAN GRAFW DECIMAL KAI DIAVAZW ME TELEIES KAI KOMMATA
        //TODO cannot read Dictionary
        [TestMethod]
        public void CanCreateLBOC()
        {
            int noOfVWords = 10;
            int NumOfcolors = 10;
            int resize = 16;
            int patches = 16;
            ColorConversion.ColorSpace colorspace = ColorConversion.ColorSpace.RGB;
            string paleteFile = @"C:\Users\l.valavanis\Desktop\palete.txt";
            string dictionaryFile = @"C:\Users\l.valavanis\Desktop\dictionary.txt";
            string trainFile= @"C:\Users\l.valavanis\Desktop\train.txt";
            string testFile = @"C:\Users\l.valavanis\Desktop\test.txt";

            string baseFolder = @"C:\Users\l.valavanis\Desktop\Leo Files\DBs\Clef2013\Compound";
            string trainPath = Path.Combine(baseFolder, "Train");
            string testPath = Path.Combine(baseFolder, "Test");

            //Create Palette
            var sampleImgs = Files.GetFilesFrom(trainPath, 2);
            int[][] palete = BoCLibrary.createPalette(paleteFile, sampleImgs, resize, patches, NumOfcolors, true, colorspace);
            List<double[]> colors = AutomaticImageClassification.Utilities.Arrays.ConvertIntArrayToDoubleList(palete);
            Files.WriteFile(paleteFile, colors);

            //Create Dictionary
            KDTree tree = AutomaticImageClassification.Utilities.KDTreeImplementation.createTree(colors);
            Clusterer dictionary = BoCLibrary.createDictionary(dictionaryFile, sampleImgs, resize, patches, noOfVWords, palete,
                colorspace, tree);
        

            //Feature extraction LBOC
            List<double[]> trainFeatures = new List<double[]>();
            foreach (string train in Files.GetFilesFrom(trainPath))
            {
                BufferedImage image = IO.getImage(train);
                double[] vec = BoCLibrary.getLBoC(image, palete, dictionary, resize, patches, colorspace, tree);
                trainFeatures.Add(vec);
            }
            Files.WriteFile(trainFile, trainFeatures);

            List<double[]> testFeatures = new List<double[]>();
            foreach (string test in Files.GetFilesFrom(testPath))
            {
                BufferedImage image = IO.getImage(test);
                double[] vec = BoCLibrary.getLBoC(image, palete, dictionary, resize, patches, colorspace, tree);
                testFeatures.Add(vec);
            }
            Files.WriteFile(testFile, testFeatures);


        }

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
            double[] lines = new [] {012 , 1.1 ,2.2};
            double[] lines2 = new[] { 0.0, 5.1, 2.1 };
            features.Add(lines);
            features.Add(lines2);

            string fileToWrite = @"C:\Users\l.valavanis\Desktop\test.txt";
            Files.WriteFile(fileToWrite, features);
            
        }

        [TestMethod]
        public void CanReadFile()
        {
            string path = @"C:\Users\l.valavanis\Desktop\dictionary.txt";
            var result = Files.ReadFileToArray(path);
            string fileToWrite = @"C:\Users\l.valavanis\Desktop\testpalete.txt";
            Files.WriteFile(fileToWrite, result.ToList());

        }

    }
}
