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
    public class Test : IColorModel
    {
        public Bitmap Image { get; private set; }

        public Bitmap ImageStretched { get; private set; }

        public Dictionary<ColorValues, int[]> Values => throw new NotImplementedException();

        public Dictionary<ColorValues, int[]> ValuesStretched => throw new NotImplementedException();

        public Test(Bitmap image)
        {
            this.Image = image;
            this.HistogramStretch();
        }

        public void HistogramStretch()
        {
            Bitmap imageChange = new Bitmap(this.Image);
            int pixelAmount = Image.Width * Image.Height;
            int lowest = 0;
            int highest = 255;

            int[] values = this.GraphData(this.Image);
            int[] valuesCum = new int[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > 0)
                {
                    break;
                }
                lowest = i;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (i == 0)
                {
                    valuesCum[i] = values[i];
                }
                else
                {
                    valuesCum[i] = valuesCum[i - 1] + values[i];
                }
            }

            Color p;

            for (int x = 0; x < imageChange.Width; x++)
            {
                for (int y = 0; y < imageChange.Height; y++)
                {
                    p = imageChange.GetPixel(x, y);
                    int r = ((p.R - valuesCum[lowest]) / (pixelAmount - 1)) * 255;
                    int g = ((p.G - valuesCum[lowest]) / (pixelAmount - 1)) * 255;
                    int b = ((p.B - valuesCum[lowest]) / (pixelAmount - 1)) * 255;
                    imageChange.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            ImageStretched = new Bitmap(imageChange);

        }

            public int[] ImageGraphData()
            {
                return this.GraphData(this.Image);
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
                        data[p.R]++;
                        data[p.G]++;
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

