using System.Collections.Generic;


namespace AutomaticImageClassification.Classifiers
{
    public interface IClassifier
    {
        void Train(ref List<double[]> features,ref double[] labels);
        void GridSearch(ref List<double[]> features,ref  double[] labels);
        void Predict(ref List<double[]> features);

        double[] GetPredictedProbabilities();
        double[] GetPredictedCategories();
        List<double[]> GetPredictedScores();

    }

    public enum ClassifierMethod
    {
        LibLinear,
        LibSvm
    }


}
