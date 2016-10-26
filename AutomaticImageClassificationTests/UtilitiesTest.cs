using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void CanDivideWithSqrtSumPower()
        {
            var list = new List<double[]>();
            list.Add(new double[] { 0, 1, 1 });
            list.Add(new double[] { 0, 2, 2 });
            list.Add(new double[] { 0, 4, 5 });

            Normalization.Normalize(ref list);

        }

        [TestMethod]
        public void CanUseHellKernel()
        {
            var list = new List<double[]>();
            list.Add(new double[] { 1, 2, 3 });
            list.Add(new double[] { 0, 1, 2 });
            list.Add(new double[] { 1, 2, -2 });

            Normalization.HellKernelMapping(ref list);

            var results = new List<double[]>();
            results.Add(new[] { 1, 1.4142135623730951, 1.7320508075688772 });
            results.Add(new[] { 0, 1, 1.4142135623730951 });
            results.Add(new[] { 1, 1.4142135623730951, -1.4142135623730951 });


            for (var i = 0; i < list.Count; i++)
            {
                CollectionAssert.AreEqual(list[i].ToArray(), results[i].ToArray());
            }
        }

        [TestMethod]
        public void CanReadAllImagesFromFolder()
        {
            var searchFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound";

            string[] files = Files.GetFilesFrom(searchFolder);

            Console.WriteLine(files.Length);
            foreach (var file in files)
            {
                //double category = Convert.ToDouble(file.Split('\\')[file.Split('\\').Length - 2]);
                string cat = file.Split('\\')[file.Split('\\').Length - 2] + "\\" + file.Split('\\')[file.Split('\\').Length - 1];
                Console.WriteLine(cat);
            }

        }

        [TestMethod]
        public void CanGetSubfoldersOfPath()
        {
            var searchFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound\Test";
            string[] subfolders = Files.GetSubFolders(searchFolder);
        }

        [TestMethod]
        public void CanMapCategoriesToNumbers()
        {
            var searchFolder = @"C:\Users\l.valavanis\Desktop\personal\dbs\Clef2013\Compound\Test";
            Dictionary<string, int> mapping = Files.MapCategoriesToNumbers(searchFolder);
        }

        [TestMethod]
        public void CanWriteFileToFolder()
        {
            List<double[]> features = new List<double[]>();
            double[] lines = { 012, 1.1, 2.2 };
            double[] lines2 = { 0.0, 5.1, 2.1 };
            features.Add(lines);
            features.Add(lines2);

            string fileToWrite = @"Data\test.txt";
            Files.WriteFile(fileToWrite, features);

            string fileToWrite_2 = @"Data\test2.txt";
            string fileToWrite_2Binary = @"Data\test2binary.txt";

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



        }

        [TestMethod]
        public void CanReadFile()
        {
            string path = @"Data\train1.txt";
            var result = Files.ReadFileToListArrayList<double[]>(path);
            string fileToWrite = @"Data\testpalete.txt";
            Files.WriteFile(fileToWrite, result.ToList());

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
            string file = @"Data\test_figures.xml";
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

            var lateFusion = LateFusion.PerformLateFusion(resultsModel1, resultsModel2, weight);

            Dictionary<double, int> results = new Dictionary<double, int> { { 2, 3 }, { 101, 2 } };

            CollectionAssert.AreEqual(lateFusion, results);
        }

        [TestMethod]
        public void CanPerformFeatureSelectionUsingMostFrequent()
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
            FeatureSelection.RemoveMostFrequentFeatures(ref train, ref test, kMostFrequent);

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
        public void CanPerformFeatureSelectionUsingThreshold()
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
        public void CanPerformFeature()
        {
            const string trainDataPath = @"Data\Features\Phow_rgb_1_2_4_Lire_JavaML_1536_train.txt";
            const string testDataPath = @"Data\Features\Phow_rgb_1_2_4_Lire_JavaML_1536_test.txt";

            //MkLabRootSift_Lire_JavaML_512_test
            var trainFeat = Files.ReadFileToListArrayList<double>(trainDataPath).ToList();
            var testFeat = Files.ReadFileToListArrayList<double>(testDataPath).ToList();

            var threshold = 0.1;
            //all features ( columns ) that have more than half non zero elements are removed with threshold 0.5
            Stopwatch wat = new Stopwatch();
            wat.Start();
            FeatureSelection.RemoveMostFrequentFeaturesUsingThreshold(ref trainFeat, ref testFeat, threshold);
            wat.Stop();


        }


    }
}
