using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace DataTablePeer
{
    public partial class Form1 : Form
    {
        public static string[] AllCol = new string[1];

        public static DataTable AllTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
            label5.Text = string.Empty;
            label6.Text = string.Empty;
            label7.Text = string.Empty;
            label8.Text = string.Empty;
        }
        public static DataTable ProcessCsv(string csvInput, bool firstRowContainsFieldNames = true)
        {
            DataTable result = new DataTable();
            using (var csvReader = new StringReader(csvInput))
            using (var tfp = new NotVisualBasic.FileIO.CsvTextFieldParser(csvReader))
            {
                // Get Some Column Names
                if (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();
                    AllCol = fields;
                    for (int i = 0; i < fields.Count(); i++)
                    {
                        if (firstRowContainsFieldNames)
                            result.Columns.Add(fields[i]);
                        else
                            result.Columns.Add("Col" + i);
                    }

                    // If first line is data then add it
                    if (!firstRowContainsFieldNames)
                        result.Rows.Add(fields);
                }
                // Get Remaining Rows
                while (!tfp.EndOfData)
                    result.Rows.Add(tfp.ReadFields());
            }
            return result;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                string csvsource = File.ReadAllText(ofd.FileName);
                AllTable = ProcessCsv(csvsource);
                dataGridView1.DataSource = AllTable;
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(AllCol);
            }
            catch
            {
                MessageBox.Show("Plz, try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GraphSetForm gsf = new GraphSetForm();
                gsf.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Plz, try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private double StandardDiv(List<double> doublelist)
        {
            double ret = 0;
            int count = doublelist.Count();
            if (count > 1)
            {
                //Compute the Average
                double avg = doublelist.Average();
                //Perform the Sum of (value-avg)^2
                double sum = doublelist.Sum(d => (d - avg) * (d - avg));
                //Put it all together
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
        private double Dispersion(List<double> doublelist)
        {
            var query = from r in doublelist
                        group r by r into g
                        select new { Count = (double)g.Count() / (double)doublelist.Count, Value = g.Key };
            var dic = query.ToDictionary(r => r.Value, r => r.Count);
            double Mx = dic.Sum(e => e.Key * e.Value);
            double Dx = dic.Sum(e => e.Value * (e.Key - Mx) * (e.Key - Mx));
            return Dx;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            List<string> SelectedColumn = Form1.AllTable.AsEnumerable().Select(r => r.Field<string>(comboBox1.SelectedItem.ToString())).ToList();
            if (double.TryParse(SelectedColumn[0], out double x))
            {
                var doublelist = SelectedColumn.Select(double.Parse).ToList();
                var mean = doublelist.Average();
                int midindex = doublelist.Count / 2 + 1;
                var median = (doublelist.OrderBy(r => r).ToList()[midindex] + doublelist.OrderByDescending(r => r).ToList()[midindex]) / 2;
                var stdiv = StandardDiv(doublelist);
                var disper = Dispersion(doublelist);
                label5.Text = mean.ToString();
                label6.Text = median.ToString();
                label7.Text = stdiv.ToString();
                label8.Text = disper.ToString();
            }
            else
            {
                label5.Text = "No can do";
                label6.Text = "No can do";
                label7.Text = "No can do";
                label8.Text = "No can do";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}