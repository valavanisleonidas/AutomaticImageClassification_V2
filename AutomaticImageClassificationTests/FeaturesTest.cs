using System;
using System.Collections.Generic;
using System.IO;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Cluster.KDTree;

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
            string imagePath = @"Data\inner_piecepng.png";

            IFeatures colorCorrelo = new ColorCorrelogram();
            double[] featu = colorCorrelo.ExtractHistogram(imagePath);
            Files.WriteFile(@"C:\Users\l.valavanis\Desktop\colorCorre.txt", new List<double[]> { featu });
        }

        [TestMethod]
        public void CanUseJavaSift()
        {
            IKdTree kdtree = new AccordKdTree();
            IFeatures javasift = new JavaSift();

            string imagePath = @"Data\einstein.jpg";
            int clusterNum = 10;
            

            List<double[]> centroids = javasift.ExtractDescriptors(imagePath);

            Arrays.GetSubsetOfFeatures(ref centroids,10);
            kdtree.CreateTree(centroids);
            javasift = new JavaSift(kdtree, clusterNum);

            double[] featu = javasift.ExtractHistogram(imagePath);
            Files.WriteFile(@"C:\Users\l.valavanis\Desktop\sift.txt", new List<double[]> { featu });
        }


    }
}
