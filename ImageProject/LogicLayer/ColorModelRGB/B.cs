using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globals.Enums;

namespace LogicLayer.ColorModelRGB
{
    public class B : IColorModel
    {
        public Bitmap Image { get; private set; }

        public Bitmap ImageStretched { get; private set; }

        public Dictionary<ColorValues, int[]> Values => throw new NotImplementedException();

        public Dictionary<ColorValues, int[]> ValuesStretched => throw new NotImplementedException();

        public B(Bitmap image)
        {
            this.Image = image;
            this.HistogramStretch();
        }

        public void HistogramStretch()
        {
            Bitmap imageChange = new Bitmap(this.Image);
            int lowest = 0;
            int highest = 255;

            int[] values = this.GraphData(Image);

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
                    int b = (p.B - lowest) * ((255 - 0) / (highest - lowest)) + 0;
                    imageChange.SetPixel(x, y, Color.FromArgb(p.R, p.G, b));
                }
            }
            ImageStretched = new Bitmap(imageChange);
        }

        public int[] ImageGraphData()
        {
            return this.GraphData(Image);
        }

        public int[] ImageStretchedGraphData()
        {
            return this.GraphData(ImageStretched);
        }

        private int[] GraphData(Bitmap image)
        {
            int[] data = new int[256];

            Color p;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    p = image.GetPixel(x, y);
                    data[p.B]++;
                }
            }
            return data;
        }

        public void HistogramStretch(ColorValues k)
        {
            throw new NotImplementedException();
        }

        public void HistogramStretchIndividual(ColorValues k)
        {
            throw new NotImplementedException();
        }
    }
}
