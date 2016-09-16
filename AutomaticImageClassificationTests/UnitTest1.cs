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
        public void TestNormalization()
        {
            double[,] arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            double[][] arr = Arrays.ToJaggedArray(ref arr1);

            FeatureNormalization.SqrtArray(ref arr[1]);
            var norm1 = FeatureNormalization.ComputeL1Norm(ref arr[0]);
            var norm2 = FeatureNormalization.ComputeL2Norm(ref arr[0]);
            FeatureNormalization.NormalizeArray(ref arr[0], ref norm1);

            FeatureNormalization.Tfidf(ref arr);

            Console.WriteLine(arr);

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


        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        //pass vlfeat kmeans,vlfeat em , accord kmeans
        [TestMethod]
        public void CanCluster()
        {
            string baseFolder = @"C:\Users\l.valavanis\Desktop\Leo Files\DBs\Clef2013\Compound";
            string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(trainPath, 1);

            IFeatures phow = new Phow();
            ICluster cluster = new AccordKmeans(10000);
            List<double[]> colors = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 2)
                {
                    break;
                }
                counter++;
                colors.AddRange(phow.ExtractDescriptors(image));
            }
            List<double[]> vocab = cluster.CreateClusters(colors, numOfClusters);

        }

        //pass liblinear
        [TestMethod]
        public void CanUseLibLinear()
        {
            string trainDataPath = @"C:\Users\l.valavanis\Desktop\train1.txt";
            string testDataPath = @"C:\Users\l.valavanis\Desktop\test1.txt";

            List<double[]> trainFeat = Files.ReadFileToArray(trainDataPath).ToList();
            double[] trainlabels = { 1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0
                ,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0
                ,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0,1,0,0,1,1,1,0,0,2,1,0,2,1,0,1,0,2,1,1,1,2,2,2,0,1,0,1,0,1,0
                ,1,0,0,0,0,0,0};

            double[] testlabels = { 1, 0, 1, 0, 1, 2, 1, 0, 2, 1, 0, 2, 1, 0, 1, 0, 1, 1, 1, 1, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0 };
            List<double[]> testFeat = Files.ReadFileToArray(testDataPath).ToList();


            //List<double[]> train_feat_arr = { { 1, 2, 3 }, { 2, 3, 4 }, { 3, 2, 1 } };
            //double[] trainlabels = { 0, 1, 1 };

            //		System.out.println(train_feat_arr.length +" : "+ train_feat_arr[0].length);
            //		System.out.println(test_feat_arr.length +" : "+ test_feat_arr[0].length);


            Parameters _params = new Parameters();
            _params.Gamma = 0.5;
            _params.Homker = "KCHI2";
            _params.Kernel = "chi2";
            _params.Cost = 1;
            _params.BiasMultiplier = 1;
            _params.Solver = "liblinear"; //liblinear
            _params.SolverType = 0;
            _params.IsManualCv = false;

            LibLinearLib classifier = new LibLinearLib(_params);


            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.ApplyKernelMapping(ref testFeat);

            classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);

            classifier.Predict(ref testFeat);


        }
        //Passed Phow extraction 
        [TestMethod]
        public void phow_test()
        {

            string baseFolder = @"C:\Users\l.valavanis\Desktop\Leo Files\DBs\Clef2013\Compound";
            string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(trainPath, 1);

            IFeatures phow = new Phow();
            ICluster cluster = new VlFeatKmeans(10000);
            List<double[]> colors = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 2)
                {
                    //break;
                }
                counter++;
                colors.AddRange(phow.ExtractDescriptors(image));
            }
            List<double[]> vocab = cluster.CreateClusters(colors, numOfClusters);
            phow = new Phow(vocab);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image "+image);
                double[] vector = phow.ExtractHistogram(image);
            }

        }

        //pass test
        [TestMethod]
        public void CanUseAccordKdTree()
        {

            List<double[]> centers= new List<double[]>();
            centers.Add(new[] {1.0, 2.0, 3.0 });
            centers.Add(new[] { 10.0, 20.0, 30.0 });
            centers.Add(new[] { 100.0, 200.0, 300.0 });
            centers.Add(new[] { 1000.0, 2000.0, 3000.0 });
            centers.Add(new[] { 10000.0, 20000.0, 30000.0 });

            IKdTree kdtree = new AccordKdTree();
            kdtree.CreateTree(centers);
            double[] search = {50.0, 50.0, 50.0};
            int index = kdtree.SearchTree(search);


        }

        //pass test
        [TestMethod]
        public void CanUseAccordSurf()
        {
            string imagePath = @"C:\Users\l.valavanis\Desktop\Leo Files\images\believe.jpg";
            string imagePath1 = @"C:\Users\l.valavanis\Desktop\Leo Files\images\titan.png";

            IFeatures surf = new AccordSurf();
            List<double[]> featu = surf.ExtractDescriptors(imagePath1);
        }

        //pass test
        [TestMethod]
        public void CanUseJOpenSurf()
        {
            string imagePath = @"C:\Users\l.valavanis\Desktop\Leo Files\images\believe.jpg";
            string imagePath1 = @"C:\Users\l.valavanis\Desktop\Leo Files\images\titan.png";

            IFeatures surf = new JOpenSurf();
            List<double[]> featu = surf.ExtractDescriptors(imagePath1);
        }

        [TestMethod]
        public void CanUseEmguSift()
        {
            string imagePath = @"C:\Users\l.valavanis\Desktop\Leo Files\images\believe.jpg";
            string imagePath1 = @"C:\Users\l.valavanis\Desktop\Leo Files\images\titan.png";

            IFeatures sift = new EmguSift();
            List<double[]> featu = sift.ExtractDescriptors(imagePath1);
        }

    }
}
