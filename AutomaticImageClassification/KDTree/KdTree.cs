using System;
using System.Collections.Generic;
using System.Linq;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.KDTree
{
    //from vlfeat
    public class KdTree : IKdTree
    {
        private bool _isRandomizedTree;
        private MWStructArray _kdtree;
        private List<double[]> _vocab;

        public KdTree()
        {
            _isRandomizedTree = true;
        }

        public KdTree(bool isRandomizedTree)
        {
            _isRandomizedTree = isRandomizedTree;
        }

        public KdTree(List<double[]> vocab)
        {
            _isRandomizedTree = true;
            _vocab = vocab;
        }

        public KdTree(MWStructArray kdtree, List<double[]> vocab)
        {
            _kdtree = kdtree;
            _vocab = vocab;
        }

        public void CreateTree(List<double[]> centers)
        {
            _vocab = centers;
            try
            {
                var kdtree = new MatlabAPI.KdTree();

                MWArray[] result = kdtree.CreateTree(1,
                    new MWNumericArray(centers.ToArray()),
                    new MWLogicalArray(_isRandomizedTree));

                _kdtree = (MWStructArray)result[0];

                result = null;
                kdtree.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        //returns index - 1 because matlab index starts from 1, not 0 
        public int SearchTree(double[] centroid)
        {
            try
            {
                var kdtree = new MatlabAPI.KdTree();

                MWArray[] result = kdtree.SearchTree(2,
                    _kdtree,
                    new MWNumericArray(_vocab.ToArray()),
                    new MWNumericArray(centroid));

                int index = (int)((MWNumericArray)result[0]).ToVector(MWArrayComponent.Real).GetValue(0);
                //double distance = (double)((MWNumericArray)result[1]).ToVector(MWArrayComponent.Real).GetValue(0);

                result = null;
                kdtree.Dispose();
                return index - 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //returns index - 1 because matlab index starts from 1, not 0 
        public List<int> SearchTree(List<double[]> centers)
        {
            try
            {
                var kdtree = new MatlabAPI.KdTree();

                MWArray[] result = kdtree.SearchTree(2,
                    _kdtree,
                    new MWNumericArray(_vocab.ToArray()),
                    new MWNumericArray(centers.ToArray()));

                int[] indexes = (int[])((MWNumericArray)result[0]).ToVector(MWArrayComponent.Real);
                //double[] distances = (double[])((MWNumericArray)result[1]).ToVector(MWArrayComponent.Real);

                result = null;
                kdtree.Dispose();
                return indexes.Select(a => a - 1).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return "VlFeatKdTree" + (_isRandomizedTree ? "_Randomized" : "");
        }

    }

}



