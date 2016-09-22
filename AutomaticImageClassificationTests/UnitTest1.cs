using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomaticImageClassification.Classifiers;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.KDTree;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Utilities;


namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class UnitTest1
    {
        //TODO THEMA OTAN GRAFW DECIMAL KAI DIAVAZW ME TELEIES KAI KOMMATA
        //TODO cannot read Dictionary
        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        [TestMethod]
        public void CanCreateLboc()
        {
            //ICluster cluster = new Lire_Kmeans();

            //int noOfVWords = 10;
            //int NumOfcolors = 10;
            //int resize = 16;
            //int patches = 16;
            //ColorConversion.ColorSpace colorspace = ColorConversion.ColorSpace.RGB;
            //string paleteFile = @"C:\Users\l.valavanis\Desktop\palete.txt";
            //string dictionaryFile = @"C:\Users\l.valavanis\Desktop\dictionary.txt";
            //string trainFile= @"C:\Users\l.valavanis\Desktop\train.txt";
            //string testFile = @"C:\Users\l.valavanis\Desktop\test.txt";

            //string baseFolder = @"C:\Users\l.valavanis\Desktop\Leo Files\DBs\Clef2013\Compound";
            //string trainPath = Path.Combine(baseFolder, "Train");
            //string testPath = Path.Combine(baseFolder, "Test");

            ////Create Palette
            //var sampleImgs = Files.GetFilesFrom(trainPath, 2);
            //int[][] palete = BoCLibrary.createPalette(paleteFile, sampleImgs, resize, patches, NumOfcolors, true, colorspace);
            //List<double[]> colors = AutomaticImageClassification.Utilities.Arrays.ConvertIntArrayToDoubleList(ref palete);
            //Files.WriteFile(paleteFile, colors);

            ////Create Dictionary
            //KDTree tree = AutomaticImageClassification.Utilities.KDTreeImplementation.createTree(colors);


            ////Feature extraction LBOC
            //List<double[]> trainFeatures = new List<double[]>();
            //foreach (string train in Files.GetFilesFrom(trainPath))
            //{
            //    BufferedImage image = ImageUtility.getImage(train);
            //    double[] vec = BoCLibrary.getLBoC(image, palete, dictionary, resize, patches, colorspace, tree);
            //    trainFeatures.Add(vec);
            //}
            //Files.WriteFile(trainFile, trainFeatures);

            //List<double[]> testFeatures = new List<double[]>();
            //foreach (string test in Files.GetFilesFrom(testPath))
            //{
            //    BufferedImage image = ImageUtility.getImage(test);
            //    double[] vec = BoCLibrary.getLBoC(image, palete, dictionary, resize, patches, colorspace, tree);
            //    testFeatures.Add(vec);
            //}
            //Files.WriteFile(testFile, testFeatures);


        }

     
        [TestMethod]
        public void CanCreateBovw()
        {



            //string baseFolder = @"C:\Users\l.valavanis\Desktop\Leo Files\DBs\Clef2013\Compound";
            //string trainPath = Path.Combine(baseFolder, "Train");
            //string testPath = Path.Combine(baseFolder, "Test");

            //string trainFile = @"C:\Users\l.valavanis\Desktop\train.txt";
            //string testFile = @"C:\Users\l.valavanis\Desktop\test.txt";
            //int clusterNum = 10;
            //string filename = @"C:\Users\l.valavanis\Desktop\clusters.txt";


            //ICluster cluster = new Lire_Kmeans(100000);

            ////IFeatures feature = new Sift();
            //IFeatures feature = new Phow();
            ////Clusteriiiiiiiiiiing

            //var sampleImgs = Files.GetFilesFrom(trainPath, 2);

            //var clusters = new java.util.ArrayList();


            //for (int i = 0; i < sampleImgs.Length; i++)
            //{
            //    clusters.addAll(feature.extractDescriptors(sampleImgs[i]));
            //}

            //ICluster kmeans = new Lire_Kmeans(1000);
            //var finalClusters = kmeans.CreateClusters(clusters, clusterNum);
            //KDTree tree = Utilities.KDTreeImplementation.createTree(finalClusters);

            ////feature = new Sift(tree, clusterNum);
            //feature = new Phow(finalClusters);
  
            
            ////Feature extraction bovw
            //List<double[]> trainFeatures = new List<double[]>();
            //foreach (string train in Files.GetFilesFrom(trainPath))
            //{
            //    double[] vec = feature.extractHistogram(train);
            //    trainFeatures.Add(vec);
            //}
            //Files.WriteFile(trainFile, trainFeatures);

            ////Feature extraction bovw
            //List<double[]> testFeatures = new List<double[]>();
            //foreach (string test in Files.GetFilesFrom(testPath))
            //{
            //    double[] vec = feature.extractHistogram(test);
            //    testFeatures.Add(vec);
            //}
            //Files.WriteFile(testFile, testFeatures);


        }


    }
}
