namespace VinCAD.WindowsUI
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.runButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.simulationTimeNum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.state1ChangeTimeNum = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.state0ChangeTimeNum = new System.Windows.Forms.NumericUpDown();
            this.inputsComboBox = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.diagramBox = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simulationTimeNum)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.state1ChangeTimeNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.state0ChangeTimeNum)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagramBox)).BeginInit();
            this.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(200, 179);
            this.panel1.TabIndex = 7;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(11, 144);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(178, 23);
            this.runButton.TabIndex = 13;
            this.runButton.Text = Resource.Localization.OK;
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = Resource.Localization.TimeDiagramDialog_LabelTime;
            // 
            // simulationTimeNum
            // 
            this.simulationTimeNum.Location = new System.Drawing.Point(95, 118);
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
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.state1ChangeTimeNum);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.state0ChangeTimeNum);
            this.groupBox1.Controls.Add(this.inputsComboBox);
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 100);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = Resource.Localization.TimeDiagramDialog_GroupBoxInputs;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = Resource.Localization.TimeDiagramDialog_Label_State1ChangeTime;
            // 
            // state1ChangeTimeNum
            // 
            this.state1ChangeTimeNum.Location = new System.Drawing.Point(115, 72);
            this.state1ChangeTimeNum.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.state1ChangeTimeNum.Name = "state1ChangeTimeNum";
            this.state1ChangeTimeNum.Size = new System.Drawing.Size(57, 20);
            this.state1ChangeTimeNum.TabIndex = 7;
            this.state1ChangeTimeNum.ValueChanged += new System.EventHandler(this.state1ChangeTimeNum_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = Resource.Localization.TimeDiagramDialog_Label_State0ChangeTime;
			// 
			// state0ChangeTimeNum
			// 
			this.state0ChangeTimeNum.Location = new System.Drawing.Point(115, 46);
            this.state0ChangeTimeNum.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.state0ChangeTimeNum.Name = "state0ChangeTimeNum";
            this.state0ChangeTimeNum.Size = new System.Drawing.Size(57, 20);
            this.state0ChangeTimeNum.TabIndex = 5;
            this.state0ChangeTimeNum.ValueChanged += new System.EventHandler(this.state0ChangeTimeNum_ValueChanged);
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
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.diagramBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(206, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(529, 179);
            this.panel2.TabIndex = 8;
            // 
            // diagramBox
            // 
            this.diagramBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.diagramBox.Location = new System.Drawing.Point(3, 0);
            this.diagramBox.Name = "diagramBox";
            this.diagramBox.Size = new System.Drawing.Size(526, 179);
            this.diagramBox.TabIndex = 7;
            this.diagramBox.TabStop = false;
            // 
            // TimeDiagramDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 179);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = global::VinCAD.WindowsUI.Properties.Resources.component1;
            this.Name = "TimeDiagramDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = Resource.Localization.TimeDiagramDialog_Name;
			this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simulationTimeNum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.state1ChangeTimeNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.state0ChangeTimeNum)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagramBox)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown simulationTimeNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown state0ChangeTimeNum;
        private System.Windows.Forms.ComboBox inputsComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown state1ChangeTimeNum;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox diagramBox;
    }
}