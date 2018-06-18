using Globals.Enums;
using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ColorModelRGB.Expirimental
{
    public class RGBSeperate : IColorModel
    {
        public Bitmap Image { get; private set; }
        public Bitmap ImageStretched { get; private set; }
        public Dictionary<ColorValues, int[]> ValuesStretched { get; private set; }
        public Dictionary<ColorValues, int[]> Values { get; private set; }

        public RGBSeperate(Bitmap image)
        {
            this.Image = image;
            this.ImageStretched = image;
            Values = GraphData(Image);
            this.HistogramStretch(ColorValues.R);
            this.HistogramStretch(ColorValues.G);
            this.HistogramStretch(ColorValues.B);
            ValuesStretched = GraphData(Image);
        }

        public void HistogramStretch(ColorValues e)
        {
            Bitmap imageChange = new Bitmap(this.ImageStretched);
            int lowest = 0;
            int highest = 255;

            int[] values = Values[e];

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > 0)
                {
                    break;
                }
                lowest = i;
            }
            for (int i = values.Length - 1; i >= 0; i--)
            {
                if (values[i] > 0)
                {
                    break;
                }
                highest = i;
            }

            Color p;

            for (int x = 0; x < imageChange.Width; x++)
            {
                for (int y = 0; y < imageChange.Height; y++)
                {
                    p = imageChange.GetPixel(x, y);

                    if (e == ColorValues.R)
                    {
                        int r = (int)((p.R - lowest) * ((255 - 0.0) / (highest - lowest)) + 0);
                        imageChange.SetPixel(x, y, Color.FromArgb(r, p.G, p.B));
                    }
                    else if (e == ColorValues.G)
                    {
                        int g = (int)((p.G - lowest) * ((255 - 0.0) / (highest - lowest)) + 0);
                        imageChange.SetPixel(x, y, Color.FromArgb(p.R, g, p.B));
                    }
                    else if (e == ColorValues.B)
                    {
                        int b = (int)((p.B - lowest) * ((255 - 0.0) / (highest - lowest)) + 0);
                        imageChange.SetPixel(x, y, Color.FromArgb(p.R, p.G, b));
                    }
                }
            }
            ImageStretched = new Bitmap(imageChange);
        }

        private Dictionary<ColorValues, int[]> GraphData(Bitmap image)
        {
            int[] dataR = new int[256];
            int[] dataG = new int[256];
            int[] dataB = new int[256];

            Color p;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    p = image.GetPixel(x, y);
                    dataR[p.R]++;
                    dataG[p.G]++;
                    dataB[p.B]++;
                }
            }

            Dictionary<ColorValues, int[]> val = new Dictionary<ColorValues, int[]>();
            val.Add(ColorValues.R, dataR);
            val.Add(ColorValues.G, dataG);
            val.Add(ColorValues.G, dataG);

            return val;
        }

        public void HistogramStretchIndividual(ColorValues k)
        {
            throw new NotImplementedException();
        }
    }
}
