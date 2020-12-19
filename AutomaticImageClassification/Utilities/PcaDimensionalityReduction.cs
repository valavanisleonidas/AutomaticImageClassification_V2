using Accord.Statistics.Analysis;


namespace AutomaticImageClassification.Utilities
{
    //Some users would like to analyze huge amounts of data.In this case,
    //computing the SVD directly on the data could result in memory exceptions or excessive computing times. 
    //If your data's number of dimensions is much less than the number of observations (i.e. your matrix have less columns than rows) 
    //then it would be a better idea to summarize your data in the form of a covariance or correlation matrix and compute PCA using the EVD.
    //
    //
    //EXAMPLE
    //
    //
    // Create the Principal Component Analysis 
    // specifying the CovarianceMatrix method:
    //var pca = new PrincipalComponentAnalysis()
    //{
    //    Method = PrincipalComponentMethod.CovarianceMatrix,
    //    Means = mean // pass the original data mean vectors
    //};
    //// Learn the PCA projection using passing the cov matrix
    //MultivariateLinearRegression transform = pca.Learn(cov);
    //// Now, we can transform data as usual
    //double[][] actual = pca.Transform(data);

    public class PcaDimensionalityReduction
    {
        private static PrincipalComponentAnalysis _pca;
        private static double _explainedVariancePercentage;

        public PcaDimensionalityReduction()
        {
            // create an analysis with centering (covariance method)
            // but no standardization (correlation method) and whitening:
            _pca = new PrincipalComponentAnalysis
            {
                Method = PrincipalComponentMethod.Center,
                Whiten = true
            };
        }

        public PcaDimensionalityReduction(PcaMethod method, bool doWhiten, double explainedVariancePercentage)
        {
            PrincipalComponentMethod accordMethod;

            switch (method)
            {
                case PcaMethod.Center:
                    accordMethod = PrincipalComponentMethod.Center;
                    break;
                case PcaMethod.CorrelationMatrix:
                    accordMethod = PrincipalComponentMethod.CorrelationMatrix;
                    break;
                case PcaMethod.CovarianceMatrix:
                    accordMethod = PrincipalComponentMethod.CovarianceMatrix;
                    break;
                case PcaMethod.KernelMatrix:
                    accordMethod = PrincipalComponentMethod.KernelMatrix;
                    break;
                case PcaMethod.Standardize:
                    accordMethod = PrincipalComponentMethod.Standardize;
                    break;
                default:
                    accordMethod = PrincipalComponentMethod.Center;
                    break;
            }

            _pca = new PrincipalComponentAnalysis
            {
                Method = accordMethod,
                Whiten = doWhiten
            };
            _explainedVariancePercentage = explainedVariancePercentage;
        }
        
        public void ComputePca(ref double[][] data)
        {
            //if (data.Length > data[0].Length && _pca.Method != PrincipalComponentMethod.CovarianceMatrix)
            //{
            //   _pca.Method = PrincipalComponentMethod.CovarianceMatrix;
            //}
            
            // learn the linear projection from the data
            _pca.Learn(data);
            //limit to percentage of explained variance
            _pca.ExplainedVariance = _explainedVariancePercentage;
            // project data
            data = _pca.Transform(data);
        }

        public enum PcaMethod
        {
            Center,
            CorrelationMatrix,
            CovarianceMatrix,
            KernelMatrix,
            Standardize
        }
    }
}
