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
using static AutomaticImageClassification.Feature.Local.Surf;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class FeatureExtractionTest
    {
        

        [TestMethod]
        public void CanUsePHOG()
        {
            //string imagePath = @"Data\database\einstein.jpg";
            //LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new PHOG();
            //double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);

            ////Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\leonidas\Desktop\images\colorCorre_Lire.txt", new List<double[]> { featuNaiveHuangAlgorithm });


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");


            var trainFile = @"Data\Features\" + colorCorrelo + "_train.txt";
            var testFile = @"Data\Features\" + colorCorrelo + "_test.txt";

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);

                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);

            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);

                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }

        }


        [TestMethod]
        public void CanUseEdgeHistogram()
        {
            //string imagePath = @"Data\database\einstein.jpg";
            //LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new EdgeHistogram();
            //double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);

            ////Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\leonidas\Desktop\images\colorCorre_Lire.txt", new List<double[]> { featuNaiveHuangAlgorithm });


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            trainPath = @"E:\Datasets\Image\ImageCLEF_2017\Caption\CaptionPrediction\training\CaptionPredictionTraining2017";
            testPath = @"E:\Datasets\Image\ImageCLEF_2017\Caption\ConceptDetection\test\ConceptDetectionTesting2017";

            var trainFile = @"Data\Features\" + colorCorrelo + "_train.txt";
            var testFile = @"Data\Features\" + colorCorrelo + "_test.txt";


            var trainDest = @"E:\Datasets\Image\ImageCLEF_2017\Caption\Image Features\Features\EdgeHistogram\train\";
            var testDest = @"E:\Datasets\Image\ImageCLEF_2017\Caption\Image Features\Features\EdgeHistogram\test\";

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);

                var vec = colorCorrelo.ExtractHistogram(bitmap);

                var name = Files.GetFileNameWithoutExtension(train);
                Files.WriteFile(trainDest + name + "_EH.txt", vec.ToList(),true);
                //Files.WriteAppendFile(trainFile, vec);

            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);

                var vec = colorCorrelo.ExtractHistogram(bitmap);

                var name = Files.GetFileNameWithoutExtension(test);
                Files.WriteFile(testDest + name + "_EH.txt", vec.ToList(), true);
                

                //Files.WriteAppendFile(testFile, vec);
            }

        }


        [TestMethod]
        public void CanUseJCD()
        {
            //string imagePath = @"Data\database\einstein.jpg";
            //LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new JCD();
            //double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);

            ////Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\leonidas\Desktop\images\colorCorre_Lire.txt", new List<double[]> { featuNaiveHuangAlgorithm });


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            trainPath = @"E:\Datasets\Image\ImageCLEF_2017\Caption\CaptionPrediction\training\CaptionPredictionTraining2017";
            testPath = @"E:\Datasets\Image\ImageCLEF_2017\Caption\ConceptDetection\test\ConceptDetectionTesting2017";

            var trainFile = @"Data\Features\" + colorCorrelo + "_train.txt";
            var testFile = @"Data\Features\" + colorCorrelo + "_test.txt";


            var trainDest = @"E:\Datasets\Image\ImageCLEF_2017\Caption\Image Features\Features\JCD\train\";
            var testDest = @"E:\Datasets\Image\ImageCLEF_2017\Caption\Image Features\Features\JCD\test\";

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);

                var vec = colorCorrelo.ExtractHistogram(bitmap);

                var name = Files.GetFileNameWithoutExtension(train);
                Files.WriteFile(trainDest + name + "_JCD.txt", vec.ToList(), true);
                //Files.WriteAppendFile(trainFile, vec);

            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);

                var vec = colorCorrelo.ExtractHistogram(bitmap);

                var name = Files.GetFileNameWithoutExtension(test);
                Files.WriteFile(testDest + name + "_JCD.txt", vec.ToList(), true);


                //Files.WriteAppendFile(testFile, vec);
            }

        }


        [TestMethod]
        public void CanUseFCTH()
        {
            //string imagePath = @"Data\database\einstein.jpg";
            //LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new FCTH();
            //double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);

            ////Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\leonidas\Desktop\images\colorCorre_Lire.txt", new List<double[]> { featuNaiveHuangAlgorithm });


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");


            var trainFile = @"Data\Features\" + colorCorrelo + "_train.txt";
            var testFile = @"Data\Features\" + colorCorrelo + "_test.txt";

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);

                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);

            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);

                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }

        }


        [TestMethod]
        public void CanUseCEDD()
        {
            //string imagePath = @"Data\database\einstein.jpg";
            //LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new CEDD();
            //double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);

            ////Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\leonidas\Desktop\images\colorCorre_Lire.txt", new List<double[]> { featuNaiveHuangAlgorithm });


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");


            var trainFile = @"Data\Features\" + colorCorrelo + "_train.txt";
            var testFile = @"Data\Features\" + colorCorrelo + "_test.txt";

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);

                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);
                
            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);

                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }
            

        }

        [TestMethod]
        public void CanUseBTDH()
        {
            //string imagePath = @"Data\database\einstein.jpg";
            //LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new BTDH();
            //double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);

            ////Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\leonidas\Desktop\images\colorCorre_Lire.txt", new List<double[]> { featuNaiveHuangAlgorithm });


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");


            var trainFile = @"Data\Features\" + colorCorrelo + "_train.txt";
            var testFile = @"Data\Features\" + colorCorrelo + "_test.txt";
            

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);
                var vec = colorCorrelo.ExtractHistogram(bitmap);

                Files.WriteAppendFile(trainFile, vec);
            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);
                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }


        }


        [TestMethod]
        public void CanUseLireAutoColorCorrelogram()
        {
            //string imagePath = @"Data\database\einstein.jpg";
            //LocalBitmap bitmap = new LocalBitmap(imagePath);


            IFeatures colorCorrelo = new ColorCorrelogram(ColorCorrelogramExtractionMethod.LireAlgorithm);
            //double[] featuNaiveHuangAlgorithm = colorCorrelo.ExtractHistogram(bitmap);
            
            ////Assert.AreEqual(featuNaiveHuangAlgorithm.Length, 1024);
            //Files.WriteFile(@"C:\Users\leonidas\Desktop\images\colorCorre_Lire.txt", new List<double[]> { featuNaiveHuangAlgorithm });


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            
            var trainFile = @"Data\Features\" + colorCorrelo + "_train.txt";
            var testFile = @"Data\Features\" + colorCorrelo + "_test.txt";

            
            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);
                var vec = colorCorrelo.ExtractHistogram(bitmap);
                
                Files.WriteAppendFile(trainFile, vec);
            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);
                var vec = colorCorrelo.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);
            }


        }

        //TODO THEMA OTAN GRAFW DECIMAL KAI DIAVAZW ME TELEIES KAI KOMMATA
        //TODO cannot read Dictionary
        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        [TestMethod]
        public void CanCreateboc()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            var colorspace = ColorConversion.ColorSpace.RGB;

            ILocalFeatures boc = new Boc(colorspace);
            ICluster cluster = new Kmeans();
            IKdTree tree = new KdTree();

            const bool isDistinctColors = true;
            const int numOfcolors = 512;
            const int sampleImages = 2;

            var resizeImages = 256;


            var trainFile = @"Data\Features\" + boc + "_" + cluster + "_" + numOfcolors + "_" + tree + "_train.txt";
            var testFile = @"Data\Features\" + boc + "_" + cluster + "_" + numOfcolors + "_" + tree + "_test.txt";
            var paleteFile = @"Data\Palettes\" + boc + "_" + cluster + "_" + numOfcolors + "_" + tree + "_clusters.txt";

            const string trainLabelsFile = @"Data\Features\boc_labels_train.txt";
            const string testLabelsFile = @"Data\Features\boc_labels_test.txt";


            const string baseFolder = @"C:\Users\l.valavanis\Desktop\Clef2013";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;



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

            ColorConversion.ColorSpace colorspace = ColorConversion.ColorSpace.RGB;

            ICluster cluster = new Kmeans();
            ILocalFeatures boc = new Boc(colorspace);
            IKdTree lboctree = new KdTree();

            var resizeImages = 256;
            const bool isDistinctColors = true;
            const int numOfcolors = 50;
            const int numOfVisualWords = 1024;

            const int sampleImages = 2;
            var paleteFile = @"Data\Palettes\" + boc + "_" + cluster + "_" + numOfcolors + "_clusters.txt";
            var dictionaryFile = @"Data\Dictionaries\lboc_" + numOfVisualWords + "_" + lboctree + "Lire.txt";
            
            string trainFile = @"Data\Features\lboc_" + "_" + cluster + "_" + numOfcolors + "_" + numOfVisualWords + "_Lire_AccordKDTree_train.txt";
            string testFile = @"Data\Features\lboc_" + "_" + cluster + "_" + numOfcolors + "_" + numOfVisualWords + "_Lire_AccordKDTree_test.txt";

            const string baseFolder = @"C:\Users\l.valavanis\Desktop\Clef2013";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;


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

                model = cluster.CreateClusters(colors, numOfcolors);
                colors.Clear();
                finalClusters = model.Means;
                finalClusters = finalClusters.OrderByDescending(b => b[0]).ToList();

                Files.WriteFile(paleteFile, finalClusters);

                #endregion
            }

         
            #endregion


            #region cluster lboc and create dictionary

            ILocalFeatures lboc = new Lboc(colorspace, model);

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
        public void CanCreateGBoC()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            var colorspace = ColorConversion.ColorSpace.RGB;

            ILocalFeatures boc = new GBoC(colorspace);
            ICluster cluster = new Kmeans();

            const bool isDistinctColors = true;
            const int numOfcolors = 50;
            const int sampleImages = 2;

            var trainFile = @"Data\Features\" + boc + "_" + cluster + "_" + numOfcolors  + "_train.txt";
            var testFile = @"Data\Features\" + boc + "_" + cluster + "_" + numOfcolors + "_test.txt";
            var paleteFile = @"Data\Palettes\" + boc + "_" + cluster + "_" + numOfcolors + "_clusters.txt";

            const string trainLabelsFile = @"Data\Features\boc_labels_train.txt";
            const string testLabelsFile = @"Data\Features\boc_labels_test.txt";


            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;



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
                    LocalBitmap bitmap = new LocalBitmap(image);
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
            
            //int[][] palette = Arrays.ConvertDoubleListToIntArray(ref centers);
            boc = new GBoC(colorspace, model);

           
            #region Feature extraction GBOC

            var trainFeatures = new List<double[]>();
            var trainLabels = new List<double>();

            Dictionary<string, int> mapping = Files.MapCategoriesToNumbers(trainPath);
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);
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
                LocalBitmap bitmap = new LocalBitmap(test);
                double[] vec = boc.ExtractHistogram(bitmap);

                int cat;
                mapping.TryGetValue(test.Split('\\')[test.Split('\\').Length - 2], out cat);

                testLabels.Add(cat);
                testFeatures.Add(vec);
            }
            Files.WriteFile(testFile, testFeatures);
            Files.WriteFile(testLabelsFile, testLabels);

            #endregion

            stopwatch.Stop();
            Console.WriteLine("program run for : " + stopwatch.Elapsed);
        }

        [TestMethod]
        public void CanCreateBovw()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch

            const int clusterNum = 100;
            const int sampleImages = 2;

            SurfExtractionMethod surf = SurfExtractionMethod.Surf;

            ICluster cluster = new Kmeans();
            ILocalFeatures feature = new Vlad();
            IKdTree tree = new KdTree();

            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;

            var trainFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_train.txt";
            var testFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_test.txt";
            var clustersFile = @"Data\Palettes\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_clusters.txt";

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

 
            tree.CreateTree(finalClusters);
            model.Tree = tree;
            
            #endregion

            feature = new Vlad(model);


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

            const int clusterNum = 1536;
            const int sampleImages = 2;
            const int maxNumberClusterFeatures = 300000;

            ICluster cluster = new Kmeans();
            ILocalFeatures feature = new Phow();
            IKdTree tree = new KdTree();

            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;

            var trainFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_train.txt";
            var testFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_test.txt";
            var clustersFile = @"Data\Palettes\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_clusters.txt";

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
            
            tree.CreateTree(finalClusters);
            model.Tree = tree;

            feature = new Phow(model);


            LocalBitmap bitmap1 = new LocalBitmap(@"Data\database\einstein.jpg");
            double[] vec1 = feature.ExtractHistogram(bitmap1);
            Files.WriteFile(@"C:\Users\leonidas\Desktop\images\einsteinc#_phow_fast_phowtest.txt", new List<double[]> { vec1 });



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

            const int clusterNum = 512;
            const int sampleImages = 3;
            const int maxNumberClusterFeatures = 300000;

            ICluster cluster = new Kmeans();
            ILocalFeatures feature = new DenseSift();
            IKdTree tree = new KdTree();

            const string baseFolder = @"C:\Users\leonidas\Desktop\experiments\Clef2013\Compound";
            var trainPath = Path.Combine(baseFolder, "TrainSet");
            var testPath = Path.Combine(baseFolder, "TestSet");

            trainPath = @"E:\Datasets\Image\ImageCLEF_2017\Caption\CaptionPrediction\training\CaptionPredictionTraining2017";
            testPath = @"E:\Datasets\Image\ImageCLEF_2017\Caption\ConceptDetection\test\ConceptDetectionTesting2017";


            var trainDest = @"E:\Datasets\Image\ImageCLEF_2017\Caption\Image Features\Features\DenseSift_512\train\";
            var testDest = @"E:\Datasets\Image\ImageCLEF_2017\Caption\Image Features\Features\DenseSift_512\test\";



            //const string baseFolder = @"Data\database";
            //var trainPath = baseFolder;
            //var testPath = baseFolder;

            var trainFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_train.txt";
            var testFile = @"Data\Features\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_test.txt";
            var clustersFile = @"Data\Palettes\" + feature + "_" + cluster + "_" + clusterNum + "_" + tree + "_clusters.txt";

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

                //var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages,null,false);
                var sampleImgs = Files.GetFilesFrom(trainPath).OrderBy(x => Guid.NewGuid()).Take(4000).ToArray();
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

            tree.CreateTree(finalClusters);
            model.Tree = tree;

            feature = new DenseSift(model);

            #region features extraction

            //Feature extraction bovw
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train);
                var vec = feature.ExtractHistogram(bitmap);

                var name = Files.GetFileNameWithoutExtension(train);
                Files.WriteFile(trainDest + name + "_DenseSift.txt", vec.ToList(), true);


                //Files.WriteAppendFile(trainFile, vec);
            }

            //Feature extraction bovw

            foreach (var test in Files.GetFilesFrom(testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test);
                var vec = feature.ExtractHistogram(bitmap);

                var name = Files.GetFileNameWithoutExtension(test);
                Files.WriteFile(testDest + name + "_DenseSift.txt", vec.ToList(), true);

                //Files.WriteAppendFile(testFile, vec);
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

            ILocalFeatures extractor = new FisherVector();
            ICluster cluster = new GMM();
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

            ILocalFeatures fisher = new FisherVector(model,new FisherVector() );
            LocalBitmap bitmap1 = new LocalBitmap(imagePath);
            var histogram = fisher.ExtractHistogram(bitmap1);
        }


    }
}
