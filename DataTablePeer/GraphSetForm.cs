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
    public partial class GraphSetForm : Form
    {
        public GraphSetForm()
        {
            InitializeComponent();
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(Form1.AllCol);
        }
        public static Dictionary<string, int> FrecColumn = new Dictionary<string, int>();

        public static List<KeyValuePair<string, string>> X2YColumn = new List<KeyValuePair<string, string>>();


        private void button1_Click(object sender, EventArgs e)
        {
            if (label1.Text == "No can do")
                MessageBox.Show("No can do, choose 1 or 2 column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (label1.Text == "Just graph")
            {
                List<string> SelectedColumn = Form1.AllTable.AsEnumerable().Select(r => r.Field<string>(checkedListBox1.CheckedItems[0].ToString())).ToList();
                var query = from r in SelectedColumn
                            group r by r into g
                            select new { Count = g.Count(), Value = g.Key };
                FrecColumn = query.ToDictionary(r => r.Value, r => r.Count);
                ChartForm cf = new ChartForm();
                cf.Show();
                this.Close();
            }
            if (label1.Text == "X --> Y")
            {
                List<string> SelectedColumnX = Form1.AllTable.AsEnumerable().Select(r => r.Field<string>(checkedListBox1.CheckedItems[0].ToString())).ToList();
                List<string> SelectedColumnY = Form1.AllTable.AsEnumerable().Select(r => r.Field<string>(checkedListBox1.CheckedItems[1].ToString())).ToList();
                for (int i = 0; i < SelectedColumnX.Count; i++)
                {
                    X2YColumn.Add(new KeyValuePair<string, string>(SelectedColumnX[i], SelectedColumnY[i]));
                }
                ChartForm cf = new ChartForm();
                cf.Show();
                this.Close();
            }
        }




        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (checkedListBox1.CheckedItems.Count)
            {
                case 0:
                    if (e.NewValue == CheckState.Checked)
                        label1.Text = "Just graph";
                    else
                        label1.Text = "No can do";
                    break;
                case 1:
                    if (e.NewValue == CheckState.Checked)
                    {
                        label1.Text = "X --> Y";
                    }
                    else
                        label1.Text = "No can do";
                    break;
                case 2:
                    if (e.NewValue == CheckState.Checked)
                        label1.Text = "No can do";
                    else
                        label1.Text = "Just graph";
                    break;
                case 3:
                    if (e.NewValue == CheckState.Checked)
                        label1.Text = "No can do";
                    else
                        label1.Text = "X --> Y";
                    break;
                default:
                    label1.Text = "No can do";
                    break;
            }
        }
    }
}
