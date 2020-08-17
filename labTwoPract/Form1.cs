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

namespace labTwoPract
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void makeChart()
        {
            double x = 0, y = 0, xStart = -7, xEnd = 3, division = 0.01;
            double r = double.Parse(textBox2.Text);
            x = xStart;
            listBox1.Items.Clear();
            this.chart1.Series[0].Points.Clear();
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.Crossing = 0;
            chart1.ChartAreas[0].AxisY.Crossing = 0;
            while (x <= xEnd)
            {
                if (x >= -7 && x < -6)
                {
                    y = 1;
                }
                if (x >= -6 && x <= -4)
                {
                    y = (-x - Math.Pow(r, 2)) / 2;
                }
                if (x > -4 && x < 0)
                {
                    y = Math.Sqrt(Math.Pow(r, 2) - Math.Pow((x + r), 2));
                }
                if (x >= 0 && x <= 2)
                {
                    y = -Math.Sqrt(Math.Abs(Math.Pow(r / 2, 2) - Math.Pow((x - r / 2), 2)));
                }
                if (x > 2 && x <= 3)
                {
                    y = (-x + r) / 2;
                }
                listBox1.Items.Add("X = " + Math.Round(x, 3).ToString() + " Y = " + Math.Round(y, 3).ToString());
                this.chart1.Series[0].Points.AddXY(x, y);
                x += division;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            makeChart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            makeChart();
            double y = double.NaN;
            double r = double.Parse(textBox2.Text), x = double.Parse(textBox1.Text);
            if (x < -6)
            {
                y = 1;
            }
            if (x >= -6 && x <= -4)
            {
                y = (-x - Math.Pow(r, 2)) / 2;
            }
            if (x > -4 && x < 0)
            {
                y = Math.Sqrt(Math.Pow(r, 2) - Math.Pow((x + r), 2));
            }
            if (x >= 0 && x <= 2)
            {
                y = -Math.Sqrt(Math.Abs(Math.Pow(r / 2, 2) - Math.Pow((x - r / 2), 2)));
            }
            if (x > 2)
            {
                y = (-x + r) / 2;
            }
            // Красим найденную позицию
            var spoints = chart1.Series[0].Points.Where(el => Math.Round(el.YValues.Min(), 4) == Math.Round(y,4)).ToList();
            foreach(DataPoint el in spoints)
            {
                if (Math.Round(el.XValue, 4) == x)
                {
                    el.Color = Color.Red;
                    el.MarkerSize = 10;
                    el.MarkerStyle = MarkerStyle.Diamond;

                }
            }
            label4.Text = "Результат Y = " + Math.Round(y, 3).ToString();
        }

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart1.HitTest(pos.X, pos.Y, false,
                                         ChartElementType.PlottingArea);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    var xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                    var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

                    tooltip.Show("X=" + Math.Round(xVal, 2) + ", Y=" + Math.Round(yVal, 2), this.chart1,
                                 pos.X, pos.Y - 15);
                }
            }
        }
    }
}
