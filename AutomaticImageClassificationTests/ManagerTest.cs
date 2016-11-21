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
        //private bool TestAllClusterMethods = false;
        //private bool TestAllKdTreeMethods = false;


        private BaseParameters _baseParameters;
        
        private ClusterManager _clusterManager;
        private KdTreeManager _kdTreeManager;
        private ImageRepresentationManager _imageRepresentationManager;



        private const string BaseFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";
        private readonly string _trainPath = Path.Combine(BaseFolder, "Train");
        private readonly string _testPath = Path.Combine(BaseFolder, "Test");


        private const int ImageHeight = 256;
        private const int ImageWidth = 256;
        private readonly int _clusterNum = 512;
        private readonly int _paletteSize = 50;
        private readonly int _sampleImages = 10;
        private readonly int _extractImage = int.MaxValue;
        private readonly int _maxNumberClusterFeatures = 200000;

        private readonly ClusterMethod _clusterMethod = ClusterMethod.LireKmeans;
        private readonly ImageRepresentationMethod _imageRepresentationMethod = ImageRepresentationMethod.JOpenSurf;

        private readonly bool _lite = true;

        public ManagerTest()
        {
            //if lite then experiment for testing so values are small
            if (_lite)
            {
                _clusterNum = 10;
                _sampleImages = 1;
                _extractImage = 2;
                _maxNumberClusterFeatures = 5000;
            }

            _baseParameters = new BaseParameters(ImageHeight, ImageWidth)
            {
                IrmParameters = new ImageRepresentationParameters()
                {
                    ImageRepresentationMethod = _imageRepresentationMethod,
                    Feature = new AccordSurf(),
                    ColorCorrelogramExtractionMethod = ColorCorrelogram.ColorCorrelogramExtractionMethod.LireAlgorithm,
                    MkLabSiftExtractionMethod = MkLabSift.MkLabSiftExtractionMethod.RootSift,
                    MkLabSurfExtractionMethod = MkLabSurf.MkLabSurfExtractionMethod.ColorSurf,
                    //IrmToUseDescriptors = ImageRepresentationMethod.AccordSurf,
                    ColorSpace = ColorConversion.ColorSpace.RGB,
                    UseCombinedQuantization = true
                },
                ClusterParameters = new ClusterParameters()
                {
                    BaseFolder = _trainPath,
                    SampleImages = _sampleImages,
                    ClusterNum = _clusterNum,
                    PaletteSize = _paletteSize,
                    ClusterMethod = _clusterMethod,
                    MaxNumberClusterFeatures = _maxNumberClusterFeatures,
                    IsDistinctDescriptors = false,
                    OrderByDescending = false
                },
                KdTreeParameters = new KdTreeParameters
                {
                    Kdtree = KdTreeMethod.VlFeatKdTree
                }
            };

            _imageRepresentationManager = new ImageRepresentationManager(_baseParameters);
            _baseParameters.ExtractionFeature = _imageRepresentationManager.InitBeforeCluster();

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
                    //initialize new parameters each time
                    //_baseParameters = new BaseParameters(ImageHeight, ImageWidth);

                    _baseParameters.ClusterParameters.ClusterMethod = method == ImageRepresentationMethod.VlFeatFisherVector
                                        ? ClusterMethod.VlFeatGmm
                                        : _clusterMethod;

                    _baseParameters.IrmParameters.ImageRepresentationMethod = method;



                    _imageRepresentationManager = new ImageRepresentationManager(_baseParameters);
                    _baseParameters.ExtractionFeature = _imageRepresentationManager.InitBeforeCluster();

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

            var clustersFile = @"Data\Palettes\" + _baseParameters.ExtractionFeature + "_" + _clusterNum + "_clusters.txt";
            Files.WriteFile(clustersFile, _baseParameters.IrmParameters.ClusterModel.Means);

            ImageRepresentationManagerTest();
        }

        public void ClusterManagerTest()
        {
            if (!_baseParameters.ExtractionFeature.CanCluster)
                return;

            _clusterManager = new ClusterManager(_baseParameters);
            _baseParameters.IrmParameters.ClusterModel = _clusterManager.Cluster();
            
        }

        public void KdTreeManagerTest()
        {
            if (!_baseParameters.ExtractionFeature.CanCluster)
                return;

            _baseParameters.KdTreeParameters.Model = _baseParameters.IrmParameters.ClusterModel;
            _kdTreeManager = new KdTreeManager(_baseParameters.KdTreeParameters);
            _baseParameters.IrmParameters.ClusterModel.Tree = _kdTreeManager.CreateTree();
        }

        public void ImageRepresentationManagerTest()
        {
            var trainFile = @"Data\Features\" + _baseParameters.ExtractionFeature + "_" + _baseParameters.IrmParameters.ClusterModel + "_" + _baseParameters.ClusterParameters.ClusterNum + "_train.txt";
            var testFile = @"Data\Features\" + _baseParameters.ExtractionFeature + "_" + _baseParameters.IrmParameters.ClusterModel + "_" + _baseParameters.ClusterParameters.ClusterNum + "_test.txt";
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

            _baseParameters.ExtractionFeature = _imageRepresentationManager.InitAfterCluster();

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

                var vec = _baseParameters.ExtractionFeature.ExtractHistogram(bitmap);
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
                var vec = _baseParameters.ExtractionFeature.ExtractHistogram(bitmap);
                Files.WriteAppendFile(testFile, vec);

                int cat;
                mapping.TryGetValue(test.Split('\\')[test.Split('\\').Length - 2], out cat);
                testLabels.Add(cat);
            }
            Files.WriteFile(_testLabelsFile, testLabels);

        }

    }
}
