using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathWorks.MATLAB.NET.Arrays;

namespace AutomaticImageClassification.Utilities
{
    public class Quantization
    {
        public static double[] CombineQuantizations(List<double[]> frames, List<int> indexes, int width, int height, int clusterNum,
            int[,] numSpatialX, int[,] numSpatialY)
        {
            try
            {
                var quantizations = new MatlabAPI.Quantizations();

                //return frames descriptors( features )
                MWArray[] result = quantizations.CombineQuantizations(1,
                    new MWNumericArray(frames.ToArray()),
                    new MWNumericArray(new[] { indexes.Select(a => a + 1).ToArray() }),
                    new MWNumericArray(width),
                    new MWNumericArray(height),
                    new MWNumericArray(clusterNum),
                    new MWNumericArray(numSpatialX),
                    new MWNumericArray(numSpatialY)
                    );

                quantizations.Dispose();
                return (double[])((MWNumericArray)result[0]).ToVector(MWArrayComponent.Real);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
