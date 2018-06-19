using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Cluster.EM;
using AutomaticImageClassification.Cluster.GaussianMixtureModel;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Cluster.Kmeans;
using AutomaticImageClassification.Feature.Boc;
using AutomaticImageClassification.Feature.Local;
using AutomaticImageClassification.Feature.Textual;
using AutomaticImageClassification.KDTree;

using static AutomaticImageClassification.Feature.Global.ColorCorrelogram;
using AutomaticImageClassification.Feature.Global;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class FeaturesTest
    {
        //Passed VlFeatPhow extraction 
        [TestMethod]
        public void phow_test()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            IFeatures phow = new VlFeatPhow();
            ICluster cluster = new VlFeatKmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(phow.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new VlFeatKdTree();
            tree.CreateTree(model.Means);
            phow = new VlFeatPhow(model, true);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = phow.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void MKLabVlad_test()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            IFeatures phow = new MkLabVlad();
            ICluster cluster = new VlFeatKmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(phow.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new VlFeatKdTree();
            tree.CreateTree(model.Means);
            model.Tree = tree;

            phow = new MkLabVlad(model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = phow.ExtractHistogram(bitmap);
            }

        }


        ////pass test
        //[TestMethod]
        //public void CanUseAccordSurf()
        //{
        //    string imagePath = @"Data\database\einstein.jpg";

        //    IFeatures surf = new AccordSurf();
        //    LocalBitmap bitmap = new LocalBitmap(imagePath);
        //    List<double[]> featu = surf.ExtractDescriptors(bitmap);
        //}

        //pass test
        //[TestMethod]
        //public void CanUseJOpenSurf()
        //{
        //    string imagePath = @"Data\database\einstein.jpg";

        //    IFeatures surf = new JOpenSurf();
        //    LocalBitmap bitmap = new LocalBitmap(imagePath);
        //    List<double[]> featu = surf.ExtractDescriptors(bitmap);
        //}

        [TestMethod]
        public void CanUseLireAutoColorCorrelogram()
        {
            string imagePath = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new ColorCorrelogram(ColorCorrelogramExtractionMethod.NaiveHuangAlgorithm);
            double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);


         
            Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\l.valavanis\Desktop\colorCorre.txt", new List<double[]> { featu });
        }

        //TODO THEMA OTAN GRAFW DECIMAL KAI DIAVAZW ME TELEIES KAI KOMMATA
        //TODO cannot read Dictionary
        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        [TestMethod]
        public void CanCreateboc()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            ICluster cluster = new VlFeatKmeans();

            const bool isDistinctColors = true;
            const int numOfcolors = 50;
            const int sampleImages = 2;
            var colorspace = ColorConversion.ColorSpace.RGB;

            var resizeImages = 256;
            const string paleteFile = @"Data\Palettes\boc_paleteVLFeatLmeans.txt";
            string trainFile = @"Data\Features\boc_train_VLFeatLmeans.txt";
            const string testFile = @"Data\Features\boc_test_VLFeatLmeans.txt";
            const string trainLabelsFile = @"Data\Features\boc_labels_train.txt";
            const string testLabelsFile = @"Data\Features\boc_labels_test.txt";


            const string baseFolder = @"C:\Users\l.valavanis\Desktop\Clef2013";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;

            IFeatures boc = new Boc(colorspace);


            //Create Palette
            ClusterModel model;
            List<double[]> finalClusters;
            if (File.Exists(paleteFile))
            {
                #region read Clusters from File

                finalClusters = Files.ReadFileToListArrayList<double>(paleteFile);
                model = new KmeansModel(finalClusters);

                #endregion
            }
            else
            {
                #region Cluster

                var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);

                var colors = new List<double[]>();
                foreach (var image in sampleImgs)
                {
                    LocalBitmap bitmap = new LocalBitmap(image, resizeImages, resizeImages);
                    colors.AddRange(boc.ExtractDescriptors(bitmap));
                    //.OrderBy(x => Guid.NewGuid()).Take(clusterFeaturesPerImage));
                }
                if (isDistinctColors)
                {
                    Arrays.GetDistinctObjects(ref colors);
                }

                sampleImgs = null;
                model = cluster.CreateClusters(colors, numOfcolors);
                colors.Clear();
                finalClusters = model.Means;
                finalClusters = finalClusters.OrderByDescending(b => b[0]).ToList();
                
                Files.WriteFile(paleteFile, finalClusters);

                #endregion
            }


            //Create Kd-Tree
            IKdTree tree = new VlFeatKdTree();

            tree.CreateTree(finalClusters);
            model.Tree = tree;

            //int[][] palette = Arrays.ConvertDoubleListToIntArray(ref centers);
            boc = new Boc(colorspace, model);

            //Feature extraction BOC
            var trainFeatures = new List<double[]>();
            var trainLabels = new List<double>();

            Dictionary<string, int> mapping = Files.MapCategoriesToNumbers(trainPath);
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train, resizeImages, resizeImages);
                double[] vec = boc.ExtractHistogram(bitmap);

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
                LocalBitmap bitmap = new LocalBitmap(test, resizeImages, resizeImages);
                double[] vec = boc.ExtractHistogram(bitmap);

                int cat;
                mapping.TryGetValue(test.Split('\\')[test.Split('\\').Length - 2], out cat);

                testLabels.Add(cat);
                testFeatures.Add(vec);
            }
            Files.WriteFile(testFile, testFeatures);
            Files.WriteFile(testLabelsFile, testLabels);

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void CanCreateLboc()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            ICluster cluster = new LireKmeans();

            var resizeImages = 256;
            const bool isDistinctColors = true;
            const int numOfcolors = 50;
            const int numOfVisualWords = 1024;

            const int sampleImages = 2;
            ColorConversion.ColorSpace colorspace = ColorConversion.ColorSpace.RGB;
            const string paleteFile = @"Data\Palettes\lboc_paleteLire1.txt";
            const string dictionaryFile = @"Data\Dictionaries\lboc_dictionaryLire.txt";

            string trainFile = @"Data\Features\lboc_" + numOfcolors + "_" + numOfVisualWords + "_Lire_AccordKDTree_train.txt";
            string testFile = @"Data\Features\lboc_" + numOfcolors + "_" + numOfVisualWords + "_Lire_AccordKDTree_test.txt";

            const string baseFolder = @"C:\Users\l.valavanis\Desktop\Clef2013";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;

            IFeatures boc = new Boc(colorspace);

            #region cluster boc



            //Create Palette
            ClusterModel model;
            List<double[]> finalClusters;
            var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);

            if (File.Exists(paleteFile))
            {
                #region read Clusters from File

                finalClusters = Files.ReadFileToListArrayList<double>(paleteFile);
                model = new KmeansModel(finalClusters);

                #endregion
            }
            else
            {
                #region Cluster


                var colors = new List<double[]>();
                foreach (var image in sampleImgs)
                {
                    LocalBitmap bitmap = new LocalBitmap(image, resizeImages, resizeImages);
                    colors.AddRange(boc.ExtractDescriptors(bitmap));
                    //.OrderBy(x => Guid.NewGuid()).Take(clusterFeaturesPerImage));
                }
                if (isDistinctColors)
                {
                    Arrays.GetDistinctObjects(ref colors);
                }

                sampleImgs = null;
                model = cluster.CreateClusters(colors, numOfcolors);
                colors.Clear();
                finalClusters = model.Means;
                finalClusters = finalClusters.OrderByDescending(b => b[0]).ToList();

                Files.WriteFile(paleteFile, finalClusters);

                #endregion
            }

         
            #endregion


            #region cluster lboc and create dictionary

            IFeatures lboc = new Lboc(colorspace, model);

            ClusterModel lbocModel;
            List<double[]> lbocCenters;
            if (File.Exists(dictionaryFile))
            {
                #region read Clusters from File

                lbocCenters = Files.ReadFileToListArrayList<double>(dictionaryFile);
                lbocModel = new KmeansModel(lbocCenters);

                #endregion
            }
            else{
                List<double[]> lbocColors = new List<double[]>();
                foreach (var img in sampleImgs)
                {
                    LocalBitmap bitmap = new LocalBitmap(img, resizeImages, resizeImages);
                    lbocColors.AddRange(lboc.ExtractDescriptors(bitmap));
                }
                if (isDistinctColors)
                {
                    Arrays.GetDistinctObjects(ref lbocColors);
                }

                lbocModel = cluster.CreateClusters(lbocColors, numOfVisualWords);

                lbocCenters = lbocModel.Means;
                lbocCenters = lbocCenters.OrderByDescending(b => b[0]).ToList();

                Files.WriteFile(dictionaryFile, lbocCenters);

            }

            
            

            #endregion

            IKdTree lboctree = new VlFeatKdTree();
            lboctree.CreateTree(lbocCenters);
            lbocModel.Tree = lboctree;


            lboc = new Lboc(colorspace, model, lbocModel);

            //Feature extraction BOC
            List<double[]> trainFeatures = new List<double[]>();
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train, resizeImages, resizeImages);
                double[] vec = lboc.ExtractHistogram(bitmap);
                trainFeatures.Add(vec);
            }
            Files.WriteFile(trainFile, trainFeatures);

            List<double[]> testFeatures = new List<double[]>();

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test, resizeImages, resizeImages);
                double[] vec = lboc.ExtractHistogram(bitmap);
                testFeatures.Add(vec);
            }
            Files.WriteFile(testFile, testFeatures);

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void CanCreateBovw()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            const int clusterNum = 512;
            const int sampleImages = 2;

            ICluster cluster = new VlFeatKmeans();
            IFeatures feature = new MkLabSurf(MkLabSurf.MkLabSurfExtractionMethod.Surf);

            const string baseFolder = @"C:\Users\l.valavanis\Desktop\Clef2013";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;

            var trainFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_train.txt";
            var testFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_test.txt";
            var clustersFile = @"Data\Palettes\" + feature + "_" + clusterNum + "_clusters.txt";

            #region Cluster

            ClusterModel model;
            List<double[]> finalClusters;
            if (File.Exists(clustersFile))
            {
                #region read Clusters from File

                finalClusters = Files.ReadFileToListArrayList<double>(clustersFile);
                model = new KmeansModel(finalClusters);

                #endregion
            }
            else
            {
                #region Cluster

                var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);

                var clusters = new List<double[]>();
                foreach (var image in sampleImgs)
                {
                    LocalBitmap bitmap = new LocalBitmap(image);
                    clusters.AddRange(feature.ExtractDescriptors(bitmap));
                                    //.OrderBy(x => Guid.NewGuid()).Take(clusterFeaturesPerImage));
                }
                sampleImgs = null;
                model = cluster.CreateClusters(clusters, clusterNum);
                clusters.Clear();
                finalClusters = model.Means;


                Files.WriteFile(clustersFile, finalClusters);

                #endregion
            }

 
            IKdTree tree = new VlFeatKdTree();
            tree.CreateTree(finalClusters);
            model.Tree = tree;
            
            #endregion

            feature = new MkLabSurf(model, MkLabSurf.MkLabSurfExtractionMethod.Surf);


            #region features extraction

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);
                var vec = feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);
            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);
                var vec = feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }

            #endregion

            stopwatch.Stop();
            Console.WriteLine(" program run for : " + stopwatch.Elapsed);

        }

        [TestMethod]
        public void CanCreatePhow()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            const int clusterNum = 10;
            const int sampleImages = 10;
            const int maxNumberClusterFeatures = 100000;

            ICluster cluster = new VlFeatKmeans();
            IFeatures feature = new VlFeatPhow();
            IKdTree tree = new VlFeatKdTree();

            //const string baseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
            //var trainPath = Path.Combine(baseFolder, "Train");
            //var testPath = Path.Combine(baseFolder, "Test");

            const string baseFolder = @"Data\database";
            var trainPath = baseFolder;
            var testPath = baseFolder;

            var trainFile = @"Data\Features\" + feature + "_Tree" + tree + "_Cluster" + cluster + "_" + clusterNum + "_train.txt";
            var testFile = @"Data\Features\" + feature + "_Tree" + tree + "_Cluster" + cluster + "_" + clusterNum + "_test.txt";
            var clustersFile = @"Data\Palettes\" + feature + "_" + clusterNum + "_clusters.txt";

            ClusterModel model;
            List<double[]> finalClusters;
            if (File.Exists(clustersFile))
            {
                #region read Clusters from File

                finalClusters = Files.ReadFileToListArrayList<double>(clustersFile);
                model = new KmeansModel(finalClusters);

                #endregion
            }
            else
            {
                #region Cluster

                var sampleImgs = Files.GetFilesFrom(trainPath);
                var clusterFeaturesPerImage = maxNumberClusterFeatures / sampleImgs.Length;

                var clusters = new List<double[]>();
                foreach (var image in sampleImgs)
                {
                    LocalBitmap bitmap = new LocalBitmap(image);
                    clusters.AddRange(feature.ExtractDescriptors(bitmap).OrderBy(x => Guid.NewGuid()).Take(clusterFeaturesPerImage));
                }
                sampleImgs = null;
                model = cluster.CreateClusters(clusters, clusterNum);
                clusters.Clear();
                finalClusters = model.Means;


                Files.WriteFile(clustersFile, finalClusters);

                #endregion
            }

            //tree.CreateTree(finalClusters);
            feature = new VlFeatPhow(model);

            #region features extraction

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);
                var vec = feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);
            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);
                var vec = feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }

            #endregion

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed);

        }

        [TestMethod]
        public void CanCreateDenseSift()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            const int clusterNum = 100;
            const int sampleImages = 10;
            const int maxNumberClusterFeatures = 200000;

            ICluster cluster = new VlFeatKmeans();
            IFeatures feature = new VlFeatDenseSift();

            //const string baseFolder = @"C:\Users\leonidas\Desktop\libsvm\databases\Clef2013\Compound";
            //var trainPath = Path.Combine(baseFolder, "TrainSet");
            //var testPath = Path.Combine(baseFolder, "TestSet");

            const string baseFolder = @"Data\database";
            var trainPath = baseFolder;
            var testPath = baseFolder;

            var trainFile = @"Data\Features\" + feature + "_Lire_JavaML_" + clusterNum + "_train.txt";
            var testFile = @"Data\Features\" + feature + "_Lire_JavaML_" + clusterNum + "_test.txt";
            var clustersFile = @"Data\Palettes\" + feature + "_" + clusterNum + "_clusters.txt";

            #region Cluster

            //var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);
            var sampleImgs = Files.GetFilesFrom(trainPath);
            var clusterFeaturesPerImage = maxNumberClusterFeatures / sampleImgs.Length;

            var clusters = new List<double[]>();
            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                clusters.AddRange(feature.ExtractDescriptors(bitmap).OrderBy(x => Guid.NewGuid()).Take(clusterFeaturesPerImage));
            }
            sampleImgs = null;
            ClusterModel model = cluster.CreateClusters(clusters, clusterNum);
            clusters.Clear();
            List<double[]> finalClusters = model.Means;

            IKdTree tree = new VlFeatKdTree();
            tree.CreateTree(finalClusters);
            model.Tree = tree;

            Files.WriteFile(clustersFile, finalClusters);

            #endregion

            feature = new VlFeatDenseSift(model);

            #region features extraction

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);
                var vec = feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);
            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);
                var vec = feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }

            #endregion

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed);

        }
        

        [TestMethod]
        public void CanCreateTfidf()
        {
            bool removeStopwords = false;
            bool UseTfidf = true;

            string file = @"Data\textData\testFiguresTest.xml";
            Figures images = XmlFiguresReader.ReadXml<Figures>(file);

            #region train set 

            TfidfApproach trainTfidfApproach = new TfidfApproach(removeStopwords, UseTfidf);
            trainTfidfApproach.ParseData(images.FigureList, true);

            TfIdf tfidf = new TfIdf(trainTfidfApproach);
            foreach (var image in images.FigureList.Select(a => a.Caption))
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vec = tfidf.ExtractHistogram(bitmap);
            }

            #endregion

            string testfile = @"Data\textData\testFiguresTest.xml";
            Figures testImages = XmlFiguresReader.ReadXml<Figures>(testfile);

            #region test set 

            TfidfApproach testTfidfApproach = new TfidfApproach(removeStopwords, UseTfidf);
            testTfidfApproach.ParseData(testImages.FigureList, false);

            TfIdf testtTfIdf = new TfIdf(testTfidfApproach);
            foreach (var image in testImages.FigureList.Select(a => a.Caption))
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vec = testtTfIdf.ExtractHistogram(bitmap);
            }

            #endregion

        }

        [TestMethod]
        public void CanCreateEmbeddings()
        {


        }

        [TestMethod]
        public void CanUseVlFeatFisherVectors()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures extractor = new VlFeatFisherVector();
            ICluster cluster = new VlFeatGmm();
            List<double[]> clusters = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 5)
                {
                    break;
                }
                counter++;
                LocalBitmap bitmap = new LocalBitmap(image);
                clusters.AddRange(extractor.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(clusters, numOfClusters);

            string imagePath = @"Data\database\einstein.jpg";

            IFeatures fisher = new VlFeatFisherVector(model,new VlFeatFisherVector() );
            LocalBitmap bitmap1 = new LocalBitmap(imagePath);
            var histogram = fisher.ExtractHistogram(bitmap1);
        }


    }
}
