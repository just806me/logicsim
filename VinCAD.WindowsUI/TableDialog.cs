using VinCAD.Main;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace VinCAD.WindowsUI
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

			for (int c = 0, h = 0; c < width; c++, h++)
			{
				if (c < inputs_count)
				{
					dataGridView.Columns[c].HeaderText = header[h];
				}
				else
				{
					dataGridView.Columns[c].HeaderText = header[h];
					c++;
					dataGridView.Columns[c].HeaderText = "τ";
				}

			}

			for (int r = 0; r < height; r++)
			{
				var row = new DataGridViewRow();
				row.CreateCells(dataGridView);

				for (int c = 0, h = 0; c < width; c++, h++)
				{
					if (c < inputs_count)
					{
						row.Cells[c].Value = Convert.ToInt16(table[r, h].Value);
					}
					else
					{
						row.Cells[c].Value = table[r, h].isSet ? Convert.ToInt16(table[r, h].Value).ToString() : "null";
						c++;
						row.Cells[c].Value = Convert.ToInt16(table[r, h].Delay);
					}
				}

				dataGridView.Rows.Add(row);
			}

			dataGridView.GetType()
				.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
				.SetValue(dataGridView, true, null);

		}

		private void TableForm_Shown(object sender, EventArgs e)
		{
			ClientSize = new Size(
				Math.Min(
					2 + SystemInformation.SizingBorderWidth + SystemInformation.VerticalScrollBarWidth + dataGridView.Columns.Cast<DataGridViewColumn>().Sum(c => c.Width),
					Screen.PrimaryScreen.WorkingArea.Width - SystemInformation.SizingBorderWidth - SystemInformation.VerticalScrollBarWidth - 2
				),
				Math.Min(
					2 + SystemInformation.SizingBorderWidth + SystemInformation.VerticalScrollBarWidth + dataGridView.ColumnHeadersHeight + dataGridView.Rows.Cast<DataGridViewRow>().Sum(r => r.Height),
					Screen.PrimaryScreen.WorkingArea.Height - SystemInformation.CaptionHeight - SystemInformation.HorizontalScrollBarHeight - 2
				)
			);
		}
	}
}
