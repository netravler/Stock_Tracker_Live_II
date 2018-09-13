using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//
using System.Net;
using System.Windows.Forms.DataVisualization.Charting;

namespace Stock_Tracker_Live_II
{
    public partial class Form1 : Form
    {
        private Timer _Timer = new Timer();
        public int chart1_advance = 1;
        string[] holdticker;
        string[] holdchart1;

        public Form1()
        {
            InitializeComponent();
            // chart stuff
            ChartArea area = new ChartArea("First");
            chart1.ChartAreas.Add(area);
            chart1.Name="test";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _Timer.Interval = 6000;
            _Timer.Tick += new EventHandler(_Timer_Tick);
            _Timer.Start();

            //name the chart once here

            // parse for use in chart1
            //string[] holdchart1 = holdstufffromcsv.Split('+');
            
            holdticker = textBox1.Text.Split('+');

            foreach (string tickd in holdticker)
            {
                //if (chart1.Series.FindByName(holdchart1[4]) == null)
                if (chart1.Series.FindByName(tickd) == null)
                {
                    //http://www.youtube.com/watch?v=82jnryBxsnI
                    //http://www.codeproject.com/Articles/5431/A-flexible-charting-library-for-NET

                    //Series series = this.chart1.Series.Add(holdchart1[4]);
                    chart1.Series.Add(tickd).ChartType = SeriesChartType.Line;
                }
            }
            // end of chart prep
        }

        void _Timer_Tick(object sender, EventArgs e)
        {
            // read into richtextbox
            string holdchart1_old = ""; // prime for headings must not have duplicate...


            foreach (string tickd in holdticker)
            {
                WebClient clientTXT = new WebClient();
                var holdstufffromcsv = clientTXT.DownloadString("http://quote.yahoo.com/d/quotes.csv?s=" + tickd + "&f=snl1d1t1cv");
                richTextBox1.Text += holdstufffromcsv;

                // split for putting into a chart
                //string[] holdchart1 = holdstufffromcsv.Split(',');
                string[] holdchart1 = holdstufffromcsv.Split('"');

                //holdticker = textBox1.Text.Split('\');

                //Series series = this.chart1.Series.Add(holdchart1[4]);

                holdchart1[4] = holdchart1[4].Replace(",","");

                chart1.Series[tickd].Points.AddXY(holdchart1[7], Convert.ToDouble(holdchart1[4]));

                clientTXT.Dispose();
             }

            
        }

    }
}
