using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Classifiers;
using LibSVMsharp;

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
                case ClassifierMethod.LibSVM:
                    baseParameters.ClassifierParameters.Classifier = new LibSvm(baseParameters.ClassifierParameters.SvmParameter);
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
        public SVMParameter SvmParameter;
        public IClassifier Classifier;

    }
}
