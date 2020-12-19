using System;
using System.Collections.Generic;
using System.Linq;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Cluster.EM;
using AutomaticImageClassification.Cluster.GaussianMixtureModel;
using AutomaticImageClassification.Cluster.Kmeans;
using AutomaticImageClassification.Cluster.SOM;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Feature.Global;
using AutomaticImageClassification.Feature.Local;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class ClusterTest
    {

        [TestMethod]
        public void CanUseKdTree()
        {

            List<double[]> centers = new List<double[]>
            {
                new[] { 1.0, 2.0, 3.0 },
                new[] { 10.0, 20.0, 30.0 },
                new[] { 100.0, 200.0, 300.0 },
                new[] { 1000.0, 2000.0, 3000.0 },
                new[] { 10000.0, 20000.0, 30000.0 }
            };

            IKdTree kdtree = new KdTree();
            kdtree.CreateTree(centers);
            double[] search = { 50000.0, 50000.0, 50000.0 };

            int index = kdtree.SearchTree(search);

            Assert.AreEqual(index, 4);

        }

        [TestMethod]
        public void CanClusterSOM()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            ILocalFeatures colorFeatures = new Sift();
            ICluster cluster = new SelfOrganizingMaps();
            List<double[]> colors = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 2)
                {
                    break;
                }
                counter++;

                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(colorFeatures.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);
            Assert.AreEqual(model.Means.Count, numOfClusters);


        }

        [TestMethod]
        public void CanClusterKmeans()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            ILocalFeatures colorFeatures = new Sift();
            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 2)
                {
                    break;
                }
                counter++;

                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(colorFeatures.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);
            Assert.AreEqual(model.Means.Count, numOfClusters);
            

        }

        [TestMethod]
        public void CanClusterEm()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            ILocalFeatures phow = new Phow();
            ICluster cluster = new EM();
            List<double[]> colors = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 2)
                {
                    break;
                }
                counter++;

                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(phow.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);
            Assert.AreEqual(model.Means.Count, numOfClusters);

        }

        [TestMethod]
        public void CanClusterGmm()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            ILocalFeatures extractor = new Sift();
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
            Assert.AreEqual(model.Means.Count, numOfClusters);

        }

        [TestMethod]
        public void CanCreateKdTree()
        {
            Console.WriteLine("starting");

            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            ILocalFeatures extractor = new Sift();
            ICluster cluster = new Kmeans();
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

            Console.WriteLine("creating tree");

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);

            string imaging = @"Data\database\einstein.jpg";
            LocalBitmap bitmap_ = new LocalBitmap(imaging);
            var feat = extractor.ExtractDescriptors(bitmap_);

            List<int> indexes = feat.Select(doublese => tree.SearchTree(doublese)).ToList();


            double[] query =
                Enumerable.Range(0, model.Means[0].Length).Select(v => (double)new Random().Next(1, 1000)).ToArray();

            Console.WriteLine("querying tree");

            var index = tree.SearchTree(query);

        }
        
    }
}
