using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticImageClassification.Feature.Global.BrightnessTextureDirectionalityHistogram
{
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>
    /// Filter interface
    /// </summary>
    public interface Cfilter
    {
        Bitmap Apply(Bitmap img);
    }

}
