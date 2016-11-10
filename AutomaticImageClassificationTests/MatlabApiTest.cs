using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutomaticImageClassification.Classifiers;
using AutomaticImageClassification.Cluster;
using AutomaticImageClassification.Cluster.ClusterModels;
using AutomaticImageClassification.Cluster.GaussianMixtureModel;
using AutomaticImageClassification.Evaluation;
using AutomaticImageClassification.Feature;
using AutomaticImageClassification.Feature.Bovw;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    /// <summary>
    /// Summary description for MatlabApiTest
    /// </summary>
    [TestClass]
    public class MatlabApiTest
    {

        /*correct answer for list a = [1 2 3 ; 100 200 300 ; 0.4 0.6 0.8 
        0.2932    0.3338         0    0.2943    0.4502    0.1851    0.8121    0.6733    0.9866
        0.9272   -0.9215   -0.9461    0.9306   -0.8175   -0.9773         0         0         0
        0.2332    0.1984   -0.3240    0.2178    0.3591   -0.1034    0.5835    0.7394   -0.1632*/
        //same results as matlab (check results file)
        [TestMethod]
        public void CanApplyKernelMapping()
        {
            //works fine too
            var trainDataPath = @"Data\Features\boc_train.txt";
            var features = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();

            //List<double[]> features = new List<double[]>();
            //features.Add(new double[] { 1, 2, 3 });
            //features.Add(new double[] { 100, 200, 300 });
            //features.Add(new double[] { 0.4, 0.6, 0.8 });

            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2"
            };

            //liblinear
            var classifier = new LibLinearLib(_params);
            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref features);
            Files.WriteFile("liblinear_ApplyKernelMap_bocTrain.txt", features);

        }

        //same results as matlab (check results file)
        [TestMethod]
        public void CanTrainLibLinear()
        {
            var trainDataPath = @"Data\Features\boc_train.txt";
            var trainlabelsPath = @"Data\Features\train_labels.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);

            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 1,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 2,
                IsManualCv = false
            };

            //liblinear
            var classifier = new LibLinearLib(_params);

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);

            //classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            //Files.WriteFile("liblinear_model_bias.txt",classifier._model.Bias.ToList());
            //Files.WriteFile("liblinear_model_weights.txt", classifier._model.Weights.ToList());

        }

        //best cost 64 , best cv 0.37470409198512006 CORRECT -s 2 -B 1 -C 10
        //best cost 512 , best cv 0.37673317551572538 CORRECT -s 0 -B 1 -C 10
        [TestMethod]
        public void CanUseLibLinearGridSearch()
        {
            var trainDataPath = @"Data\Features\boc_train.txt";
            var trainlabelsPath = @"Data\Features\train_labels.txt";
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);

            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 1,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 0,
                IsManualCv = false
            };

            //liblinear
            var classifier = new LibLinearLib(_params);

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.GridSearch(ref trainFeat, ref trainlabels);

        }

        //same results as matlab (check results file)
        //same accuracy
        [TestMethod]
        public void CanTestLibLinear()
        {
            var trainDataPath = @"Data\Features\boc_train.txt";
            var testDataPath = @"Data\Features\boc_test.txt";

            var trainlabelsPath = @"Data\Features\train_labels.txt";
            var testlabelsPath = @"Data\Features\test_labels.txt";

            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            double[] trainlabels = Files.ReadFileTo1DArray<double>(trainlabelsPath);
            double[] testlabels = Files.ReadFileTo1DArray<double>(testlabelsPath);


            var _params = new LibLinearParameters
            {
                Gamma = 0.5,
                Homker = "KCHI2",
                Kernel = "chi2",
                Cost = 1,
                BiasMultiplier = 1,
                Solver = "liblinear",
                SolverType = 2,
                IsManualCv = false
            };

            //liblinear
            var classifier = new LibLinearLib(_params);

            // APPLY KERNEL MAPPING
            classifier.ApplyKernelMapping(ref trainFeat);
            classifier.ApplyKernelMapping(ref testFeat);

            //classifier.GridSearch(ref trainFeat, ref trainlabels);
            classifier.Train(ref trainFeat, ref trainlabels);
            classifier.Predict(ref testFeat);

            //compute accuracy
            int[] predictedLabels = Array.ConvertAll(classifier.GetPredictedCategories(), x => (int)x);
            int[] labels = Array.ConvertAll(testlabels, x => (int)x);

            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(labels, predictedLabels);

            Assert.AreEqual(accuracy, 0.39310611928737416);

        }

        [TestMethod]
        public void CanRemoveKMostFrequent()
        {
            var train = new List<double[]>
            {
                new double[] {0, 9, 1},
                new double[] {0, 0, 0},
                new double[] {0, 0, 1},
                new double[] {0, 4, 1}
            };

            var test = new List<double[]>
            {
                new double[] {0, 1, 1},
                new double[] {9, 1, 0},
                new double[] {7, 0, 1}
            };

            var kMostFrequent = 1;
            FeatureSelection.MatlabRemoveKMostFrequentFeatures(ref train, ref test, kMostFrequent);

            var correctResultTrain = new List<double[]>
            {
                new double[] {0, 9},
                new double[] {0, 0},
                new double[] {0, 0},
                new double[] {0, 4}
            };

            var correctResultTest = new List<double[]>
            {
                new double[] {0, 1},
                new double[] {9, 1},
                new double[] {7, 0}
            };
            for (int i = 0; i < correctResultTrain.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], correctResultTrain[i]);
            }
            for (int i = 0; i < correctResultTest.Count; i++)
            {
                CollectionAssert.AreEqual(test[i], correctResultTest[i]);
            }
        }

        [TestMethod]
        public void CanRemoveMostFrequestUsingThreshold()
        {
            var train = new List<double[]>
            {
                new double[] {1, 9, 1, 1, 0},
                new double[] {1, 0, 0, 1, 3},
                new double[] {1, 0, 1, 1, 4},
                new double[] {0, 4, 1, 1, 5}
            };

            var test = new List<double[]>
            {
                new double[] {0, 1, 1, 1, 2},
                new double[] {9, 1, 0, 1, 2},
                new double[] {7, 0, 1, 1, 2}
            };

            var threshold = 0.5;
            //all features ( columns ) that have more than half non zero elements are removed with threshold 0.5
            FeatureSelection.MatlabRemoveMostFrequentFeaturesUsingThreshold(ref train, ref test, threshold);

            var correctResultTrain = new List<double[]>
            {
                new double[] {9},
                new double[] {0},
                new double[] {0},
                new double[] {4}
            };

            var correctResultTest = new List<double[]>
            {
                new double[] {1},
                new double[] {1},
                new double[] {0}
            };
            for (int i = 0; i < correctResultTrain.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], correctResultTrain[i]);
            }
            for (int i = 0; i < correctResultTest.Count; i++)
            {
                CollectionAssert.AreEqual(test[i], correctResultTest[i]);
            }
        }

        [TestMethod]
        public void CanRemoveFeaturesWithInformationGainLessThanThreshold()
        {
            var train = new List<double[]>
            {
                new double[] {1,  2,  1,  0,  3,  9,  0,  1,  2},
                new double[] {0,  5,  2,  7,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1 , 0,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  0,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  1,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  0,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  4,  0,  1,  1,  1,  1 },
                new double[] {0,  0,  0,  3,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  2,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  7,  4,  0,  0,  2,  2},
                new double[] {0,  0,  0,  1,  1,  2,  4,  5,  0 }
            };


            int[] labels = { 2, 1, 3, 4, 2, 1, 2, 4, 3, 4, 1 };
            int[] categories = { 1, 2, 3, 4 };
            var threshold = 0.1;

            FeatureSelection.MatlabInformationGainUsingThreshold(ref train, ref train, ref labels, threshold);

            var results = new List<double[]>
            {
                new double[] {1,  2,  1,  3,  9,  0,  1,  2},
                new double[] {0,  5,  2,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  0,  1,  1,  1,  1 },
                new double[] {0,  0,  0,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  4,  0,  0,  2,  2},
                new double[] {0,  0,  0,  1,  2,  4,  5,  0 }
            };

            for (int i = 0; i < train.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], results[i]);
            }

            //correct results
            //double[] resultIg = { 0.845, 0.444, 0.194, 0.012, 0.433, 0.311, 0.183, 0.242, 0.189 };
            //CollectionAssert.AreEqual(ig,resultIg);

        }

        [TestMethod]
        public void CanRemoveKFeaturesWithTheLeastInformationGain()
        {
            var train = new List<double[]>
            {
                new double[] {1,  2,  1,  0,  3,  9,  0,  1,  2},
                new double[] {0,  5,  2,  7,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1 , 0,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  0,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  1,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  0,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  4,  0,  1,  1,  1,  1 },
                new double[] {0,  0,  0,  3,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  2,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  7,  4,  0,  0,  2,  2},
                new double[] {0,  0,  0,  1,  1,  2,  4,  5,  0 }
            };


            int[] labels = { 2, 1, 3, 4, 2, 1, 2, 4, 3, 4, 1 };
            var kFirst = 2;

            FeatureSelection.MatlabInformationGainKFirst(ref train, ref train, ref labels, kFirst);

            var results = new List<double[]>
            {
                new double[] {1,  2,  1,  3,  9,  1,  2},
                new double[] {0,  5,  2,  1,  0,  0,  1  },
                new double[] {0,  1,  1,  2,  0,  0,  1  },
                new double[] {0,  0,  1,  1,  0,  0,  1  },
                new double[] {3,  2,  5,  0,  0,  0,  2  },
                new double[] {0,  0,  2,  2,  2,  5,  1  },
                new double[] {2,  3,  0,  0,  1,  1,  1 },
                new double[] {0,  0,  0,  2,  0,  1,  2  },
                new double[] {0,  4,  3,  5,  6,  0,  3  },
                new double[] {0,  1,  0,  4,  0,  2,  2},
                new double[] {0,  0,  0,  1,  2,  5,  0 }
            };

            for (int i = 0; i < train.Count; i++)
            {
                CollectionAssert.AreEqual(train[i], results[i]);
            }

            //correct results
            //double[] resultIg = { 0.845, 0.444, 0.194, 0.012, 0.433, 0.311, 0.183, 0.242, 0.189 };
            //CollectionAssert.AreEqual(ig,resultIg);

        }

        [TestMethod]
        public void CanPlotConfusionMatrix()
        {
            const string trainlabelsPath = @"Data\Features\boc_labels_test.txt";
            const string testlabelsPath = @"Data\Features\labelsTest.txt";

            var trueLabels = Files.ReadFileTo1DArray<int>(trainlabelsPath);
            var predictedLabels = Files.ReadFileTo1DArray<int>(testlabelsPath);
            int[] categories = trueLabels.Distinct().ToArray();

            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(trueLabels, predictedLabels);
            var macroF1 = AutomaticImageClassification.Evaluation.Measures.MacroF1(trueLabels, predictedLabels, categories);
            var conf = AutomaticImageClassification.Evaluation.Measures.
                        ConfusionMatrix(trueLabels, predictedLabels, categories);

            Measures.PlotConfusionMatrix(ref conf, "plot", "a title" + accuracy + " " + macroF1, categories);

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
        public void CanExtendGeometricallyFeatures()
        {
            List<double[]> frames = new List<double[]>
            {
                new [] {0.8147, 0.9134, 0.2785, 0.9649, 0.9572, 0.1419},
                new [] {0.9058, 0.6324, 0.5469, 0.1576, 0.4854, 0.4218},
                new [] {0.1270, 0.0975, 0.9575, 0.9706, 0.8003, 0.9157}
            };

            List<double[]> descriptors = new List<double[]>
            {
                new[] {0.7094, 0.6797, 0.1190, 0.3404},
                new[] {0.7547, 0.6551, 0.4984, 0.5853},
                new[] {0.2760, 0.1626, 0.9597, 0.2238}
            };

            Normalization.ExtendGeometricalyFeatures(ref descriptors, ref frames, "xy", 600, 600);

            List<double[]> finalResultdescriptors = new List<double[]>
            {
                new[] {0.7094, 0.6797, 0.1190, 0.3404, -0.4986, -0.4985},
                new[] {0.7547, 0.6551, 0.4984, 0.5853, -0.4985, -0.4989},
                new[] {0.2760, 0.1626, 0.9597, 0.2238, -0.4998, -0.4998}
            };

            //for (int i = 0; i < descriptors.Count; i++)
            //{
            //    CollectionAssert.AreEqual(descriptors[i],finalResultdescriptors[i]);
            //}

        }

    }
}
