using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace FSRMTrends
{
    public partial class MainForm : Form
    {
        private ArrayList quotaNames = new ArrayList();
        private ArrayList quotaDates = new ArrayList();
        bool clearGraphOnSelect = true;
        bool showQuotaSize = false;
        bool fromZero = false;

        public MainForm()
        {
            InitializeComponent();
            listQuotas();
        }

        public string setDebugText
        {
            get { return this.lblDebug.Text; }

            set { this.lblDebug.Text = value; }
        }

        private void listQuotas()
        {
            if (SQLDataSource.Default.ConnectionString.Equals("") || SQLDataSource.Default.FSRMServerName.Equals(""))
            {
                if (SQLDataSource.Default.FSRMServerName.Equals("") && SQLDataSource.Default.ConnectionString.Equals(""))
                {
                    MessageBox.Show("Please enter a Connection String and FSRM Server Name in Settings->Options");
                }
                else if (SQLDataSource.Default.ConnectionString.Equals(""))
                {
                    MessageBox.Show("Please enter a Connection String in Settings->Options");
                }
                else
                {
                    MessageBox.Show("Please enter a FSRM Server Name in Settings->Options");
                }
            }
            else
            {

                pnlQuotas.Controls.Clear();
                quotaNames = SQLCommander.getQuotaNames();

                int counter = 0;
                foreach (String quotaName in quotaNames)
                {
                    Button temp = new Button();
                    temp.Name = "btnPath" + counter;

                    temp.Text = quotaName;
                    temp.TextAlign = ContentAlignment.MiddleLeft;

                    temp.Width = (380);
                    temp.Click += new EventHandler(btnPath_Click);

                    pnlQuotas.Controls.Add(temp);
                    counter++;
                }
            }
        }

        private void displayGraph(String path)
        {
            ArrayList singleQuota = SQLCommander.getQuotaUsagesByPath(path);
            ArrayList singleQuotaSize = new ArrayList();

            if (showQuotaSize)
            {
                singleQuotaSize = SQLCommander.getQuotaSizesByPath(path);
            }

            txtDebug.Text = "Path: " + path + "\n";

            try
            {
                UInt64 minimum = UInt64.MaxValue;
                UInt64 maximum = UInt64.MinValue;
                UInt64 sizeMinimum = UInt64.MaxValue;
                UInt64 sizeMaximum = UInt64.MinValue;
                chart1.Series.Add(path);
                chart1.Series[path].Name = path;
                chart1.Series[path].BorderWidth = 4;
                chart1.Series[path].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                
                foreach (ArrayList usage in singleQuota)
                {
                    txtDebug.Text += ("\tDate: " + usage[0] + "\n\tUsage: " + ((UInt64)usage[1] / 1000000000f) + "\n\n");
                    chart1.Series[path].Points.AddXY((DateTime)usage[0], ((UInt64)usage[1] / 1000000000f));
                    if ((UInt64)usage[1] < minimum)
                    {
                        minimum = (UInt64)usage[1];
                    }
                    if ((UInt64)usage[1] > maximum)
                    {
                        maximum = (UInt64)usage[1];
                    }
                }


                if (showQuotaSize)
                {
                    String quotaLimit = path + " Quota Limit";
                    chart1.Series.Add(quotaLimit);
                    chart1.Series[quotaLimit].Name = quotaLimit;
                    chart1.Series[quotaLimit].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                    chart1.Series[quotaLimit].BorderWidth = 4;

                    foreach (ArrayList usage in singleQuotaSize)
                    {
                        chart1.Series[quotaLimit].Points.AddXY((DateTime)usage[0], ((UInt64)usage[1] / 1000000000f));

                        if ((UInt64)usage[1] < sizeMinimum)
                        {
                            sizeMinimum = (UInt64)usage[1];
                        }
                        if ((UInt64)usage[1] > sizeMaximum)
                        {
                            sizeMaximum = (UInt64)usage[1];
                        }
                    }
                }

                if (fromZero)
                {
                    minimum = 0;
                } else
                {
                    if (minimum > sizeMinimum)
                    {
                        minimum = sizeMinimum;
                    }
                }

                if (maximum < sizeMaximum)
                {
                    maximum = sizeMaximum;
                }

                if (minimum == maximum)
                {
                    minimum = (UInt64)(minimum * 0.9);
                }

                if (clearGraphOnSelect)
                {
                    chart1.ChartAreas[0].AxisY.Minimum = Math.Round(minimum / 1000000000f - (minimum / 1000000000f * .1), 2);
                    chart1.ChartAreas[0].AxisY.Maximum = Math.Round(maximum / 1000000000f + (maximum / 1000000000f * .1), 2);
                } else
                {
                    chart1.ChartAreas[0].AxisY.Minimum = Double.NaN;
                    chart1.ChartAreas[0].AxisY.Maximum = Double.NaN;
                    chart1.ChartAreas[0].RecalculateAxesScale();
                }

                if (chart1.ChartAreas[0].AxisY.Minimum == chart1.ChartAreas[0].AxisY.Maximum)
                {
                    chart1.ChartAreas[0].AxisY.Maximum += 0.1;
                }
                

            } catch (Exception e)
            {

            }
        }

        private void removeGraph(String path)
        {
            chart1.Series.RemoveAt(chart1.Series.IndexOf(path));
            if (chart1.Series.IndexOf(path + " Quota Limit") != -1)
            {
                chart1.Series.RemoveAt(chart1.Series.IndexOf(path + " Quota Limit"));
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            lblDebug.Text = btn.Text;

            if (clearGraphOnSelect)
            {
                chart1.Series.Clear();
            }

            displayGraph(btn.Text);
        }

        private void btnListQuotas_Click(object sender, EventArgs e)
        {
            listQuotas();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            clearGraphOnSelect = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            showQuotaSize = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            fromZero = checkBox3.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            options.Activate();
            options.Show();
        }
    }
}