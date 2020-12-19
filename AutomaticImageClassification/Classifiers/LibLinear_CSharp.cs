//using Accord.Statistics.Analysis;
//using Accord.Statistics;
//using Accord.Statistics.Kernels;
//using Accord.Controls;
//using Accord.MachineLearning.VectorMachines.Learning;
//using System.Collections.Generic;
//using AutomaticImageClassification.Utilities;
//using Accord.MachineLearning.VectorMachines;
//using System.Diagnostics;
//using System.Linq;

//using System;
//using System.IO;
//using System.Text;


//namespace AutomaticImageClassification.Classifiers
//{
//    public class LibLinear_CSharp
//    {


//        public int[] example(List<double[]> train, List<double[]> test, int[] trainL, int[] testL)
//        {
//            HomogeneousKernelMap map = new HomogeneousKernelMap();

//            List<double[]> train_new = new List<double[]>();
//            List<double[]> test_new = new List<double[]>();

//            int counter = 0;
//            foreach (double[] t in train)
//            {
//                train_new.Add(map.Evaluate(t));
//                counter++;
//            }

//            counter = 0;
//            foreach (double[] t in test)
//            {

//                test_new.Add(map.Evaluate(t));
//                counter++;
//            }
//            train = train_new;
//            test = test_new;

//            double[][] tr = train.ToArray();
//            tr = Arrays.TransposeMatrix(ref tr);

//            double[][] te = test.ToArray();
//            te = Arrays.TransposeMatrix(ref te);

//            train = Arrays.ConvertDoubleArrayToList(ref tr);
//            test = Arrays.ConvertDoubleArrayToList(ref te);


//            Normalization.Normalize(ref train);
//            Normalization.Normalize(ref test);

//            tr = train.ToArray();
//            tr = Arrays.TransposeMatrix(ref tr);

//            te = test.ToArray();
//            te = Arrays.TransposeMatrix(ref te);



//            train = Arrays.ConvertDoubleArrayToList(ref tr);
//            test = Arrays.ConvertDoubleArrayToList(ref te);



//            double[][] problem =
//               {
//                //             a    b    a + b
//                new double[] { 0,   0,     0    },
//                new double[] { 0,   1,     0    },
//                new double[] { 1,   0,     0    },
//                new double[] { 1,   1,     1    },
//            };

//            // Plot the problem on screen
//            //ScatterplotBox.Show("AND", new double[] { 5,4,3 }, new double[] { 1,2,3}).Hold();

//            double[][] inputs =
//            {
//                //               input         output
//                new double[] { 0, 1, 1, 0 }, //  0 
//                new double[] { 0, 1, 0, 0 }, //  0
//                new double[] { 0, 0, 1, 0 }, //  0
//                new double[] { 0, 1, 1, 0 }, //  0
//                new double[] { 0, 1, 0, 0 }, //  0
//                new double[] { 1, 0, 0, 0 }, //  1
//                new double[] { 1, 0, 0, 0 }, //  1
//                new double[] { 1, 0, 0, 1 }, //  1
//                new double[] { 0, 0, 0, 1 }, //  1
//                new double[] { 0, 0, 0, 1 }, //  1
//                new double[] { 1, 1, 1, 1 }, //  2
//                new double[] { 1, 0, 1, 1 }, //  2
//                new double[] { 1, 1, 0, 1 }, //  2
//                new double[] { 0, 1, 1, 1 }, //  2
//                new double[] { 1, 1, 1, 1 }, //  2
//            };

//            int[] outputs = // those are the class labels
//                        {
//                0, 0, 0, 0, 0,
//                1, 1, 1, 1, 1,
//                2, 2, 2, 2, 2,
//            };

//            // Create a one-vs-one multi-class SVM learning algorithm 
//            var teacher = new MulticlassSupportVectorLearning<Linear>()
//            {
//                // using LIBLINEAR's L2-loss SVC dual for each SVM
//                Learner = (p) => new LinearDualCoordinateDescent()
//                {
//                    Loss = Loss.L2,
//                    Complexity = 16

//                }

//            };
//            // Learn a machine
//            var machine = teacher.Learn(train.ToArray(), trainL);
//            train.Clear();
//            // Obtain class predictions for each sample
//            int[] predicted = machine.Decide(test.ToArray());


//            // Compute classification accuracy
//            double acc = new GeneralConfusionMatrix(expected: testL, predicted: predicted).Accuracy;


//            var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(testL, predicted);
//            var macrof1 = AutomaticImageClassification.Evaluation.Measures.MacroF1(testL, predicted, trainL);

//            return predicted;

//        }


//        public MulticlassSupportVectorMachine<Linear> train(List<double[]> train, int[] trainL)
//        {
//            Stopwatch watch = new Stopwatch();
//            watch.Start();
//            train = transformData(train);
//            watch.Stop();
                

//            // Create a one-vs-one multi-class SVM learning algorithm 
//            var teacher = new MulticlassSupportVectorLearning<Linear>()
//            {
//                // using LIBLINEAR's L2-loss SVC dual for each SVM
//                Learner = (p) => new LinearDualCoordinateDescent()
//                {
//                    Complexity = 16

//                }

//            };
//            // Learn a machine
//            Stopwatch watch1 = new Stopwatch();
//            watch1.Start();

//            var machine = teacher.Learn(train.ToArray(), trainL);

//            watch1.Stop();

//            var a = watch.Elapsed;
//            var b = watch1.Elapsed;

//            return machine;

//        }

//        public int[] decide(MulticlassSupportVectorMachine<Linear>  machine, List<double[]> test)
//        {
//            Stopwatch watch = new Stopwatch();
//            watch.Start();
//            test = transformData(test);
//            watch.Stop();

//            var a = watch.Elapsed;
//            // Obtain class predictions for each sample
//            int[] predicted = machine.Decide(test.ToArray());


//            //// Compute classification accuracy
//            //double acc = new GeneralConfusionMatrix(expected: testL, predicted: predicted).Accuracy;


//            //var accuracy = AutomaticImageClassification.Evaluation.Measures.Accuracy(testL, predicted);
//            //var macrof1 = AutomaticImageClassification.Evaluation.Measures.MacroF1(testL, predicted, trainL);

//            return predicted;
//        }

//        public List<double[]> transformData(List<double[]> data)
//        {
            
//            HomogeneousKernelMap map = new HomogeneousKernelMap();

//            data = map.Evaluate(data);
            

//            data = Accord.Math.Matrix.Transpose(data).ToList();
            
//            Normalization.Normalize(ref data);

//            data = Accord.Math.Matrix.Transpose(data).ToList();

//            return data;
//        }



//    }
//}
