using LogicSimulator.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogicSimulator.WindowsUI
{
	public partial class TimeDiagramDialog : Form
	{
		public TimeDiagramDialog(Scheme Scheme)
		{
			InitializeComponent();


			uint[] a = { 10, 6, 8, 2 };

			diagramBox.Image = Scheme.DrawTimeDiagram(a, 50);

			
			//diagramBox.Image = bt;
		}
	}
}
