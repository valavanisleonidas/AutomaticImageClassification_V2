using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Utilities;
using java.awt.image;
using Cluster;
using Feature;
using ModelLibraries.BocLibrary;
using net.sf.javaml.core.kdtree;
using Utilities;

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
            ICluster cluster = new Lire_Kmeans();

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
        public void matlab()
        {
            ////liblinear.LibLinear svm = new LibLinear();
            ////svm.applyKernelMapping()
            //int[,] _arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }; //Matrix 1
            //MWNumericArray arr1 = new MWNumericArray(5);
            //MWNumericArray arr2 = new MWNumericArray(5);

            //MatlabAPI_DLL.Utils util = new Utils();
            //Object[] result = util.PearsonCorrelationCoefficient(1,arr1, arr2);
            

            //double output = ( (MWNumericArray)result[0]).ToScalarDouble();


        }

        [TestMethod]
        public void TestNormalization()
        {
            double[,] arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            double[][] arr = Arrays.ToJaggedArray(ref arr1);

            //FeatureNormalization.SqrtArray(ref arr[1]);
            //var norm1 =FeatureNormalization.ComputeL1Norm(ref arr[0]);
            //var norm2 = FeatureNormalization.ComputeL2Norm(ref arr[0]);
            //FeatureNormalization.NormalizeArray(ref arr[0],ref norm1);

            FeatureNormalization.TFIDF(ref arr);

            Console.WriteLine(arr);

        }

        [TestMethod]
        public void CanCreateBovw()
        {
            string baseFolder = @"C:\Users\l.valavanis\Desktop\Leo Files\DBs\Clef2013\Compound";
            string trainPath = Path.Combine(baseFolder, "Train");
            string testPath = Path.Combine(baseFolder, "Test");

            string trainFile = @"C:\Users\l.valavanis\Desktop\train.txt";
            string testFile = @"C:\Users\l.valavanis\Desktop\test.txt";
            int clusterNum = 10;
            string filename = @"C:\Users\l.valavanis\Desktop\clusters.txt";


            ICluster cluster = new Lire_Kmeans(100000);

            IFeatures feature = new Phow();
            //Clusteriiiiiiiiiiing
            
            var sampleImgs = Files.GetFilesFrom(trainPath, 2);

            var clusters = new java.util.ArrayList();

            
            for (int i = 0; i < sampleImgs.Length; i++)
            {
                clusters.addAll(feature.extractDescriptors(sampleImgs[i]));
            }

            ICluster kmeans = new Lire_Kmeans();
            var finalClusters = kmeans.CreateClusters(clusters,clusterNum);
            KDTree tree = Utilities.KDTreeImplementation.createTree( finalClusters );

            feature = new Phow(finalClusters);
            
            //Feature extraction bovw
            List<double[]> trainFeatures = new List<double[]>();
            foreach (string train in Files.GetFilesFrom(trainPath))
            {
                double[] vec = feature.extractHistogram(train);
                trainFeatures.Add(vec);
            }
            Files.WriteFile(trainFile, trainFeatures);

            //Feature extraction bovw
            List<double[]> testFeatures = new List<double[]>();
            foreach (string test in Files.GetFilesFrom(testPath))
            {
                double[] vec = feature.extractHistogram(test);
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
