using Globals.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals.Interfaces
{
    public interface IColorModel
    {
        Bitmap Image { get; }
        Bitmap ImageStretched { get; }
        Dictionary<ColorValues, int[]> Values { get; }
        Dictionary<ColorValues, int[]> ValuesStretched { get; }

        void HistogramStretch(ColorValues k);
        void HistogramStretchIndividual(ColorValues k);
    }
}
