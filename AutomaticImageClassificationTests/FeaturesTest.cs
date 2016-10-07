using System;
using System.Collections.Generic;
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
            ICluster cluster = new LireKmeans();

            bool isDistinctColors = true;
            int numOfcolors = 50;
            int resize = 256;
            int patches = 64;
            int sampleImages = 5;
            ColorConversion.ColorSpace colorspace = ColorConversion.ColorSpace.RGB;
            string paleteFile = @"Data\Palettes\boc_paleteLire.txt";
            string trainFile = @"Data\Features\boc_train.txt";
            string testFile = @"Data\Features\boc_test.txt";

            string baseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
            string trainPath = Path.Combine(baseFolder, "Train");
            string testPath = Path.Combine(baseFolder, "Test");

            //Create Palette
            var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);
            IFeatures boc = new Boc(resize, patches, colorspace);

            List<double[]> colors = new List<double[]>();
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

            //Create Dictionary
            IKdTree tree = new JavaMlKdTree();
            tree.CreateTree(centers);

            int[][] palette = Arrays.ConvertDoubleListToIntArray(ref centers);
            boc = new Boc(resize, colorspace, palette, tree);

            //Feature extraction BOC
            List<double[]> trainFeatures = new List<double[]>();
            foreach (var train in Files.GetFilesFrom(trainPath))
            {
                double[] vec = boc.ExtractHistogram(train);
                trainFeatures.Add(vec);
            }
            Files.WriteFile(trainFile, trainFeatures);

            List<double[]> testFeatures = new List<double[]>();
            foreach (var test in Files.GetFilesFrom(testPath))
            {
                double[] vec = boc.ExtractHistogram(test);
                testFeatures.Add(vec);
            }
            Files.WriteFile(testFile, testFeatures);


        }

        [TestMethod]
        public void CanCreateLboc()
        {
            ICluster cluster = new LireKmeans();

            bool isDistinctColors = true;
            int numOfcolors = 10;
            int numOfVisualWords = 10;

            int resize = 50;
            int patches = 25;
            int sampleImages = 5;
            ColorConversion.ColorSpace colorspace = ColorConversion.ColorSpace.RGB;
            string paleteFile = @"Data\Palettes\lboc_paleteLire.txt";
            string dictionaryFile = @"Data\Dictionaries\lboc_dictionaryLire.txt";

            string trainFile = @"Data\Features\lboc_train.txt";
            string testFile = @"Data\Features\lboc_test.txt";

            string baseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
            string trainPath = Path.Combine(baseFolder, "Train");
            string testPath = Path.Combine(baseFolder, "Test");

            #region cluster boc

            var sampleImgs = Files.GetFilesFrom(trainPath, sampleImages);
            IFeatures _boc = new Boc(resize, patches, colorspace);

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

            IKdTree boctree = new JavaMlKdTree();
            boctree.CreateTree(bocCenters);
            int[][] palette = Arrays.ConvertDoubleListToIntArray(ref bocCenters);

            #endregion

            
            #region cluster lboc and create dictionary

            IFeatures lboc = new Lboc(resize, patches, colorspace,palette,boctree);

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

            IKdTree lboctree = new JavaMlKdTree();
            lboctree.CreateTree(lbocCenters);


            #endregion

            lboc = new Lboc(resize,patches, lbocCenters, colorspace,palette,boctree,lboctree);

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


        }



    }
}
