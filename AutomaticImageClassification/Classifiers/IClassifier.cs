using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Classifiers
{
    public interface IClassifier
    {
        void Train(ref List<double[]> features,ref double[] labels);
        void GridSearch(ref List<double[]> features,ref  double[] labels);
        void Predict(ref List<double[]> features);

        double[] GetPredictedProbabilities();
        int[] GetPredictedCategories();

    }
}
