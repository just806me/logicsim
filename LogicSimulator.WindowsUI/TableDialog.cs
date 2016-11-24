using LogicSimulator.Main;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LogicSimulator.WindowsUI
{
    public partial class TableDialog : Form
    {
        public TableDialog(ElementValue[,] table, string[] header, int inputs_count)
        {
            InitializeComponent();

            int height = table.GetLength(0);
            int width = table.GetLength(1);
            width += table.GetLength(1) - inputs_count;

            dataGridView.ColumnCount = width;

            for (int c = 0; c < width; c++)
            {
                if (c < inputs_count)
                    dataGridView.Columns[c].HeaderText = header[c];
                else if (c % 2 == 1)
                    dataGridView.Columns[c].HeaderText = header[c - (c - inputs_count) / 2];
                else
                    dataGridView.Columns[c].HeaderText = "τ";
            }

            for (int r = 0; r < height; r++)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dataGridView);

                for (int c = 0; c < width; c++)
                {
                    if (c < inputs_count)
                        row.Cells[c].Value = Convert.ToInt16(table[r, c].Value);
                    else if (c % 2 == 1)
                        row.Cells[c].Value = Convert.ToInt16(table[r, c - (c - inputs_count) / 2].Value);
                    else
                        row.Cells[c].Value = Convert.ToInt16(table[r, c - (c - inputs_count) / 2 - 1].Delay);
                }

                dataGridView.Rows.Add(row);
            }
        }

        private void TableForm_Shown(object sender, EventArgs e)
        {
            ClientSize = new Size(
                Math.Min(
                    5 + dataGridView.Columns.Cast<DataGridViewColumn>().Sum(c => c.Width),
                    Screen.PrimaryScreen.WorkingArea.Width
                ),
                Math.Min(
                    5 + dataGridView.ColumnHeadersHeight + dataGridView.Rows.Cast<DataGridViewRow>().Sum(r => r.Height),
                    Screen.PrimaryScreen.WorkingArea.Height
                )
            );
        }
    }
}
