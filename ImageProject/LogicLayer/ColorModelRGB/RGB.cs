using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globals.Interfaces;
using Globals.Enums;

namespace LogicLayer.ColorModelRGB
{
    public class RGB : IColorModel
    {
        public Bitmap Image { get; private set; }
        public Bitmap ImageStretched { get; private set; }
        public Dictionary<ColorValues, int[]> ValuesStretched { get; private set; }
        public Dictionary<ColorValues, int[]> Values { get; private set; }

        public RGB(Bitmap image, Boolean individual)
        {
            if (individual)
            {
                this.Image = image;
                this.ImageStretched = image;
                Values = GraphData(Image);
                this.HistogramStretchIndividual(ColorValues.R);
                this.HistogramStretchIndividual(ColorValues.G);
                this.HistogramStretchIndividual(ColorValues.B);
                ValuesStretched = GraphData(ImageStretched);
            }
            else
            {
                this.Image = image;
                this.ImageStretched = image;
                Values = GraphData(Image);
                this.HistogramStretch(ColorValues.RGB);
                ValuesStretched = GraphData(ImageStretched);
            }
        }

        public void HistogramStretch(ColorValues e)
        {
            Bitmap imageChange = new Bitmap(this.ImageStretched);
            int lowest = GetLowest(Values[e]);
            int highest = GetHighest(Values[e]);

            ImageStretched = HistogramStretchCalc(new Bitmap(ImageStretched), lowest, highest);
        }

        private Bitmap HistogramStretchCalc(Bitmap imageChange, int lowest, int highest)
        {
            Color p;
            for (int x = 0; x < imageChange.Width; x++)
            {
                for (int y = 0; y < imageChange.Height; y++)
                {
                    p = imageChange.GetPixel(x, y);

                    int r = (int)((p.R - lowest) * ((255 - 0.0) / (highest - lowest)) + 0);
                    int g = (int)((p.G - lowest) * ((255 - 0.0) / (highest - lowest)) + 0);
                    int b = (int)((p.B - lowest) * ((255 - 0.0) / (highest - lowest)) + 0);
                    /*
                    int r = (int)(((p.R - lowest) / (0.0 + highest - lowest)) * 255);
                    int g = (int)(((p.G - lowest) / (0.0 + highest - lowest)) * 255);
                    int b = (int)(((p.B - lowest) / (0.0 + highest - lowest)) * 255);
                    */
                    imageChange.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return imageChange;
        }

        private int GetLowest(int[] values)
        {
            int lowest = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > 0)
                {
                    break;
                }
                lowest = i;
            }
            return lowest;
        }

        private int GetHighest(int[] values)
        {
            int highest = 255;

            for (int i = values.Length - 1; i >= 0; i--)
            {
                if (values[i] > 0)
                {
                    break;
                }
                highest = i;
            }
            return highest;
        }

        public void HistogramStretchIndividual(ColorValues e)
        {
            int lowest = GetLowest(Values[e]);
            int highest = GetHighest(Values[e]);

            ImageStretched = new Bitmap(HistogramStretchIndividualCalc(e, new Bitmap(ImageStretched), lowest, highest));
        }

        private Bitmap HistogramStretchIndividualCalc(ColorValues e, Bitmap imageChange, int lowest, int highest)
        {
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
            return imageChange;
        }

        private Dictionary<ColorValues, int[]> GraphData(Bitmap image)
        {
            int[][] data = new int[4][];
            data[0] = new int[256];
            data[1] = new int[256];
            data[2] = new int[256];
            data[3] = new int[256];
            data = GraphDataCount(image, data);

            Dictionary<ColorValues, int[]> val = new Dictionary<ColorValues, int[]>();
            val.Add(ColorValues.R, data[0]);
            val.Add(ColorValues.G, data[1]);
            val.Add(ColorValues.B, data[2]);
            val.Add(ColorValues.RGB, data[3]);

            return val;
        }

        private int[][] GraphDataCount(Bitmap image, int[][] data)
        {
            Color p;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    p = image.GetPixel(x, y);
                    data[0][p.R]++;
                    data[1][p.G]++;
                    data[2][p.B]++;
                    data[3][p.R]++;
                    data[3][p.G]++;
                    data[3][p.B]++;
                }
            }
            return data;
        }
    }
}