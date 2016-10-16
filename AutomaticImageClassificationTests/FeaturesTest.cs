using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Cluster.KDTree;
using java.awt.image;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class FeaturesTest
    {
        //Passed Phow extraction 
        [TestMethod]
        public void phow_test()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            IFeatures phow = new Phow();
            ICluster cluster = new VlFeatKmeans(10000);
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                colors.AddRange(phow.ExtractDescriptors(image));
            }
            List<double[]> vocab = cluster.CreateClusters(colors, numOfClusters);
            phow = new Phow(vocab);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                double[] vector = phow.ExtractHistogram(image);
            }

        }

        //pass test
        [TestMethod]
        public void CanUseAccordSurf()
        {
            string imagePath = @"Data\einstein.jpg";

            IFeatures surf = new AccordSurf();
            List<double[]> featu = surf.ExtractDescriptors(imagePath);
        }

        //pass test
        [TestMethod]
        public void CanUseJOpenSurf()
        {
            string imagePath = @"Data\einstein.jpg";

            IFeatures surf = new JOpenSurf();
            List<double[]> featu = surf.ExtractDescriptors(imagePath);
        }

        [TestMethod]
        public void CanUseOpenCvSift()
        {
            string imagePath = @"Data\einstein.jpg";

            IFeatures sift = new OpenCvSift();
            List<double[]> featu = sift.ExtractDescriptors(imagePath);
        }

        [TestMethod]
        public void CanUseOpenCvSurf()
        {
            string imagePath = @"Data\einstein.jpg";

            IFeatures surf = new OpenCvSurf();
            List<double[]> featu = surf.ExtractDescriptors(imagePath);
        }

        [TestMethod]
        public void CanUseLireAutoColorCorrelogram()
        {
            string imagePath = @"Data\einstein.jpg";

            IFeatures colorCorrelo = new ColorCorrelogram();
            double[] featu = colorCorrelo.ExtractHistogram(imagePath);
            Files.WriteFile(@"C:\Users\l.valavanis\Desktop\colorCorre.txt", new List<double[]> { featu });
        }

        [TestMethod]
        public void CanUseJavaSift()
        {
            IFeatures javasift = new JavaSift();

            string imagePath = @"Data\einstein.jpg";
            int clusterNum = 10;

            List<double[]> centroids = javasift.ExtractDescriptors(imagePath);
            IKdTree kdtree = new AccordKdTree(centroids);

            //Arrays.GetSubsetOfFeatures(ref centroids,10);
            //kdtree.CreateTree(centroids);
            //javasift = new JavaSift(kdtree, clusterNum);

            //double[] featu = javasift.ExtractHistogram(imagePath);
            //Files.WriteFile(@"C:\Users\l.valavanis\Desktop\sift.txt", new List<double[]> { featu });
            Files.WriteFile(@"C:\Users\l.valavanis\Desktop\sift.txt", centroids);
        }

        //TODO THEMA OTAN GRAFW DECIMAL KAI DIAVAZW ME TELEIES KAI KOMMATA
        //TODO cannot read Dictionary
        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        [TestMethod]
        public void CanCreateboc()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            ICluster cluster = new LireKmeans();

            const bool isDistinctColors = true;
            const int numOfcolors = 512;
            const int sampleImages = 10;
            var colorspace = ColorConversion.ColorSpace.RGB;
            const string paleteFile = @"Data\Palettes\boc_paleteLire.txt";
            const string trainFile = @"Data\Features\boc_Lire_Accord_train.txt";
            const string testFile = @"Data\Features\boc_Lire_Accord_test.txt";
            const string trainLabelsFile = @"Data\Features\boc_labels_train.txt";
            const string testLabelsFile = @"Data\Features\boc_labels_test.txt";


            //const string baseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
            //var trainPath = Path.Combine(baseFolder, "Train");
            //var testPath = Path.Combine(baseFolder, "Test");

            const string baseFolder = @"C:\Users\leonidas\Desktop\libsvm\databases\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //Create Palette
            var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);
            IFeatures boc = new Boc(colorspace);

            var colors = new List<double[]>();
            foreach (var img in sampleImgs)
            {
                colors.AddRange(boc.ExtractDescriptors(img));
            }
            if (isDistinctColors)
            {
                Arrays.GetDistinctColors(ref colors);
            }

            List<double[]> centers = cluster.CreateClusters(colors, numOfcolors);
            centers = centers.OrderByDescending(b => b[0]).ToList();

            Files.WriteFile(paleteFile, centers);

            //Create Kd-Tree
            IKdTree tree = new AccordKdTree(centers);
            tree.CreateTree(centers);

            int[][] palette = Arrays.ConvertDoubleListToIntArray(ref centers);
            boc = new Boc(colorspace, palette, tree);

            //Feature extraction BOC
            var trainFeatures = new List<double[]>();
            var trainLabels = new List<double>();

            Dictionary<string, int> mapping = Files.MapCategoriesToNumbers(trainPath);
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                double[] vec = boc.ExtractHistogram(train);

                int cat;
                mapping.TryGetValue(train.Split('\\')[train.Split('\\').Length - 2], out cat);

                trainLabels.Add(cat);
                trainFeatures.Add(vec);
            }
            Files.WriteFile(trainFile, trainFeatures);
            Files.WriteFile(trainLabelsFile, trainLabels);

            var testFeatures = new List<double[]>();
            var testLabels = new List<double>();
            foreach (var test in Files.GetFilesFrom(testPath))
            {
                double[] vec = boc.ExtractHistogram(test);

                int cat;
                mapping.TryGetValue(test.Split('\\')[test.Split('\\').Length - 2], out cat);

                testLabels.Add(cat);
                testFeatures.Add(vec);
            }
            Files.WriteFile(testFile, testFeatures);
            Files.WriteFile(testLabelsFile, testLabels);

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed.Minutes + " minutes and " + stopwatch.Elapsed.Seconds + " seconds ");


        }

        [TestMethod]
        public void CanCreateLboc()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            ICluster cluster = new LireKmeans();

            const bool isDistinctColors = true;
            const int numOfcolors = 50;
            const int numOfVisualWords = 1024;

            const int sampleImages = 10;
            ColorConversion.ColorSpace colorspace = ColorConversion.ColorSpace.RGB;
            const string paleteFile = @"Data\Palettes\lboc_paleteLire.txt";
            const string dictionaryFile = @"Data\Dictionaries\lboc_dictionaryLire.txt";

            string trainFile = @"Data\Features\lboc_" + numOfcolors + "_" + numOfVisualWords + "_Lire_AccordKDTree_train.txt";
            string testFile = @"Data\Features\lboc_" + numOfcolors + "_" + numOfVisualWords + "_Lire_AccordKDTree_test.txt";

            //const string baseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
            //string trainPath = Path.Combine(baseFolder, "Train");
            //string testPath = Path.Combine(baseFolder, "Test");

            const string baseFolder = @"C:\Users\leonidas\Desktop\libsvm\databases\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            #region cluster boc

            var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);
            IFeatures _boc = new Boc(colorspace);

            List<double[]> bocColors = new List<double[]>();
            foreach (var img in sampleImgs)
            {
                bocColors.AddRange(_boc.ExtractDescriptors(img));
            }
            if (isDistinctColors)
            {
                Arrays.GetDistinctColors(ref bocColors);
            }

            List<double[]> bocCenters = cluster.CreateClusters(bocColors, numOfcolors);
            bocCenters = bocCenters.OrderByDescending(b => b[0]).ToList();

            Files.WriteFile(paleteFile, bocCenters);

            IKdTree boctree = new AccordKdTree(bocCenters);
            boctree.CreateTree(bocCenters);
            int[][] palette = Arrays.ConvertDoubleListToIntArray(ref bocCenters);

            #endregion


            #region cluster lboc and create dictionary

            IFeatures lboc = new Lboc(colorspace, palette, boctree);

            List<double[]> lbocColors = new List<double[]>();
            foreach (var img in sampleImgs)
            {
                lbocColors.AddRange(lboc.ExtractDescriptors(img));
            }
            if (isDistinctColors)
            {
                Arrays.GetDistinctColors(ref lbocColors);
            }

            List<double[]> lbocCenters = cluster.CreateClusters(lbocColors, numOfVisualWords);
            lbocCenters = lbocCenters.OrderByDescending(b => b[0]).ToList();

            Files.WriteFile(dictionaryFile, lbocCenters);

            IKdTree lboctree = new AccordKdTree(lbocCenters);
            lboctree.CreateTree(lbocCenters);


            #endregion

            lboc = new Lboc(lbocCenters, colorspace, palette, boctree, lboctree);

            //Feature extraction BOC
            List<double[]> trainFeatures = new List<double[]>();
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                double[] vec = lboc.ExtractHistogram(train);
                trainFeatures.Add(vec);
            }
            Files.WriteFile(trainFile, trainFeatures);

            List<double[]> testFeatures = new List<double[]>();

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                double[] vec = lboc.ExtractHistogram(test);
                testFeatures.Add(vec);
            }
            Files.WriteFile(testFile, testFeatures);

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed.Minutes + " minutes and " + stopwatch.Elapsed.Seconds + " seconds ");
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

        [TestMethod]
        public void CanCreateTfidf()
        {
            bool removeStopwords = false;
            bool UseTfidf = true;

            string file = @"Data\testFiguresTest.xml";
            Figures images = XmlFiguresReader.ReadXml<Figures>(file);

            #region train set 

            TfidfApproach trainTfidfApproach = new TfidfApproach(removeStopwords, UseTfidf);
            trainTfidfApproach.ParseData(images.FigureList, true);

            TfIdf tfidf = new TfIdf(trainTfidfApproach);
            foreach (var image in images.FigureList.Select(a => a.Caption))
            {
                double[] vec = tfidf.ExtractHistogram(image);
            }

            #endregion

            string testfile = @"Data\testFiguresTest.xml";
            Figures testImages = XmlFiguresReader.ReadXml<Figures>(testfile);

            #region test set 

            TfidfApproach testTfidfApproach = new TfidfApproach(removeStopwords, UseTfidf);
            testTfidfApproach.ParseData(testImages.FigureList, false);

            TfIdf testtTfIdf = new TfIdf(testTfidfApproach);
            foreach (var image in testImages.FigureList.Select(a => a.Caption))
            {
                double[] vec = testtTfIdf.ExtractHistogram(image);
            }

            #endregion

        }

        [TestMethod]
        public void CanCreateEmbeddings()
        {


        }
    }
}
