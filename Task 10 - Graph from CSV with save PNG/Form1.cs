using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace Task_10___Graph_from_CSV_with_save_PNG
{
    public partial class Form1 : Form
    {
        class PointD
        {
            public double X;
            public double Y;

            public PointD(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        void drawGraphDouble(List<PointD> points, string XAxisTitle = "x", string YAxisTitle = "y", bool PlotPoints = true)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series1 = new Series
            {
                Name = "Line",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };
            Series series2 = new Series
            {
                Name = "Points",
                Color = Color.Red,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Point,
                BorderWidth = 2
            };
            chart1.Series.Add(series1);
            if (PlotPoints)
                chart1.Series.Add(series2);
            foreach (PointD p in points)
            {
                series1.Points.AddXY(p.X, p.Y);
                series2.Points.AddXY(p.X, p.Y);
            }
            chart1.ChartAreas[0].AxisX.Title = XAxisTitle;
            chart1.ChartAreas[0].AxisY.Title = YAxisTitle;
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "CSV Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    List<PointD> points = new List<PointD>();
                    string[] titles;
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        titles = sr.ReadLine().Split(',');
                        while (!sr.EndOfStream)
                        {
                            string[] l = sr.ReadLine().Split(',');
                            points.Add(new PointD(double.Parse(l[0]), double.Parse(l[1])));
                        }
                    }
                    drawGraphDouble(points, titles[0], titles[1]);
                }
                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " failed to open.");
                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is in the wrong format.");
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is in the wrong format.");
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "PNG Files|*.PNG";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show(saveFileDialog1.FileName, " failed to save.");
                }
            }
        }
    }
}
