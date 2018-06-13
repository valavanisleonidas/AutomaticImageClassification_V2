using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.FusionTypes;
using AutomaticImageClassification.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void CanNormalize()
        {
            var list = new List<double[]> { new double[] { 0, 1, 1 }, new double[] { 0, 2, 2 }, new double[] { 0, 4, 5 } };
            Normalization.Normalize(ref list);
  
            var results = new List<double[]> {
                new double[] { 0, 0.2182, 0.1826 },
                new double[] { 0, 0.4364, 0.3651 },
                new double[] { 0, 0.8729, 0.9129 }
            };

            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Select(d => Math.Round(d,4)).ToArray();
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }

        }

        [TestMethod]
        public void CanRenormalize()
        {
            var list = new List<double[]> { new double[] { 0, 1, 1 }, new double[] { 0, 2, 2 }, new double[] { 0, 4, 5 } };
            var sigmoid = 0.2;
            Normalization.ReNormalize(ref list, sigmoid);
            
            var results = new List<double[]> {
                new double[] { 0.5000, 0.5498, 0.5498 },
                new double[] { 0.5000, 0.5987, 0.5987 },
                new double[] { 0.5000, 0.6900, 0.7311 }
            };

            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Select(d => Math.Round(d, 4)).ToArray();
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }
        }

        [TestMethod]
        public void CanUseHellKernel()
        {
            var list = new List<double[]> { new double[] { 1, 2, 3 }, new double[] { 0, 1, 2 }, new double[] { 1, 2, -2 } };
            Normalization.HellKernelMapping(ref list);

            var results = new List<double[]>
            {
                new[] {1, 1.4142135623730951, 1.7320508075688772},
                new[] {0, 1, 1.4142135623730951},
                new[] {1, 1.4142135623730951, -1.4142135623730951}
            };

            for (var i = 0; i < list.Count; i++)
            {
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }
        }

        [TestMethod]
        public void CanReadAllImagesFromFolder()
        {
            var searchFolder = @"Data\Database";
            string[] files = Files.GetFilesFrom(searchFolder);
            
            Assert.AreEqual(files.Length, 5);
            
        }

        [TestMethod]
        public void CanGetSubfoldersOfPath()
        {
            var searchFolder = @"Data\Database";
            string[] subfolders = Files.GetSubFolders(searchFolder);

            Assert.AreEqual(subfolders.Length, 0);
        }

        [TestMethod]
        public void CanMapCategoriesToNumbers()
        {
            var searchFolder = @"Data";
            Dictionary<string, int> mapping = Files.MapCategoriesToNumbers(searchFolder);

            Dictionary<string, int> results = new Dictionary<string, int>();
            results.Add("Comparison", 1);
            results.Add("database",2);
            results.Add("Dictionaries", 3);
            results.Add("Features",4);
            results.Add("Palettes", 5);
            results.Add("test results", 6);
            results.Add("textData", 7);

            CollectionAssert.AreEquivalent(mapping, results);

        }

        [TestMethod]
        public void CanWriteFileToFolder()
        {
            List<double[]> features = new List<double[]>();
            double[] lines = { 12, 1.1, 2.2 };
            double[] lines2 = { 0.0, 5.1, 2.1 };
            features.Add(lines);
            features.Add(lines2);

            string fileToWrite = @"Data\test.txt";
            Files.WriteFile(fileToWrite, features);

            string fileToWrite_2 = @"Data\test2.txt";
            //string fileToWrite_2Binary = @"Data\test2binary.txt";

            Files.WriteFile(fileToWrite, features);

            foreach (var doublese in features)
            {
                Files.WriteAppendFile(fileToWrite_2, doublese);
                //Files.WriteAppendBinaryFile(fileToWrite_2Binary, doublese);   
            }

            List<double> categories = new List<double>();
            categories.Add(1);
            categories.Add(3);
            string fileToWrite2 = @"Data\categories.txt";
            Files.WriteFile(fileToWrite2, categories);
            
            //TODO
        }

        [TestMethod]
        public void CanReadFile()
        {
            List<double[]> list = new List<double[]>();

            list.Add(new double[] { 5, 2, 3 });
            list.Add(new double[] { 5, 2, 3 });
            list.Add(new double[] { 5, 1, 3 });
            list.Add(new double[] { 5, 4, 3 });
            list.Add(new double[] { 5, 4, 3 });
            list.Add(new double[] { 2, 4, 1 });
            list.Add(new double[] { 3, 2, 3 });

            string path = @"Data\test_listFile.txt";
            Files.WriteFile(path,list);

            var result = Files.ReadFileToListArrayList<double>(path);

            for (int i = 0; i < result.Count; i++)
            {
                CollectionAssert.AreEqual(list[i], result[i]);
            }
        }

        [TestMethod]
        public void TestNormalizationSquareArray()
        {
            double[,] arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            double[][] arr = Arrays.ToJaggedArray(ref arr1);

            var a = Normalization.SqrtArray(ref arr[1]);

            Assert.AreEqual(a, new double[] { 2, 2.23, 2.44 });

        }


        [TestMethod]
        public void TestNormalization()
        {
            double[,] arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            double[][] arr = Arrays.ToJaggedArray(ref arr1);

            Normalization.SqrtArray(ref arr[1]);

            var norm1 = Normalization.ComputeL1Norm(ref arr[0]);
            var norm2 = Normalization.ComputeL2Norm(ref arr[0]);

            Normalization.NormalizeArray(ref arr[0], ref norm1);
            Normalization.Tfidf(ref arr);

            Console.WriteLine(arr);
            //TODO add assert

        }

        [TestMethod]
        public void TestDistinctColors()
        {
            List<double[]> list = new List<double[]>();
            list.Add(new double[] { 5, 2, 3 });
            list.Add(new double[] { 5, 2, 3 });

            list.Add(new double[] { 5, 1, 3 });

            list.Add(new double[] { 5, 4, 3 });
            list.Add(new double[] { 5, 4, 3 });

            list.Add(new double[] { 2, 4, 1 });

            list.Add(new double[] { 3, 2, 3 });

            Arrays.GetDistinctObjects(ref list);

            Assert.AreEqual(list.Count, 5);
        }

        [TestMethod]
        public void CanReadXml()
        {
            string file = @"Data\textData\test_figures.xml";
            Figures images = XmlFiguresReader.ReadXml<Figures>(file);
        }

        [TestMethod]
        public void CanPerformEarlyFusion()
        {
            List<double[]> resultsModel1 = new List<double[]>();
            List<double[]> resultsModel2 = new List<double[]>();

            resultsModel1.Add(new double[] { 1, 2, 3 });
            resultsModel1.Add(new double[] { 1, 2, 3 });

            resultsModel2.Add(new double[] { 0, 1, 1 });
            resultsModel2.Add(new double[] { 1, 200, 1 });
            var concat = EarlyFusion.ConcatArrays(ref resultsModel1, ref resultsModel2);

            List<double[]> concatResultList = new List<double[]>();

            concatResultList.Add(new double[] { 1, 2, 3, 0, 1, 1 });
            concatResultList.Add(new double[] { 1, 2, 3, 1, 200, 1 });

            for (int i = 0; i < concatResultList.Count; i++)
            {
                CollectionAssert.AreEqual(concat[i], concatResultList[i]);
            }

        }

        [TestMethod]
        public void CanPerformLateFusion()
        {
            List<double[]> resultsModel1 = new List<double[]>();
            List<double[]> resultsModel2 = new List<double[]>();

            resultsModel1.Add(new double[] { 1, 2, 3 });
            resultsModel1.Add(new double[] { 1, 2, 3 });

            resultsModel2.Add(new double[] { 0, 1, 1 });
            resultsModel2.Add(new double[] { 1, 200, 1 });

            double weight = 0.5;
            double sigmoid = 0.2;
            var lateFusion = LateFusion.PerformLateFusion(ref resultsModel1, ref resultsModel2, weight, sigmoid);

            //var arrayElementsNum = 100;
            //double[] weights = Enumerable.Range(0, arrayElementsNum).Select(v => (double)v / arrayElementsNum).ToArray();
            //double[] sigmoids = Enumerable.Range(0, arrayElementsNum).Select(v => (double)v / arrayElementsNum).ToArray();
            //LateFusion.PerformLateFusion(ref resultsModel1, ref resultsModel2, weight, sigmoid);

            Dictionary<double, int> results = new Dictionary<double, int> { { 2, 3 }, { 101, 2 } };

            CollectionAssert.AreEqual(lateFusion, results);
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
            FeatureSelection.RemoveKMostFrequentFeatures(ref train, ref test, kMostFrequent);

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
            FeatureSelection.RemoveMostFrequentFeaturesUsingThreshold(ref train, ref test, threshold);

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

            FeatureSelection.InformationGainThreshold(ref train, ref train, ref labels, threshold);

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
            int[] categories = { 1, 2, 3, 4 };
            var kFirst = 2;

            FeatureSelection.InformationGainKFirst(ref train, ref train, ref labels, kFirst);

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
            
        }

        [TestMethod]
        public void CanComputeInformationGain()
        {
            var train = new List<double[]>
            {
                new double[] {1,  2,  1,  0,  3,  9,  0,  1,  2  },
                new double[] {0,  5,  2,  7,  1,  0,  2,  0,  1  },
                new double[] {0,  1,  1 , 0,  2,  0,  1,  0,  1  },
                new double[] {0,  0,  1,  0,  1,  0,  4,  0,  1  },
                new double[] {3,  2,  5,  1,  0,  0,  1,  0,  2  },
                new double[] {0,  0,  2,  0,  2,  2,  1,  5,  1  },
                new double[] {2,  3,  0,  4,  0,  1,  1,  1,  1  },
                new double[] {0,  0,  0,  3,  2,  0,  1,  1,  2  },
                new double[] {0,  4,  3,  2,  5,  6,  1,  0,  3  },
                new double[] {0,  1,  0,  7,  4,  0,  0,  2,  2  },
                new double[] {0,  0,  0,  1,  1,  2,  4,  5,  0  }
            };

            int[] labels = { 2, 1, 3, 4, 2, 1, 2, 4, 3, 4, 1 };
            int[] categories = { 1, 2, 3, 4 };

            var ig = FeatureSelection.InformationGain(ref train, ref labels, categories);

            //correct results
            double[] resultIg = { 0.845, 0.444, 0.194, 0.012, 0.433, 0.311, 0.183, 0.242, 0.189 };
            
            //keep only 3 digits 
            ig = ig.Select(d => Math.Truncate(d * 1000d) / 1000d ).ToArray();

            CollectionAssert.AreEqual(ig,resultIg);
        }

        [TestMethod]
        public void CanPerformFeatureSelectionUsingThresholdCompareMatlabCSharp()
        {

            var trainDataPath = @"Data\Features\lboc_50_1024_train_libsvm_test.txt";
            var testDataPath = @"Data\Features\lboc_50_1024_test_libsvm_test.txt";
            var trainlabelsPath = @"Data\Features\boc_labels_train_libsvm_test.txt";
            
            //MkLabRootSift_Lire_JavaML_512_test
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            var trainlabels = Files.ReadFileTo1DArray<int>(trainlabelsPath);

            var threshold = 0.1;
            var train = trainFeat;
            var test = testFeat;
            //all features ( columns ) that have more than half non zero elements are removed with threshold 0.1
            Stopwatch wat = new Stopwatch();
            wat.Start();
            FeatureSelection.InformationGainThreshold(ref trainFeat, ref testFeat, ref trainlabels, threshold);
            wat.Stop();

            Stopwatch _wat = new Stopwatch();
            _wat.Start();
            FeatureSelection.MatlabInformationGainUsingThreshold(ref train, ref test, ref trainlabels, threshold);
            _wat.Stop();

            for (int i = 0; i < trainFeat.Count; i++)
            {
                CollectionAssert.AreEqual(trainFeat[i], train[i]);
            }

            for (int i = 0; i < test.Count; i++)
            {
                CollectionAssert.AreEqual(testFeat[i], test[i]);
            }

        }

        [TestMethod]
        public void CanPerformFeatureSelectionKFirstCompareMatlabCSharp()
        {

            var trainDataPath = @"Data\Features\lboc_50_1024_train_libsvm_test.txt";
            var testDataPath = @"Data\Features\lboc_50_1024_test_libsvm_test.txt";

            var trainlabelsPath = @"Data\Features\boc_labels_train_libsvm_test.txt";
           
            
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            var trainlabels = Files.ReadFileTo1DArray<int>(trainlabelsPath);

            var kFirst = 100;
            var train = trainFeat;
            var test = testFeat;
            //all features ( columns ) that have more than half non zero elements are removed with threshold 0.5
            Stopwatch wat = new Stopwatch();
            wat.Start();
            FeatureSelection.InformationGainKFirst(ref trainFeat, ref testFeat, ref trainlabels, kFirst);
            wat.Stop();

            Stopwatch _wat = new Stopwatch();
            _wat.Start();
            FeatureSelection.MatlabInformationGainKFirst(ref train, ref test, ref trainlabels, kFirst);
            _wat.Stop();

            for (int i = 0; i < trainFeat.Count; i++)
            {
                CollectionAssert.AreEqual(trainFeat[i], train[i]);
            }

            for (int i = 0; i < test.Count; i++)
            {
                CollectionAssert.AreEqual(testFeat[i], test[i]);
            }

        }

        [TestMethod]
        public void CanComputePearsonCorrelation()
        {
            //A_1 = [10 200 7 150];
            //A_2 = [0.001 0.450 0.007 0.200];
            //R = corrcoef(A_1, A_2)
            //matlab result = 0.9568


            var a = new double[] { 10, 200, 7, 150 };
            var b = new double[] { 0.001, 0.450, 0.007, 0.200 };
            
            var result = PearsonCorrelationCoefficient.Compute(ref a, ref b);
            
            Assert.AreEqual(result, 0.956,0.001);
        }

        [TestMethod]
        public void CanComputePcaDimensionalityReduction()
        {
            const string trainDataPath = @"Data\Features\MkLabRootSift_Lire_JavaML_512_train.txt";
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToArray();

            var pca = new PcaDimensionalityReduction(PcaDimensionalityReduction.PcaMethod.Center, true, 0.8);
            pca.ComputePca(ref trainFeat);
        }

        [TestMethod]
        public void CanResizeSaveImage()
        {
            string imagePath = @"Data\database\einstein.jpg";
            string savedImagePath = @"Data\test_ResizedImage.jpg";

            int width = 256;
            int height = 200;

            Bitmap image = new Bitmap(imagePath);
            Bitmap im = ImageProcessing.ResizeImage(image, width, height);
            ImageProcessing.SaveImage(im, savedImagePath);

            Bitmap savedImage = new Bitmap(savedImagePath);

            Assert.AreEqual(savedImage.Height, height);
            Assert.AreEqual(savedImage.Width, width);
        }

        [TestMethod]
        public void CanResizeImageFixedHeight()
        {
            string imagePath = @"Data\database\einstein.jpg";
            string savedImagePath = @"Data\test_ResizedImageFixedHeight.jpg";

            int height = 550;

            Bitmap ima = new Bitmap(imagePath);
            Bitmap resizedImage = (Bitmap)ImageProcessing.ResizeImageFixedHeight(ima, 550);
            ImageProcessing.SaveImage(resizedImage, savedImagePath);

            Bitmap savedImage = new Bitmap(savedImagePath);

            Assert.AreEqual(savedImage.Height, height);

        }

        [TestMethod]
        public void CanSplitImage()
        {
            int blokX = 1;
            int blokY = 1;
            string imagePath = @"Data\database\einstein.jpg";
            Bitmap img = new Bitmap(imagePath);
            img = ImageProcessing.ResizeImage(img, 256, 256);

            List<Image> res = new List<Image>();
            int pembagiLebar = (int)Math.Ceiling((float)img.Width / (float)blokX);
            int pembagiTinggi = (int)Math.Ceiling((float)img.Height / (float)blokY);


            for (int i = 0; i < pembagiLebar; i++)//baris
            {
                for (int j = 0; j < pembagiTinggi; j++)//kolom
                {
                    Bitmap bmp = new Bitmap(blokX, blokY);

                    using (Graphics grp = Graphics.FromImage(bmp))
                    {
                        grp.DrawImage(img, 0, 0, new Rectangle(i * blokX, j * blokY, blokX, blokY), GraphicsUnit.Pixel);
                    }

                    res.Add(bmp);
                }
            }
            
            
        }

        [TestMethod]
        public void CanGetFileNameWithoutExtension()
        {
            var name = @"C:\Users\leonidas\Downloads\test.1.2-2017.jpg";
            name = Files.GetFileNameWithoutExtension(name);
            Assert.AreEqual(name, "test.1.2-2017");
        }

        [TestMethod]
        public void CanGetExtension()
        {
            var name = @"C:\Users\leonidas\Downloads\test.1.2-2017.jpg";
            name = Files.GetExtension(name);
            Assert.AreEqual(name, ".jpg");

        }

    }
}
