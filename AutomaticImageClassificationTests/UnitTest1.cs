using System;
using System.Collections.Generic;
using System.Linq;
using boclibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticImageClassification.Utilities;
using net.sf.javaml.core.kdtree;


namespace AutomaticImageClassificationTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanCreatePalette()
        {
            string file = "as.txt";
            var sampleImgs = new string[]{ @"C:\Users\l.valavanis\Desktop\Leo Files\images\Photos\1.JPG" };
            int[][] palete = BoCLibrary.createPalette(file,sampleImgs,64,10,10,true,ColorConversion.ColorSpace.RGB);

            Console.WriteLine("palete");
            foreach (int[] i in palete)
            {
                Console.WriteLine(i[0] + " " + i[1] + " " + i[2]);
            }

            List<double[]> colors = AutomaticImageClassification.Utilities.Arrays.ConvertIntArrayToDoubleList(palete);
            
            Console.WriteLine("lsit");
            Console.WriteLine("--------------------");
            foreach (double[] color in colors)
            {
                Console.WriteLine(color[0] + " " + color[1] + " " + color[2]);
            }
            
            KDTree tree = AutomaticImageClassification.Utilities.KDTreeImplementation.createTree(colors);

            int index =AutomaticImageClassification.Utilities.KDTreeImplementation.SearchTree(new double[] {100, 100 ,100}, tree);
            Console.WriteLine("closest to 100 100 100");
            Console.WriteLine(palete[index][0] + " " + palete[index][1] + " " + palete[index][2] + " " );


            Clusterer dictionary = BoCLibrary.createDictionary(file, sampleImgs, 64, 10, 10, palete,
                ColorConversion.ColorSpace.RGB, tree);


            

        }
    }
}
