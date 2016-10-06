using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.KDTree;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            IKdTree kdtree = new AccordKdTree();
            kdtree.CreateTree(centers);
            double[] search = { 50.0, 50.0, 50.0 };
            int index = kdtree.SearchTree(search);

        }

        [TestMethod]
        public void CanClusterAccordKmeans()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

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

        //TODO NEED TO CHECK vsexecution problem.exe PROBLEM
        //pass vlfeat kmeans,vlfeat em , accord kmeans
        [TestMethod]
        public void CanClusterVlfeatKmeans()
        {
            string baseFolder = @"Data";
            //string trainPath = Path.Combine(baseFolder, "Train");

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder);

            IFeatures phow = new Phow();
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
            List<double[]> vocab = cluster.CreateClusters(colors, numOfClusters);

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

            IFeatures phow = new Phow();
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
            List<double[]> vocab = cluster.CreateClusters(colors, numOfClusters);

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
            List<double[]> vocab = cluster.CreateClusters(colors, numOfClusters);
        }


    }
}
