using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts.Helpers;
using System.Windows.Media;
using Globals.Interfaces;
using LogicLayer.ColorModelRGB;
using LogicLayer.ColorModelCMYK;
using Globals.Enums;

namespace MainForm
{
    public partial class Form1 : Form
    {
        public IColorModel model;

        private bool checkBoxChanged = false;

        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
            saveFileDialog1.Title = "Save Processed Image";
            comboBox1.SelectedIndex = 0;
            richTextBox1.Text = "Please select an image.";
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Reload(openFileDialog1.FileName);
            }
            else
            {
                return;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (model != null)
            {
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    PickFileExtension(fs);
                    fs.Close();
                }
            }
        }

        private void PickFileExtension(System.IO.FileStream fs)
        {
            switch (saveFileDialog1.FilterIndex)
            {
                case 1:
                    model.ImageStretched.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case 2:
                    model.ImageStretched.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case 3:
                    model.ImageStretched.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case 4:
                    model.ImageStretched.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (model != null)
            {
                Reload();
            }
        }

        private void Reload()
        {
            if (model != null)
            {
                if ((string)comboBox1.SelectedItem == "CMYK" && (model is RGB || this.checkBoxChanged))
                {
                    richTextBox1.ForeColor = System.Drawing.Color.Black;
                    richTextBox1.Text = "Calculating, this could take a while...";
                    richTextBox1.Refresh();
                    model = new CMYK(model.Image, checkBox1.Checked);
                    checkBoxChanged = false;
                }
                else if ((string)comboBox1.SelectedItem == "RGB" && (model is CMYK || this.checkBoxChanged))
                {
                    richTextBox1.ForeColor = System.Drawing.Color.Black;
                    richTextBox1.Text = "Calculating, this could take a while...";
                    richTextBox1.Refresh();
                    model = new RGB(model.Image, checkBox1.Checked);
                    checkBoxChanged = false;
                }
                else
                    return;
                LoadImageGraph();
            }  
        }

        private void Reload(string path)
        {
            if ((string)comboBox1.SelectedItem == "CMYK")
            {
                richTextBox1.ForeColor = System.Drawing.Color.Black;
                richTextBox1.Text = "Calculating, this could take a while...";
                richTextBox1.Refresh();
                model = new CMYK(new Bitmap(path), checkBox1.Checked);
                checkBoxChanged = false;
            }
            else if ((string)comboBox1.SelectedItem == "RGB")
            {
                richTextBox1.ForeColor = System.Drawing.Color.Black;
                richTextBox1.Text = "Calculating, this could take a while...";
                richTextBox1.Refresh();
                model = new RGB(new Bitmap(path), checkBox1.Checked);
                checkBoxChanged = false;
            }
            LoadImageGraph();
        }

        private void LoadImageGraph()
        {
            pictureBox1.Image = model.Image;
            pictureBox2.Image = model.ImageStretched;

            DrawChart1();
            DrawChart2();
            richTextBox1.ForeColor = System.Drawing.Color.Green;
            richTextBox1.Text = "Done stretching histogram!";
        }


        private void DrawChart1()
        {
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisY.Clear();
            cartesianChart1.AxisY.Add(new Axis
            {
                MinValue = 0
            });

            cartesianChart1.Series.Add(new LineSeries
            {
                Title = (string)comboBox1.SelectedItem,
                Values = model.Values.Last().Value.AsChartValues(),
                LineSmoothness = 0, //straight lines, 1 really smooth lines
                PointGeometry = null,
                PointGeometrySize = 0,
                StrokeThickness = 1
            });
        }


        private void DrawChart2()
        {
            cartesianChart2.Series.Clear();

            cartesianChart2.AxisY.Clear();
            cartesianChart2.AxisY.Add(new Axis
            {
                MinValue = 0
            });
            
            cartesianChart2.Series.Add(new LineSeries
            {
                Title = (string)comboBox1.SelectedItem,
                Values = model.ValuesStretched.Last().Value.AsChartValues(),
                LineSmoothness = 0, //straight lines, 1 really smooth lines
                PointGeometry = null,
                PointGeometrySize = 0,
                StrokeThickness = 1
            });
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxChanged = !this.checkBoxChanged;
        }

    }
}
