using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticImageClassification.Classifiers
{
    public abstract class BaseParameters
    {
        public double Gamma;
        public double Cost;
        public double CvAccuracy = 0;
    }
}
