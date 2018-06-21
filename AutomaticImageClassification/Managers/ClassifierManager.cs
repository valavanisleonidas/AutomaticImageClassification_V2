using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Classifiers;

namespace AutomaticImageClassification.Managers
{
    public class ClassifierManager
    {
        public static void Init(ref BaseParameters baseParameters)
        {
            switch (baseParameters.ClassifierParameters.ClassifierMethod)
            {
                case ClassifierMethod.SVM:
                    baseParameters.ClassifierParameters.Classifier = new Classifiers.SVM(baseParameters.ClassifierParameters.LibLinearParameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class ClassifierParameters
    {
        public ClassifierMethod ClassifierMethod;
        public LibLinearParameters LibLinearParameters;
        public IClassifier Classifier;

    }
}
