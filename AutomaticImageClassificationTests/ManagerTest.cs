using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Feature.Global;
using AutomaticImageClassification.KDTree;
using AutomaticImageClassification.Managers;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Feature.Global;
using AutomaticImageClassification.Feature.Local;

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

        private const string BaseFolder = @"Data";
        private readonly string _trainPath = Path.Combine(BaseFolder, "database");
        private readonly string _testPath = Path.Combine(BaseFolder, "database");


        private const int ImageHeight = 256;
        private const int ImageWidth = 256;
        private readonly int _clusterNum = 512;
        private readonly int _paletteSize = 50;
        private readonly int _noOfVWords = 1024;

        private readonly int _sampleImages = 10;
        private readonly int _extractImage = int.MaxValue;
        private readonly int _maxNumberClusterFeatures = 200000;

        private readonly ClusterMethod _clusterMethod = ClusterMethod.KMeans;
        private readonly KdTreeMethod _kdTreeMethod = KdTreeMethod.KdTree;
        private readonly ImageRepresentationMethod _imageRepresentationMethod = ImageRepresentationMethod.Surf;

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
                _paletteSize = 10;
                _noOfVWords = 20;
            }

            _baseParameters = new BaseParameters(ImageHeight, ImageWidth)
            {
                IrmParameters = new ImageRepresentationParameters()
                {
                    BasicImageRepresentationMethod = _imageRepresentationMethod,
                    CurrentImageRepresentationMethod = _imageRepresentationMethod,
                    IrmToUseDescriptors = ImageRepresentationMethod.Sift,

                    ColorCorrelogramExtractionMethod = ColorCorrelogram.ColorCorrelogramExtractionMethod.LireAlgorithm,
                    MkLabSurfExtractionMethod = Surf.MkLabSurfExtractionMethod.ColorSurf,

                    ColorSpace = ColorConversion.ColorSpace.RGB,
                    UseCombinedQuantization = true
                },
                ClusterParameters = new ClusterParameters()
                {
                    BaseFolder = _trainPath,
                    SampleImages = _sampleImages,
                    ClusterNum = _clusterNum,
                    PaletteSize = _paletteSize,
                    NumVWords = _noOfVWords,
                    ClusterMethod = _clusterMethod,
                    MaxNumberClusterFeatures = _maxNumberClusterFeatures,
                    IsDistinctDescriptors = false,
                    OrderByDescending = false
                },
                KdTreeParameters = new KdTreeParameters
                {
                    Kdtree = _kdTreeMethod
                }
            };

            ImageRepresentationManager.InitBeforeCluster(ref _baseParameters);

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
                    #region init

                    //initialize new parameters each time
                    //_baseParameters = new BaseParameters(ImageHeight, ImageWidth);

                    _baseParameters.IrmParameters.ClusterModels.Clear();
                    _baseParameters.IrmParameters.BasicImageRepresentationMethod = method;
                    _baseParameters.IrmParameters.CurrentImageRepresentationMethod = method;

                    //Fisher Vector
                    _baseParameters.ClusterParameters.ClusterMethod = method == ImageRepresentationMethod.VlFeatFisherVector
                                        ? ClusterMethod.GMM
                                        : _clusterMethod;

                    //LBOC
                    if (method == ImageRepresentationMethod.Lboc)
                    {
                        _baseParameters.ClusterParameters.ClusterNum = _baseParameters.ClusterParameters.PaletteSize;
                    }

                    #endregion

                    ImageRepresentationManager.InitBeforeCluster(ref _baseParameters);

                    #region  continue init
                    //LBOC
                    if (method == ImageRepresentationMethod.Lboc)
                    {
                        _baseParameters.ClusterParameters.ClusterNum = _baseParameters.ClusterParameters.NumVWords;
                    }

                    #endregion


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

            ImageRepresentationManager.InitAfterCluster(ref _baseParameters);

            if (_baseParameters.ExtractionFeature.CanCluster)
            {
                var clustersFile = @"Data\Palettes\" + _baseParameters.ExtractionFeature + "_" + _clusterNum +
                                                   "_clusters.txt";
                Files.WriteFile(clustersFile, _baseParameters.IrmParameters.ClusterModels[0].Means);
            }
            ImageRepresentationManagerTest();
        }

        public void ClusterManagerTest()
        {
            if (!_baseParameters.ExtractionFeature.CanCluster)
                return;

            ClusterManager.Cluster(ref _baseParameters);
        }

        public void KdTreeManagerTest()
        {
            if (!_baseParameters.ExtractionFeature.CanCluster)
                return;

            KdTreeManager.CreateTree(ref _baseParameters);
        }

        public void ImageRepresentationManagerTest()
        {
            var clusterName = _baseParameters.IrmParameters.ClusterModels.Count == 0
                ? ""
                : _baseParameters.IrmParameters.ClusterModels[0].ToString();

            var trainFile = @"Data\Features\" + _baseParameters.ExtractionFeature + "_" + clusterName + "_" + _baseParameters.ClusterParameters.ClusterNum + "_train.txt";
            var testFile = @"Data\Features\" + _baseParameters.ExtractionFeature + "_" + clusterName + "_" + _baseParameters.ClusterParameters.ClusterNum + "_test.txt";
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
