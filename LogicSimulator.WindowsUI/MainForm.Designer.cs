namespace LogicSimulator.WindowsUI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAsHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromEquationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.schemeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearSchemeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox = new System.Windows.Forms.ListBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.moveButton = new System.Windows.Forms.ToolStripButton();
            this.addButton = new System.Windows.Forms.ToolStripButton();
            this.connectionButton = new System.Windows.Forms.ToolStripButton();
            this.deleteButton = new System.Windows.Forms.ToolStripButton();
            this.elementMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteElementMenuStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearElementMenuStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.mainMenuStrip.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.schemeMenuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.elementMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.printToolStripMenuItem,
            this.exportAsHTMLToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // exportAsHTMLToolStripMenuItem
            // 
            this.exportAsHTMLToolStripMenuItem.Name = "exportAsHTMLToolStripMenuItem";
            this.exportAsHTMLToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.exportAsHTMLToolStripMenuItem.Text = "Export as HTML";
            this.exportAsHTMLToolStripMenuItem.Click += new System.EventHandler(this.exportAsHTMLToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(154, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromEquationToolStripMenuItem,
            this.calculateToolStripMenuItem,
            this.timeDiagramToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.editToolStripMenuItem.Text = "Tools";
            // 
            // fromEquationToolStripMenuItem
            // 
            this.fromEquationToolStripMenuItem.Name = "fromEquationToolStripMenuItem";
            this.fromEquationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fromEquationToolStripMenuItem.Text = "From equation";
            this.fromEquationToolStripMenuItem.Click += new System.EventHandler(this.fromEquationToolStripMenuItem_Click);
            // 
            // calculateToolStripMenuItem
            // 
            this.calculateToolStripMenuItem.Name = "calculateToolStripMenuItem";
            this.calculateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.calculateToolStripMenuItem.Text = "Table of values";
            this.calculateToolStripMenuItem.Click += new System.EventHandler(this.tableOfValuesElementMenuStripItem_Click);
            // 
            // timeDiagramToolStripMenuItem
            // 
            this.timeDiagramToolStripMenuItem.Name = "timeDiagramToolStripMenuItem";
            this.timeDiagramToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.timeDiagramToolStripMenuItem.Text = "Time diagram";
            this.timeDiagramToolStripMenuItem.Click += new System.EventHandler(this.timeDiagramToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Scheme files|*.jsch";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Scheme files|*.jsch";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 173F));
            this.tableLayoutPanel.Controls.Add(this.pictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.listBox, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 53);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.655493F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.34451F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 508F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(784, 508);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.ContextMenuStrip = this.schemeMenuStrip;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(605, 502);
            this.pictureBox.TabIndex = 10;
            this.pictureBox.TabStop = false;
            this.pictureBox.SizeChanged += new System.EventHandler(this.pictureBox_SizeChanged);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            // 
            // schemeMenuStrip
            // 
            this.schemeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearSchemeMenuItem});
            this.schemeMenuStrip.Name = "elementMenuStrip";
            this.schemeMenuStrip.Size = new System.Drawing.Size(117, 26);
            // 
            // clearSchemeMenuItem
            // 
            this.clearSchemeMenuItem.Name = "clearSchemeMenuItem";
            this.clearSchemeMenuItem.Size = new System.Drawing.Size(116, 22);
            this.clearSchemeMenuItem.Text = "Clear all";
            this.clearSchemeMenuItem.Click += new System.EventHandler(this.clearElementMenuStripItem_Click);
            // 
            // listBox
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(614, 3);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(167, 502);
            this.listBox.TabIndex = 11;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.CanOverflow = false;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveButton,
            this.addButton,
            this.connectionButton,
            this.deleteButton});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 26);
            this.toolStrip.TabIndex = 11;
            this.toolStrip.Text = "toolStrip";
            // 
            // moveButton
            // 
            this.moveButton.CheckOnClick = true;
            this.moveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveButton.Image = global::LogicSimulator.WindowsUI.Properties.Resources.cursor;
            this.moveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(23, 23);
            this.moveButton.ToolTipText = "Move elements";
            this.moveButton.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // addButton
            // 
            this.addButton.Checked = true;
            this.addButton.CheckOnClick = true;
            this.addButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addButton.Image = global::LogicSimulator.WindowsUI.Properties.Resources.component;
            this.addButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 23);
            this.addButton.ToolTipText = "Add elements";
            this.addButton.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // connectionButton
            // 
            this.connectionButton.CheckOnClick = true;
            this.connectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.connectionButton.Image = global::LogicSimulator.WindowsUI.Properties.Resources.connect;
            this.connectionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectionButton.Name = "connectionButton";
            this.connectionButton.Size = new System.Drawing.Size(23, 23);
            this.connectionButton.ToolTipText = "Connect elements";
            this.connectionButton.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteButton.Image = global::LogicSimulator.WindowsUI.Properties.Resources.delete;
            this.deleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(23, 23);
            this.deleteButton.ToolTipText = "Delete elements";
            this.deleteButton.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // elementMenuStrip
            // 
            this.elementMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createConnectionToolStripMenuItem,
            this.renameElementToolStripMenuItem,
            this.moveElementToolStripMenuItem,
            this.toolStripMenuItem3,
            this.deleteElementMenuStripItem,
            this.clearElementMenuStripItem});
            this.elementMenuStrip.Name = "elementMenuStrip";
            this.elementMenuStrip.Size = new System.Drawing.Size(120, 120);
            // 
            // createConnectionToolStripMenuItem
            // 
            this.createConnectionToolStripMenuItem.Name = "createConnectionToolStripMenuItem";
            this.createConnectionToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.createConnectionToolStripMenuItem.Text = "Connect";
            this.createConnectionToolStripMenuItem.Click += new System.EventHandler(this.createConnectionToolStripMenuItem_Click);
            // 
            // renameElementToolStripMenuItem
            // 
            this.renameElementToolStripMenuItem.Name = "renameElementToolStripMenuItem";
            this.renameElementToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.renameElementToolStripMenuItem.Text = "Rename";
            this.renameElementToolStripMenuItem.Click += new System.EventHandler(this.renameElementToolStripMenuItem_Click);
            // 
            // moveElementToolStripMenuItem
            // 
            this.moveElementToolStripMenuItem.Name = "moveElementToolStripMenuItem";
            this.moveElementToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.moveElementToolStripMenuItem.Text = "Move";
            this.moveElementToolStripMenuItem.Click += new System.EventHandler(this.moveElementToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(116, 6);
            // 
            // deleteElementMenuStripItem
            // 
            this.deleteElementMenuStripItem.Name = "deleteElementMenuStripItem";
            this.deleteElementMenuStripItem.Size = new System.Drawing.Size(119, 22);
            this.deleteElementMenuStripItem.Text = "Delete";
            this.deleteElementMenuStripItem.Click += new System.EventHandler(this.deleteElementMenuStripItem_Click);
            // 
            // clearElementMenuStripItem
            // 
            this.clearElementMenuStripItem.Name = "clearElementMenuStripItem";
            this.clearElementMenuStripItem.Size = new System.Drawing.Size(119, 22);
            this.clearElementMenuStripItem.Text = "Clear all";
            this.clearElementMenuStripItem.Click += new System.EventHandler(this.clearElementMenuStripItem_Click);
            // 
            // exportFileDialog
            // 
            this.exportFileDialog.Filter = "HTML files|*.html";
            // 
            // printDialog
            // 
            this.printDialog.ShowHelp = true;
            this.printDialog.UseEXDialog = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = global::LogicSimulator.WindowsUI.Properties.Resources.component1;
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logic Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.schemeMenuStrip.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.elementMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton moveButton;
        private System.Windows.Forms.ToolStripButton addButton;
        private System.Windows.Forms.ToolStripButton connectionButton;
        private System.Windows.Forms.ToolStripButton deleteButton;
        private System.Windows.Forms.ContextMenuStrip elementMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteElementMenuStripItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem clearElementMenuStripItem;
        private System.Windows.Forms.ContextMenuStrip schemeMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearSchemeMenuItem;
        private System.Windows.Forms.SaveFileDialog exportFileDialog;
        private System.Windows.Forms.ToolStripMenuItem exportAsHTMLToolStripMenuItem;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.ToolStripMenuItem fromEquationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeDiagramToolStripMenuItem;
    }
}

