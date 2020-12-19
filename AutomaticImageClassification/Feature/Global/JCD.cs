using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Global
{
    public class JCD : IGlobalFeatures
    {
        private CEDD cedd;
        private FCTH fcth;
        
        public JCD()
        {
            cedd = new CEDD();
            fcth = new FCTH();
        }


        public double[] ExtractHistogram(LocalBitmap input)
        {
            var cedd_descr = cedd.ExtractHistogram(input);
            var fcth_descr = fcth.ExtractHistogram(input);

            return JointHistograms(cedd_descr, fcth_descr);
        }

        public double[] JointHistograms(double[] CEDD, double[] FCTH)
        {

            double[] JointDescriptor = new double[168];

            double[] TempTable1 = new double[24];
            double[] TempTable2 = new double[24];
            double[] TempTable3 = new double[24];
            double[] TempTable4 = new double[24];

            for (int i = 0; i < 24; i++)
            {
                TempTable1[i] = FCTH[0 + i] + FCTH[96 + i];
                TempTable2[i] = FCTH[24 + i] + FCTH[120 + i];
                TempTable3[i] = FCTH[48 + i] + FCTH[144 + i];
                TempTable4[i] = FCTH[72 + i] + FCTH[168 + i];

            }
            
            for (int i = 0; i < 24; i++)
            {
                JointDescriptor[i] = (TempTable1[i] + CEDD[i]) / 2; //ok
                JointDescriptor[24 + i] = (TempTable2[i] + CEDD[48 + i]) / 2; //ok
                JointDescriptor[48 + i] = CEDD[96 + i]; //ok
                JointDescriptor[72 + i] = (TempTable3[i] + CEDD[72 + i]) / 2;//ok
                JointDescriptor[96 + i] = CEDD[120 + i]; //ok
                JointDescriptor[120 + i] = TempTable4[i];//ok
                JointDescriptor[144 + i] = CEDD[24 + i];//ok

            }
            return (JointDescriptor);
        }


        public override string ToString()
        {
            return "JCD";
        }


    }
}
