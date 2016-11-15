using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Cluster.EM;
using AutomaticImageClassification.Cluster.GaussianMixtureModel;
using AutomaticImageClassification.Cluster.Kmeans;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Feature.Bovw;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sun.security.jca;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class ClusterTest
    {
        //pass test
        [TestMethod]
        public void CanUseAccordKdTree()
        {

            List<double[]> centers = new List<double[]>();
            centers.Add(new[] { 1.0, 2.0, 3.0 });
            centers.Add(new[] { 10.0, 20.0, 30.0 });
            centers.Add(new[] { 100.0, 200.0, 300.0 });
            centers.Add(new[] { 1000.0, 2000.0, 3000.0 });
            centers.Add(new[] { 10000.0, 20000.0, 30000.0 });

            IKdTree kdtree = new AccordKdTree(centers);
            kdtree.CreateTree(centers);
            double[] search = { 50000.0, 50000.0, 50000.0 };

            int index = kdtree.SearchTree(search);

        }

        [TestMethod]
        public void CanUseJavaMlKdTree()
        {

            List<double[]> centers = new List<double[]>();
            centers.Add(new[] { 1.0, 2.0, 3.0 });
            centers.Add(new[] { 10.0, 20.0, 30.0 });
            centers.Add(new[] { 100.0, 200.0, 300.0 });
            centers.Add(new[] { 1000.0, 2000.0, 3000.0 });
            centers.Add(new[] { 10000.0, 20000.0, 30000.0 });

            IKdTree kdtree = new JavaMlKdTree();
            kdtree.CreateTree(centers);
            double[] search = { 5000.0, 5000.0, 5000.0 };

            int index = kdtree.SearchTree(search);

        }

        [TestMethod]
        public void CanClusterAccordKmeans()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures phow = new VlFeatPhow();
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
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

        }

        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        //pass vlfeat kmeans,vlfeat em , accord kmeans
        [TestMethod]
        public void CanClusterVlfeatKmeans()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures phow = new VlFeatPhow();
            ICluster cluster = new VlFeatKmeans(10000);
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
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

        }

        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        //pass vlfeat kmeans,vlfeat em , accord kmeans
        [TestMethod]
        public void CanClusterVlfeatEm()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures phow = new VlFeatPhow();
            ICluster cluster = new VlFeatEm(10000);
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
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

        }

        [TestMethod]
        public void CanClusterLireKMeans()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures colorFeatures = new AccordSurf();
            ICluster cluster = new LireKmeans();
            List<double[]> colors = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 2)
                {
                    break;
                }
                counter++;
                colors.AddRange(colorFeatures.ExtractDescriptors(image));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);


        }

        [TestMethod]
        public void CanClusterAccordGmm()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures extractor = new AccordSurf();
            ICluster cluster = new AccordGmm();
            List<double[]> clusters = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 5)
                {
                    break;
                }
                counter++;
                clusters.AddRange(extractor.ExtractDescriptors(image));
            }
            ClusterModel model = cluster.CreateClusters(clusters, numOfClusters);
        }

        [TestMethod]
        public void CanClusterVlFeatGmm()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures extractor = new AccordSurf();
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
                clusters.AddRange(extractor.ExtractDescriptors(image));
            }
            ClusterModel model = cluster.CreateClusters(clusters, numOfClusters);

        }

        [TestMethod]
        public void CanCreateVlFeatKdTree()
        {
            Console.WriteLine("starting");

            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures extractor = new AccordSurf();
            ICluster cluster = new AccordKmeans();
            List<double[]> clusters = new List<double[]>();
            int counter = 0;
            foreach (var image in sampleImgs)
            {
                if (counter == 5)
                {
                    break;
                }
                counter++;
                clusters.AddRange(extractor.ExtractDescriptors(image));
            }
            ClusterModel model = cluster.CreateClusters(clusters, numOfClusters);

            Console.WriteLine("creating tree");

            IKdTree tree = new VlFeatKdTree();
            tree.CreateTree(model.Means);

            string imaging = @"Data\database\einstein.jpg";
            var feat = extractor.ExtractDescriptors(imaging);

            List<int> indexes = feat.Select(doublese => tree.SearchTree(doublese)).ToList();


            double[] query =
                Enumerable.Range(0, model.Means[0].Length).Select(v => (double)new Random().Next(1, 1000)).ToArray();

            Console.WriteLine("querying tree");

            var index = tree.SearchTree(query);

        }

        [TestMethod]
        public void CanSearchVlFeatKdTree()
        {
            List<double[]> vocab = new List<double[]>
            {
                new double[] {1, 1, 1, 1},
                new double[] {100, 100, 100, 100},
                new double[] {1000, 1000, 1000, 1000},
                new double[] {2000, 1200, 200, 2000}
            };

            IKdTree tree = new VlFeatKdTree();
            tree.CreateTree(vocab);

            double[] query = new double[] { 1, 2, 3, 1 };

            int index = tree.SearchTree(query);
            Assert.AreEqual(index,0);
        }


    }
}
