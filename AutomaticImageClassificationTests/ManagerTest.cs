using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Feature.Bovw;
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

        private readonly ImageRepresentationManager _imageRepresentationManager;
        private readonly ImageRepresentationParameters _imageRepresentationParameters;

        private KdTreeManager _kdTreeManager;
        private readonly KdTreeParameters _kdTreeParameters;

        private IFeatures _feature;

        private const string BaseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
        private readonly string _trainPath = Path.Combine(BaseFolder, "Train");
        private readonly string _testPath = Path.Combine(BaseFolder, "Test");


        private const int ImageHeight = 256;
        private const int ImageWidth = 256;
        private readonly int _clusterNum = 512;
        private readonly int _sampleImages = 10;
        private readonly int _extractImage = int.MaxValue;
        private readonly int _maxNumberClusterFeatures = 200000;

        private readonly ClusterMethod _clusterMethod = ClusterMethod.LireKmeans;
        private readonly ImageRepresentationMethod _imageRepresentationMethod = ImageRepresentationMethod.JOpenSurf;

        private readonly bool _lite = true;

        public ManagerTest()
        {

            if (_lite)
            {
                _clusterNum = 10;
                _sampleImages = 1;
                _extractImage = 2;
                _maxNumberClusterFeatures = 5000;
            }


            #region image representation parameters , manager

            _imageRepresentationParameters = new ImageRepresentationParameters(null,ImageHeight,ImageWidth)
            {
                ImageRepresentationMethod = _imageRepresentationMethod,

                ColorCorrelogramExtractionMethod = ColorCorrelogram.ColorCorrelogramExtractionMethod.LireAlgorithm,
                MkLabSiftExtractionMethod = MkLabSift.MkLabSiftExtractionMethod.RootSift,
                MkLabSurfExtractionMethod = MkLabSurf.MkLabSurfExtractionMethod.ColorSurf,

                IrmToUseDescriptors = ImageRepresentationMethod.AccordSurf,
                ColorSpace = ColorConversion.ColorSpace.RGB,
                UseCombinedQuantization = true
            };

            _imageRepresentationManager = new ImageRepresentationManager(_imageRepresentationParameters);
            _feature = _imageRepresentationManager.InitBeforeCluster();
            _imageRepresentationParameters.SetExtractionFeature(_feature);


            #endregion

            #region Cluster parameters

            _clusterParameters = new ClusterParameters(_feature, ImageHeight, ImageWidth)
            {
                BaseFolder = _trainPath,
                SampleImages = _sampleImages,
                ClusterNum = _clusterNum,
                ClusterMethod = _clusterMethod,
                MaxNumberClusterFeatures = _maxNumberClusterFeatures,
                IsDistinctDescriptors = false,
                OrderByDescending = false
            };


            #endregion

            #region KdTree

            _kdTreeParameters = new KdTreeParameters
            {
                Kdtree = KdTreeMethod.JavaMlKdTree
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
                    _clusterParameters.ClusterMethod = method == ImageRepresentationMethod.VlFeatFisherVector
                        ? ClusterMethod.VlFeatGmm
                        : _clusterMethod;

                    _imageRepresentationManager.IrmParameters.ImageRepresentationMethod = method;
                    _feature = _imageRepresentationManager.InitBeforeCluster();

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
            if (!_feature.CanCluster)
                return;

            _clusterManager = new ClusterManager(_clusterParameters);
            _imageRepresentationManager.IrmParameters.ClusterManager = _clusterManager;
            _clusterModel = _clusterManager.Cluster();
            _imageRepresentationManager.IrmParameters.ClusterModel = _clusterModel;

            var clustersFile = @"Data\Palettes\" + _imageRepresentationManager.Feature + "_" + _clusterNum + "_clusters.txt";
            Files.WriteFile(clustersFile, _clusterModel.Means);
        }

        public void KdTreeManagerTest()
        {
            if (!_feature.CanCluster)
                return;

            _kdTreeParameters.Model = _clusterModel;
            _kdTreeManager = new KdTreeManager(_kdTreeParameters);
            _imageRepresentationManager.IrmParameters.ClusterModel.Tree = _kdTreeManager.CreateTree();

        }

        public void ImageRepresentationManagerTest()
        {
            var trainFile = @"Data\Features\" + _feature + "_" + _clusterModel + "_" + _clusterParameters.ClusterNum + "_train.txt";
            var testFile = @"Data\Features\" + _feature + "_" + _clusterModel + "_" + _clusterParameters.ClusterNum + "_test.txt";
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

            int counter = 0;
            var trainLabels = new List<double>();
            foreach (var train in Files.GetFilesFrom(_trainPath))
            {
                if (_extractImage < counter)
                {
                    break;
                }
                counter++;
                LocalBitmap bitmap = new LocalBitmap(train, ImageHeight, ImageWidth);

                var vec = _feature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(trainFile, vec);

                int cat;
                mapping.TryGetValue(train.Split('\\')[train.Split('\\').Length - 2], out cat);
                trainLabels.Add(cat);
            }
            Files.WriteFile(_trainLabelsFile, trainLabels);


            counter = 0;
            var testLabels = new List<double>();
            foreach (var test in Files.GetFilesFrom(_testPath))
            {
                if (_extractImage < counter)
                {
                    break;
                }
                counter++;
                LocalBitmap bitmap = new LocalBitmap(test, new Bitmap(test), ImageHeight);
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
