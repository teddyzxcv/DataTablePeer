using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                if (GraphSetForm.FrecColumn.Count != 0)
                    foreach (var item in GraphSetForm.FrecColumn)
                    {
                        chart1.Series.Add(item.Key);
                        chart1.Series[item.Key].Points.AddY(item.Value);
                    }
                else
                    foreach (var item in GraphSetForm.X2YColumn)
                    {

                    }
            }
            catch
            {
                MessageBox.Show("No real number value! Choose column with real number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
