using Globals.Enums;
using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ColorModelCMYK
{
    public class CMYK : IColorModel
    {
        public Bitmap Image { get; private set; }
        public Bitmap ImageStretched { get; private set; }
        public Dictionary<ColorValues, int[]> ValuesStretched { get; private set; }
        public Dictionary<ColorValues, int[]> Values { get; private set; }

        public CMYK(Bitmap image, Boolean individual)
        {
            if (individual)
            {
                this.Image = image;
                this.ImageStretched = image;
                Values = GraphData(Image);
                this.HistogramStretchIndividual(ColorValues.C);
                this.HistogramStretchIndividual(ColorValues.M);
                this.HistogramStretchIndividual(ColorValues.Y);
                this.HistogramStretchIndividual(ColorValues.K);
                ValuesStretched = GraphData(ImageStretched);
            }
            else
            {
                this.Image = image;
                this.ImageStretched = image;
                Values = GraphData(Image);
                this.HistogramStretch(ColorValues.CMYK);
                ValuesStretched = GraphData(ImageStretched);
            }
        }

        public void HistogramStretch(ColorValues e)
        {
            int lowest = GetLowest(Values[e]);
            int highest = GetHighest(Values[e]);
            
            ImageStretched = new Bitmap(HistogramStetchCalc(new Bitmap(ImageStretched), lowest, highest));
        }

        private Bitmap HistogramStetchCalc(Bitmap imageChange, int lowest, int highest)
        {
            Color p;
            for (int w = 0; w < imageChange.Width; w++)
            {
                for (int h = 0; h < imageChange.Height; h++)
                {
                    p = imageChange.GetPixel(w, h);
                    double k = 1 - Math.Max((p.R / 255.0), Math.Max((p.G / 255.0), (p.B / 255.0)));
                    double c = 0;
                    double m = 0;
                    double y = 0;
                    if (!((int)k == 1))
                    {
                        c = (1 - (p.R / 255.0) - k) / (1 - k);
                        m = (1 - (p.G / 255.0) - k) / (1 - k);
                        y = (1 - (p.B / 255.0) - k) / (1 - k);
                    }
                    c = (((int)(c * 100)) - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    m = (((int)(m * 100)) - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    y = (((int)(y * 100)) - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    k = (((int)(k * 100)) - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    /*
                    double cAfter = (((c2 - lowest) / (0.0 + highest - lowest)) * 100);
                    double mAfter = (((m2 - lowest) / (0.0 + highest - lowest)) * 100);
                    double yAfter = (((y2 - lowest) / (0.0 + highest - lowest)) * 100);
                    double kAfter = (((k2 - lowest) / (0.0 + highest - lowest)) * 100);
                    */
                    int red = (int)(255 * (1 - (c / 100.0)) * (1 - (k / 100.0)));
                    int green = (int)(255 * (1 - (m / 100.0)) * (1 - (k / 100.0)));
                    int blue = (int)(255 * (1 - (y / 100.0)) * (1 - (k / 100.0)));
                    imageChange.SetPixel(w, h, Color.FromArgb(red, green, blue));
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
            int highest = 100;

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
            Bitmap imageChange = new Bitmap(this.ImageStretched);
            int lowest = GetLowest(Values[e]);
            int highest = GetHighest(Values[e]);

            ImageStretched = HistogramStretchIndividualCalc(e, new Bitmap(ImageStretched), lowest, highest);
        }

        private Bitmap HistogramStretchIndividualCalc(ColorValues e, Bitmap imageChange, int lowest, int highest)
        {
            Color p;
            for (int w = 0; w < imageChange.Width; w++)
            {
                for (int h = 0; h < imageChange.Height; h++)
                {
                    p = imageChange.GetPixel(w, h);

                    double k = 1 - Math.Max((p.R / 255.0), Math.Max((p.G / 255.0), (p.B / 255.0)));
                    double c = 0;
                    double m = 0;
                    double y = 0;
                    if (!((int)k == 1))
                    {
                        c = (1 - (p.R / 255.0) - k) / (1 - k);
                        m = (1 - (p.G / 255.0) - k) / (1 - k);
                        y = (1 - (p.B / 255.0) - k) / (1 - k);
                    }

                    c = (int)(c * 100.0);
                    m = (int)(m * 100.0);
                    y = (int)(y * 100.0);
                    k = (int)(k * 100.0);

                    if (e == ColorValues.C)
                        c = (c - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    else if (e == ColorValues.M)
                        m = (m - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    else if (e == ColorValues.Y)
                        y = (y - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;
                    else if (e == ColorValues.K)
                        k = (k - lowest) * ((100 - 0.0) / (highest - lowest)) + 0;

                    int red = (int)(255 * (1 - (c / 100.0)) * (1 - (k / 100.0)));
                    int green = (int)(255 * (1 - (m / 100.0)) * (1 - (k / 100.0)));
                    int blue = (int)(255 * (1 - (y / 100.0)) * (1 - (k / 100.0)));

                    imageChange.SetPixel(w, h, Color.FromArgb(red, green, blue));
                }
            }
            return imageChange;
        }

        private Dictionary<ColorValues, int[]> GraphData(Bitmap image)
        {
            int[][] data = new int[5][];
            data[0] = new int[101];
            data[1] = new int[101];
            data[2] = new int[101];
            data[3] = new int[101];
            data[4] = new int[101];
            data = GrapDataCount(image, data);
           
            Dictionary<ColorValues, int[]> val = new Dictionary<ColorValues, int[]>();
            val.Add(ColorValues.C, data[0]);
            val.Add(ColorValues.M, data[1]);
            val.Add(ColorValues.Y, data[2]);
            val.Add(ColorValues.K, data[3]);
            val.Add(ColorValues.CMYK, data[4]);
            return val;
        }

        private int[][] GrapDataCount(Bitmap image, int[][] data)
        {
            Color p;
            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    p = image.GetPixel(w, h);
                    double k = 1 - Math.Max((p.R / 255.0), Math.Max((p.G / 255.0), (p.B / 255.0)));
                    double c = 0;
                    double m = 0;
                    double y = 0;
                    if (!((int)k == 1))
                    {
                        c = (1 - (p.R / 255.0) - k) / (1 - k);
                        m = (1 - (p.G / 255.0) - k) / (1 - k);
                        y = (1 - (p.B / 255.0) - k) / (1 - k);
                    }
                    data[4][(int)(c * 100)]++;
                    data[4][(int)(m * 100)]++;
                    data[4][(int)(y * 100)]++;
                    data[4][(int)(k * 100)]++;
                    data[0][(int)(c * 100)]++;
                    data[1][(int)(m * 100)]++;
                    data[2][(int)(y * 100)]++;
                    data[3][(int)(k * 100)]++;
                }
            }
            return data;
        }
    }
}
