namespace LogicSimulator.WindowsUI
{
	partial class TimeDiagramDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.diagramBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.runButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.simulationTimeNum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.stateChangeTimeNum = new System.Windows.Forms.NumericUpDown();
            this.inputsComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.diagramBox)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simulationTimeNum)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stateChangeTimeNum)).BeginInit();
            this.SuspendLayout();
            // 
            // diagramBox
            // 
            this.diagramBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.diagramBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.diagramBox.Location = new System.Drawing.Point(196, 0);
            this.diagramBox.Name = "diagramBox";
            this.diagramBox.Size = new System.Drawing.Size(539, 152);
            this.diagramBox.TabIndex = 6;
            this.diagramBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.runButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.simulationTimeNum);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 152);
            this.panel1.TabIndex = 7;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(11, 117);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(178, 23);
            this.runButton.TabIndex = 13;
            this.runButton.Text = "OK";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Simulation time";
            // 
            // simulationTimeNum
            // 
            this.simulationTimeNum.Location = new System.Drawing.Point(94, 91);
            this.simulationTimeNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.simulationTimeNum.Name = "simulationTimeNum";
            this.simulationTimeNum.Size = new System.Drawing.Size(95, 20);
            this.simulationTimeNum.TabIndex = 11;
            this.simulationTimeNum.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.stateChangeTimeNum);
            this.groupBox1.Controls.Add(this.inputsComboBox);
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 73);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "State change time";
            // 
            // stateChangeTimeNum
            // 
            this.stateChangeTimeNum.Location = new System.Drawing.Point(106, 46);
            this.stateChangeTimeNum.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.stateChangeTimeNum.Name = "stateChangeTimeNum";
            this.stateChangeTimeNum.Size = new System.Drawing.Size(66, 20);
            this.stateChangeTimeNum.TabIndex = 5;
            this.stateChangeTimeNum.ValueChanged += new System.EventHandler(this.stateChangeTimeNum_ValueChanged);
            // 
            // inputsComboBox
            // 
            this.inputsComboBox.FormattingEnabled = true;
            this.inputsComboBox.Location = new System.Drawing.Point(6, 19);
            this.inputsComboBox.Name = "inputsComboBox";
            this.inputsComboBox.Size = new System.Drawing.Size(166, 21);
            this.inputsComboBox.TabIndex = 4;
            this.inputsComboBox.SelectedIndexChanged += new System.EventHandler(this.inputsComboBox_SelectedIndexChanged);
            // 
            // TimeDiagramDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 152);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.diagramBox);
            this.Name = "TimeDiagramDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TimeDiagramDialog";
            ((System.ComponentModel.ISupportInitialize)(this.diagramBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simulationTimeNum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stateChangeTimeNum)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.PictureBox diagramBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown simulationTimeNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown stateChangeTimeNum;
        private System.Windows.Forms.ComboBox inputsComboBox;
    }
}