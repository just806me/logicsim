using VinCAD.Main;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VinCAD.WindowsUI
{
    public partial class TimeDiagramDialog : Form
	{
        uint[,] stateChangeTimes;
        Scheme scheme;

        public TimeDiagramDialog(Scheme Scheme)
		{
			InitializeComponent();

            scheme = Scheme;

            stateChangeTimes = new uint[scheme.Inputs.Count, 2];
            for (int i = 0; i < scheme.Inputs.Count; i++)
            {
                stateChangeTimes[i, 0] = 5;
                stateChangeTimes[i, 1] = 5;
            }

            inputsComboBox.Items.AddRange(scheme.Inputs.Select(x => x.Name).ToArray());
            inputsComboBox.SelectedIndex = 0;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            const int magic = 3;

            diagramBox.Image = scheme.DrawTimeDiagram(stateChangeTimes, (uint)simulationTimeNum.Value);

            diagramBox.Size = diagramBox.Image.Size;
            panel2.Width = Math.Min(
                diagramBox.Width + magic,
                Screen.PrimaryScreen.WorkingArea.Width - SystemInformation.SizingBorderWidth - SystemInformation.VerticalScrollBarWidth - magic - panel1.Width
            );
            ClientSize = new Size(
                panel1.Width + panel2.Width,
                Math.Min(
                    Math.Max(diagramBox.Height, 177) + magic,
                    Screen.PrimaryScreen.WorkingArea.Height - SystemInformation.CaptionHeight - SystemInformation.HorizontalScrollBarHeight - magic
                )
            );
        }

        private void state0ChangeTimeNum_ValueChanged(object sender, EventArgs e)
            => stateChangeTimes[inputsComboBox.SelectedIndex, 0] = (uint)state0ChangeTimeNum.Value;

        private void state1ChangeTimeNum_ValueChanged(object sender, EventArgs e)
            => stateChangeTimes[inputsComboBox.SelectedIndex, 1] = (uint)state1ChangeTimeNum.Value;

        private void inputsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            state0ChangeTimeNum.Value = stateChangeTimes[inputsComboBox.SelectedIndex, 0];
            state1ChangeTimeNum.Value = stateChangeTimes[inputsComboBox.SelectedIndex, 1];
        }
    }
}
