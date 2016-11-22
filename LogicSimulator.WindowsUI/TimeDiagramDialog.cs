using LogicSimulator.Main;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LogicSimulator.WindowsUI
{
    public partial class TimeDiagramDialog : Form
	{
        uint[] stateChangeTimes;
        Scheme scheme;

        public TimeDiagramDialog(Scheme Scheme)
		{
			InitializeComponent();

            scheme = Scheme;

            stateChangeTimes = new uint[scheme.Inputs.Count];
            for (int i = 0; i < stateChangeTimes.Length; i++)
                stateChangeTimes[i] = 5;

            inputsComboBox.Items.AddRange(scheme.Inputs.Select(x => x.Name).ToArray());
            inputsComboBox.SelectedIndex = 0;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            diagramBox.Image = scheme.DrawTimeDiagram(stateChangeTimes, (uint)simulationTimeNum.Value);
            diagramBox.Size = diagramBox.Image.Size;

            ClientSize = new Size(
                 Math.Min(
                    panel1.Width + diagramBox.Image.Width + 20,
                    Screen.PrimaryScreen.WorkingArea.Width
                ),
                Math.Min(
                    Math.Max(diagramBox.Image.Height, 152) + 40,
                    Screen.PrimaryScreen.WorkingArea.Height
                )
            );
        }

        private void stateChangeTimeNum_ValueChanged(object sender, EventArgs e) 
            => stateChangeTimes[inputsComboBox.SelectedIndex] = (uint)stateChangeTimeNum.Value;

        private void inputsComboBox_SelectedIndexChanged(object sender, EventArgs e) 
            => stateChangeTimeNum.Value = stateChangeTimes[inputsComboBox.SelectedIndex];
    }
}
