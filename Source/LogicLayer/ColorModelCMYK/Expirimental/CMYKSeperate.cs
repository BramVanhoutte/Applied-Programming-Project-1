using Globals.Enums;
using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ColorModelCMYK.Expirimental
{
    class CMYKSeperate : IColorModel
    {
        public Bitmap Image { get; private set; }
        public Bitmap ImageStretched { get; private set; }
        public Dictionary<ColorValues, int[]> ValuesStretched { get; private set; }
        public Dictionary<ColorValues, int[]> Values { get; private set; }

        public CMYKSeperate(Bitmap image)
        {
            this.Image = image;
            this.ImageStretched = image;
            Values = GraphData(Image);
            this.HistogramStretch(ColorValues.C);
            this.HistogramStretch(ColorValues.M);
            this.HistogramStretch(ColorValues.Y);
            this.HistogramStretch(ColorValues.K);
            ValuesStretched = GraphData(Image);
        }

        public void HistogramStretch(ColorValues e)
        {
            Bitmap imageChange = new Bitmap(this.Image);
            int lowest = 0;
            int highest = 100;

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

            for (int w = 0; w < imageChange.Width; w++)
            {
                for (int h = 0; h < imageChange.Height; h++)
                {
                    p = imageChange.GetPixel(w, h);

                    double r = p.R / 255.0;
                    double g = p.G / 255.0;
                    double b = p.B / 255.0;
                    double k = 1 - Math.Max(r, Math.Max(g, b));
                    double c = (1 - r - k) / (1 - k);
                    double m = (1 - g - k) / (1 - k);
                    double y = (1 - b - k) / (1 - k);

                    int c2 = (int)(c * 100);
                    int m2 = (int)(m * 100);
                    int y2 = (int)(y * 100);
                    int k2 = (int)(k * 100);

                    double cAfter;
                    double mAfter;
                    double yAfter;
                    double kAfter;

                    if (e == ColorValues.C)
                    {
                        cAfter = (c2 - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                        mAfter = m2;
                        yAfter = y2;
                        kAfter = k2;
                    }
                    else if (e == ColorValues.M)
                    {
                        cAfter = c2;
                        mAfter = (m2 - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                        yAfter = y2;
                        kAfter = k2;
                    }
                    else if (e == ColorValues.Y)
                    {
                        cAfter = c2;
                        mAfter = m2;
                        yAfter = (y2 - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                        kAfter = k2;
                    }
                    else if (e == ColorValues.K)
                    {
                        cAfter = c2;
                        mAfter = m2;
                        yAfter = y2;
                        kAfter = (k2 - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    }
                    else {
                        cAfter = c2;
                        mAfter = m2;
                        yAfter = y2;
                        kAfter = k2;
                    }

                    double cAfter2 = cAfter / 100.0;
                    double mAfter2 = mAfter / 100.0;
                    double yAfter2 = yAfter / 100.0;
                    double kAfter2 = kAfter / 100.0;

                    int red = (int)(255 * (1 - cAfter2) * (1 - kAfter2));
                    int green = (int)(255 * (1 - mAfter2) * (1 - kAfter2));
                    int blue = (int)(255 * (1 - yAfter2) * (1 - kAfter2));

                    imageChange.SetPixel(w, h, Color.FromArgb(red, green, blue));
                }
            }
            ImageStretched = new Bitmap(imageChange);
        }

        private Dictionary<ColorValues, int[]> GraphData(Bitmap image)
        {
            int[] data = new int[101];
            int[] dataC = new int[101];
            int[] dataM = new int[101];
            int[] dataY = new int[101];
            int[] dataK = new int[101];

            Color p;

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    p = image.GetPixel(w, h);
                    double r = p.R / 255.0;
                    double g = p.G / 255.0;
                    double b = p.B / 255.0;
                    double k = 1 - Math.Max(r, Math.Max(g, b));
                    double c = (1 - r - k) / (1 - k);
                    double m = (1 - g - k) / (1 - k);
                    double y = (1 - b - k) / (1 - k);
                    dataC[(int)(c * 100)]++;
                    dataM[(int)(m * 100)]++;
                    dataY[(int)(y * 100)]++;
                    dataK[(int)(k * 100)]++;
                }
            }
            Dictionary<ColorValues, int[]> val = new Dictionary<ColorValues, int[]>();
            val.Add(ColorValues.C, dataC);
            val.Add(ColorValues.M, dataM);
            val.Add(ColorValues.Y, dataY);
            val.Add(ColorValues.K, dataK);
            return val;
        }

        public void HistogramStretchIndividual(ColorValues k)
        {
            throw new NotImplementedException();
        }
    }
}
