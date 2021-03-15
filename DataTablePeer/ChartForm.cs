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

namespace DataTablePeer
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            try
            {
                InitializeComponent();
                chart1.Series.Clear();
                Dictionary<string, List<double>> X2Y = new Dictionary<string, List<double>>();
                if (GraphSetForm.FrecColumn.Count != 0)
                {
                    this.Text = GraphSetForm.ChartName1;
                    foreach (var item in GraphSetForm.FrecColumn)
                    {
                        chart1.Series.Add(item.Key);
                        chart1.Series[item.Key].Points.AddY(item.Value);
                    }
                    GraphSetForm.FrecColumn.Clear();
                }
                else
                {
                    if (double.TryParse(GraphSetForm.X2YColumn.FirstOrDefault().Key.Trim('k').Trim('m').Replace('.', ','), out double v1))
                    {
                        this.Text = GraphSetForm.ChartName1;
                        foreach (var item in GraphSetForm.X2YColumn)
                        {
                            if (double.TryParse(item.Key.Trim('k').Trim('m').Replace('.', ','), out double val))
                            {
                                if (!X2Y.ContainsKey(item.Value))
                                    X2Y.Add(item.Value, new List<double>() { val });
                                else
                                {
                                    X2Y[item.Value].Add(val);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No real number value! Choose column with real number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                    }
                    else if (double.TryParse(GraphSetForm.X2YColumn.FirstOrDefault().Value.Trim('k').Trim('m').Replace('.', ','), out double v2))
                    {
                        this.Text = GraphSetForm.ChartName2;
                        foreach (var item in GraphSetForm.X2YColumn)
                        {
                            if (double.TryParse(item.Value.Trim('k').Trim('m').Replace('.', ','), out double val))
                            {
                                if (!X2Y.ContainsKey(item.Key))
                                    X2Y.Add(item.Key, new List<double>() { val });
                                else
                                {
                                    X2Y[item.Key].Add(val);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No real number value! Choose column with real number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Must one column have real number value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    foreach (var item in X2Y)
                    {
                        chart1.Series.Add(item.Key);
                        chart1.Series[item.Key].Points.AddY(item.Value.Average());
                    }


                }
            }
            catch
            {
                MessageBox.Show("No real number value! Choose column with real number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Images|*.png";
                sfd.ShowDialog();
                chart1.SaveImage(sfd.FileName, ChartImageFormat.Png);
            }
            catch
            {
                MessageBox.Show("Something go wrong, plz try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                var r = chart1.HitTest(e.X, e.Y);
                if (r.ChartElementType == ChartElementType.DataPoint)
                {
                    ColorDialog cd = new ColorDialog();
                    cd.ShowDialog();
                    chart1.Series[r.Series.Name].Color = cd.Color;
                }
            }
            catch
            {
                MessageBox.Show("Something go wrong, plz try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
