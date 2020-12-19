using AutomaticImageClassification.Classifiers;
using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Utilities
{
    public class LearningCurves
    {
        public static void PlotLearningCurve(List<double[]> train, List<double[]> test,
                                        double[] train_cats, double[] test_cats,
                                        SvmParameters parameters)
        {

            try
            {
                var learningCurve = new MatlabAPI.LearningCurve();

                learningCurve.LearningCurves
                        (   
                            new MWNumericArray(train.ToArray()),
                            new MWNumericArray(test.ToArray()),
                            new MWNumericArray(train_cats),
                            new MWNumericArray(test_cats),
                            new MWNumericArray(parameters.Gamma),
                            new MWNumericArray(parameters.Cost),
                            new MWNumericArray(parameters.SolverType),
                            new MWNumericArray(parameters.BiasMultiplier)
                        );

            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
