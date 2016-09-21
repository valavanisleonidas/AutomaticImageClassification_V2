using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Classifiers
{
    public class LibSvm : IClassifier
    {
        public void Train(ref List<double[]> features, ref double[] labels)
        {
            throw new NotImplementedException();
        }

        public void GridSearch(ref List<double[]> features, ref double[] labels)
        {
            throw new NotImplementedException();
        }

        public void Predict(ref List<double[]> features)
        {
            throw new NotImplementedException();
        }

        public double[] GetPredictedProbabilities()
        {
            throw new NotImplementedException();
        }

        public double[] GetPredictedCategories()
        {
            throw new NotImplementedException();
        }
    }

    public class Results_
    {
        public List<double[]> Probabilities = new List<double[]>();
        public List<double> PredictedCategories = new List<double>();
        public int[] Labels;
    }

    public partial class Parameters : BaseParameters
    {
        public bool PredictProbabilities;
        public int KernelType = 2;
    }

}
