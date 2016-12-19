using System;
using System.Windows.Forms;

namespace VinCAD.WindowsUI
{
    public partial class EquationDialog : Form
    {
        public string Equation { get; private set; }

        public EquationDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Equation = equationTextBox.Text;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Equation = null;
            Close();
        }
    }
}
