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
        public TableDialog(ElementValue[,] table, string[] header)
        {
            InitializeComponent();

            int height = table.GetLength(0);
            int width = table.GetLength(1);

            dataGridView.ColumnCount = width;

            for (int c = 0; c < width; c++)
                dataGridView.Columns[c].HeaderText = header[c];

            for (int r = 0; r < height; r++)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dataGridView);

                for (int c = 0; c < width; c++)
                    row.Cells[c].Value = Convert.ToInt16(table[r, c].Value);

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
