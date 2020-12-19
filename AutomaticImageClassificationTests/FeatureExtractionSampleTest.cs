using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
    public class FeatureExtractionSampleTest
    {
        [TestMethod]
        public void ExtractPhow()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            ILocalFeatures phow = new Phow();
            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(phow.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            phow = new Phow(model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = phow.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void ExtractDenseSift()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            ILocalFeatures dsift = new DenseSift();
            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(dsift.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            model.Tree = tree;

            dsift = new DenseSift(model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = dsift.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void ExtractSift()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            ILocalFeatures sift = new Sift();
            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(sift.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            model.Tree = tree;

            sift = new Sift(model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = sift.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void ExtractSurf()
        {
            SurfExtractionMethod method = SurfExtractionMethod.ColorSurf;

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            ILocalFeatures surf = new Surf(method);
            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(surf.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            model.Tree = tree;

            surf = new Surf(model, method);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = surf.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void ExtractFisherVector()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            ILocalFeatures fisher = new FisherVector();
            ICluster cluster = new GMM();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(fisher.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            fisher = new FisherVector(model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = fisher.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void ExtractVlad()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            ILocalFeatures vlad = new Vlad();
            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(vlad.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            vlad = new Vlad(model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = vlad.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void ExtractBoc()
        {

            string baseFolder = @"Data";

            const bool isDistinctColors = true;

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);
            var colorspace = ColorConversion.ColorSpace.RGB;

            ILocalFeatures boc = new Boc(colorspace);

            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(boc.ExtractDescriptors(bitmap));
            }
            if (isDistinctColors)
            {
                Arrays.GetDistinctObjects(ref colors);
            }

            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            boc = new Boc(colorspace, model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = boc.ExtractHistogram(bitmap);
            }


        }

        [TestMethod]
        public void ExtractLboc()
        {
            string baseFolder = @"Data";

            const bool isDistinctColors = true;

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);
            var colorspace = ColorConversion.ColorSpace.RGB;

            ILocalFeatures boc = new Boc(colorspace);

            ICluster cluster = new Kmeans();
            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(boc.ExtractDescriptors(bitmap));
            }
            if (isDistinctColors)
            {
                Arrays.GetDistinctObjects(ref colors);
            }

            ClusterModel bocModel = cluster.CreateClusters(colors, numOfClusters);

            IKdTree boctree = new KdTree();
            boctree.CreateTree(bocModel.Means);
            bocModel.Tree = boctree;

            ILocalFeatures lboc = new Lboc(colorspace, bocModel);

            colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(lboc.ExtractDescriptors(bitmap));
            }
            if (isDistinctColors)
            {
                Arrays.GetDistinctObjects(ref colors);
            }

            ClusterModel lbocModel = cluster.CreateClusters(colors, numOfClusters);

            IKdTree lboctree = new KdTree();
            lboctree.CreateTree(lbocModel.Means);
            lbocModel.Tree = lboctree;

            lboc = new Lboc(colorspace, bocModel, lbocModel);


            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = lboc.ExtractHistogram(bitmap);
            }



        }

        [TestMethod]
        public void ExtractGboc()
        {

            string baseFolder = @"Data";

            var numOfClusters = 10;
            var sampleImgs = Files.GetFilesFrom(baseFolder, 2);

            var colorspace = ColorConversion.ColorSpace.RGB;

            ILocalFeatures gboc = new GBoC(colorspace);
            ICluster cluster = new Kmeans();

            const bool isDistinctColors = true;
            const int numOfcolors = 50;
            const int sampleImages = 2;

            List<double[]> colors = new List<double[]>();

            foreach (var image in sampleImgs)
            {
                LocalBitmap bitmap = new LocalBitmap(image);
                colors.AddRange(gboc.ExtractDescriptors(bitmap));
            }
            ClusterModel model = cluster.CreateClusters(colors, numOfClusters);

            IKdTree tree = new KdTree();
            tree.CreateTree(model.Means);
            gboc = new GBoC(colorspace, model);

            foreach (var image in sampleImgs)
            {
                Console.WriteLine("extracting image " + image);
                LocalBitmap bitmap = new LocalBitmap(image);
                double[] vector = gboc.ExtractHistogram(bitmap);
            }

        }

        [TestMethod]
        public void ExtractColorCorrelogram()
        {
            string image = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(image);

            IGlobalFeatures feat = new ColorCorrelogram();
            var histogram = feat.ExtractHistogram(bitmap);

        }

        [TestMethod]
        public void ExtractBTDH()
        {
            string image = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(image);

            IGlobalFeatures feat = new BTDH();
            var histogram = feat.ExtractHistogram(bitmap);

        }

        [TestMethod]
        public void ExtractCEDD()
        {
            string image = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(image);

            IGlobalFeatures feat = new CEDD();
            var histogram = feat.ExtractHistogram(bitmap);

        }

        [TestMethod]
        public void ExtractEdgeHistogram()
        {
            string image = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(image);

            IGlobalFeatures feat = new EdgeHistogram();
            var histogram = feat.ExtractHistogram(bitmap);

        }

        [TestMethod]
        public void ExtractFCTH()
        {
            string image = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(image);

            IGlobalFeatures feat = new FCTH();
            var histogram = feat.ExtractHistogram(bitmap);

        }

        [TestMethod]
        public void ExtractJCD()
        {
            string image = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(image);

            IGlobalFeatures feat = new JCD();
            var histogram = feat.ExtractHistogram(bitmap);

        }

        [TestMethod]
        public void ExtractPhog()
        {
            string image = @"Data\database\einstein.jpg";
            LocalBitmap bitmap = new LocalBitmap(image);

            IGlobalFeatures feat = new PHOG();
            var histogram = feat.ExtractHistogram(bitmap);

        }

    }
}
