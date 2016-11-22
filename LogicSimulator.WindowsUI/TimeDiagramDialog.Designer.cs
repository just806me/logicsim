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
			((System.ComponentModel.ISupportInitialize)(this.diagramBox)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// diagramBox
			// 
			this.diagramBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.diagramBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagramBox.Location = new System.Drawing.Point(0, 0);
			this.diagramBox.Name = "diagramBox";
			this.diagramBox.Size = new System.Drawing.Size(735, 284);
			this.diagramBox.TabIndex = 0;
			this.diagramBox.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.diagramBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(735, 284);
			this.panel1.TabIndex = 1;
			// 
			// TimeDiagramDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(735, 284);
			this.Controls.Add(this.panel1);
			this.Name = "TimeDiagramDialog";
			this.Text = "TimeDiagramDialog";
			((System.ComponentModel.ISupportInitialize)(this.diagramBox)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox diagramBox;
		private System.Windows.Forms.Panel panel1;
	}
}