using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Managers;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    /// <summary>
    /// Summary description for ManagerTest
    /// </summary>
    [TestClass]
    public class ManagerTest
    {
        private bool TestAllIMageRepresentationMethods = true;
        private bool TestAllClusterMethods = false;
        private bool TestAllKdTreeMethods = false;


        private ClusterManager _clusterManager;
        private readonly ClusterParameters _clusterParameters;
        private ClusterModel _clusterModel;

        private ImageRepresentationManager _imageRepresentationManager;
        private readonly ImageRepresentationParameters _imageRepresentationParameters;

        private KdTreeManager _kdTreeManager;
        private readonly KdTreeParameters _kdTreeParameters;

        private IFeatures _feature;

        private const string BaseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
        private readonly string _trainPath = Path.Combine(BaseFolder, "Train");
        private readonly string _testPath = Path.Combine(BaseFolder, "Test");

        private const int ImageHeight = 480;
        private const int ClusterNum = 5;
        private const int SampleImages = 1;
        private ClusterMethod _clusterMethod = ClusterMethod.LireKmeans;
        private ImageRepresentationMethod _imageRepresentationMethod = ImageRepresentationMethod.JOpenSurf;

        public ManagerTest()
        {

            #region image representation parameters , manager

            _imageRepresentationParameters = new ImageRepresentationParameters
            {
                ImageRepresentationMethod = _imageRepresentationMethod,
                ClusterNum = ClusterNum
            };

            _imageRepresentationManager = new ImageRepresentationManager(_imageRepresentationParameters);

            #endregion

            #region Cluster parameters

            _clusterParameters = new ClusterParameters
            {
                BaseFolder = _trainPath,
                SampleImages = SampleImages,
                ClusterNum = ClusterNum,
                ClusterMethod = _clusterMethod,
                Feature = _imageRepresentationManager.InitBeforeCluster(),
                NumberOfFeatures = int.MaxValue,
                IsRandomInit = false,
                Height = ImageHeight
            };

            #endregion

            #region KdTree

            _kdTreeParameters = new KdTreeParameters
            {
                Kdtree = KdTreeMethod.AccordKdTree
            };

            #endregion

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ManageAllTest()
        {
            if (TestAllIMageRepresentationMethods)
            {
                foreach (ImageRepresentationMethod method in Enum.GetValues(typeof(ImageRepresentationMethod)))
                {
                    _imageRepresentationParameters.ImageRepresentationMethod = method;
                    _imageRepresentationManager = new ImageRepresentationManager(_imageRepresentationParameters);
                    CompleteManagerTest();
                }
            }
            else
            {
                CompleteManagerTest();
            }            
        }

        public void CompleteManagerTest()
        {
            ClusterManagerTest();
            KdTreeManagerTest();
            ImageRepresentationManagerTest();
        }

        public void ClusterManagerTest()
        {
            _clusterManager = new ClusterManager(_clusterParameters);
            _clusterModel = _clusterManager.Cluster();
        }

        public void KdTreeManagerTest()
        {
            _kdTreeParameters.Model = _clusterModel;
            _kdTreeManager = new KdTreeManager(_kdTreeParameters);
            _clusterModel.Tree = _kdTreeManager.CreateTree();
            _imageRepresentationParameters.ClusterModel = _clusterModel;

        }

        public void ImageRepresentationManagerTest()
        {
            var trainFile = @"Data\Features\" + _clusterParameters.Feature + "_" + _clusterParameters.Cluster + "_" + _clusterParameters.ClusterNum + "_train.txt";
            var testFile = @"Data\Features\" + _clusterParameters.Feature + "_" + _clusterParameters.Cluster + "_" + _clusterParameters.ClusterNum + "_test.txt";
            var _trainLabelsFile = @"Data\Features\labels_train.txt";
            var _testLabelsFile = @"Data\Features\labels_test.txt";


            if (File.Exists(trainFile))
            {
                File.Delete(trainFile);
            }
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            _feature = _imageRepresentationManager.InitAfterCluster();
            var mapping = Files.MapCategoriesToNumbers(_trainPath);
            
            var trainLabels = new List<double>();
            foreach (var train in Files.GetFilesFrom(_trainPath))
            {
                LocalBitmap bitmap = new LocalBitmap(train,ImageProcessing.ResizeImage(train, ImageHeight));

                var vec = _feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);

                int cat;
                mapping.TryGetValue(train.Split('\\')[train.Split('\\').Length - 2], out cat);
                trainLabels.Add(cat);
            }
            Files.WriteFile(_trainLabelsFile, trainLabels);



            var testLabels = new List<double>();
            foreach (var test in Files.GetFilesFrom(_testPath))
            {
                LocalBitmap bitmap = new LocalBitmap(test, ImageProcessing.ResizeImage(test, ImageHeight));
                
                var vec = _feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);

                int cat;
                mapping.TryGetValue(test.Split('\\')[test.Split('\\').Length - 2], out cat);
                testLabels.Add(cat);
            }
            Files.WriteFile(_testLabelsFile, testLabels);

        }

    }
}
