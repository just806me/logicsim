using System;
using System.Windows.Forms;

namespace VinCAD.WindowsUI
{
    public partial class NameDialog : Form
    {
        public string NewName { get; private set; }

        public NameDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            NewName = nameTextBox.Text;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            NewName = null;
            Close();
        }
    }
}
