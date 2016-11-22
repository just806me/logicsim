﻿using LogicSimulator.Main;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LogicSimulator.WindowsUI
{
    public partial class MainForm : Form
    {
        private string filename;

        private Pen mainPen;
        private DrawableScheme scheme;

        private IMoveableElement dragElement;
        private Line connectLine;
        private IMoveableElement menuElement;

        private bool modified;

        private const int ElementWidth = 40;
        private const int ElementHeight = 30;

        public MainForm(string path)
        {
            InitializeComponent();

            mainPen = new Pen(Color.Black) { Width = 1 };

            listBox.Items.AddRange(Enum.GetNames(typeof(ComponentType)));
            listBox.Items.Add(nameof(Input));
            listBox.Items.Add(nameof(Output));

            LoadScheme(path);
        }

        public void LoadScheme(string path)
        {
            filename = path;
            modified = false;
            dragElement = null;
            connectLine = null;
            menuElement = null;

            if (string.IsNullOrEmpty(path))
            {
                scheme = new DrawableScheme(pictureBox.Width, pictureBox.Height);
            }
            else
            {
                try
                {
                    using (var file = File.Open(path, FileMode.Open))
                        scheme = DrawableSchemeStorage.Load(file);
                }
                catch (Exception exception)
                {
                    ShowError($"Cannot open file {path}!", exception);

                    filename = null;
                    scheme = new DrawableScheme(pictureBox.Width, pictureBox.Height);
                }
            }

            pictureBox.Image = scheme.Draw(mainPen);
        }

        private void ShowError(string message, Exception exception)
        {
            MessageBox.Show(
                $"Error: {message}{Environment.NewLine}{(exception == null ? "" : exception.Message)}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
                switch (MessageBox.Show(
                    "Do you want to save the current project?",
                    "Close Project",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                )
                {
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(sender, e);
                        break;
                    default:
                        break;
                }

            LoadScheme(null);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
                switch (MessageBox.Show(
                    "Do you want to save the current project?",
                    "Close Project",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                )
                {
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(sender, e);
                        break;
                    default:
                        break;
                }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                LoadScheme(openFileDialog.FileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename != null)
            {
                using (var file = File.Open(filename, FileMode.Create))
                    DrawableSchemeStorage.Save(scheme, file);

                modified = false;
            }
            else
                saveAsToolStripMenuItem_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var file = saveFileDialog.OpenFile())
                    DrawableSchemeStorage.Save(scheme, file);

                filename = saveFileDialog.FileName;

                modified = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
                switch (MessageBox.Show(
                    "Do you want to save the current project?",
                    "Close Project",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question)
                )
                {
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(sender, e);
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
        }

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton item in ((ToolStripButton)sender).GetCurrentParent().Items)
                if (item != null)
                    item.Checked = item == sender;

            listBox.Enabled = sender == addButton;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (dragElement != null)
                    dragElement = null;
                else if (moveButton.Checked)
                {
                    modified = true;
                    dragElement = scheme.GetElementAtLocation(e.Location) as IMoveableElement;
                }

                if (connectLine != null)
                {
                    var underMouseElement = scheme.GetElementAtLocation(e.Location) as IMoveableElement;
                    if (underMouseElement != null && underMouseElement != connectLine.connection.Item1)
                    {
                        connectLine.connection = new Tuple<IMoveableElement, IMoveableElement>(
                            connectLine.connection.Item1,
                            underMouseElement
                        );

                        var add = !scheme.Elements.Contains(connectLine);

                        if (connectLine.connection.Item2 is Component)
                            (connectLine.connection.Item2 as Component).AddInput(connectLine.connection.Item1.Name);
                        else if (connectLine.connection.Item2 is Output && (connectLine.connection.Item2 as Output).Input == null)
                            (connectLine.connection.Item2 as Output).Input = connectLine.connection.Item1.Name;
                        else
                            add = false;

                        if (add)
                        {
                            scheme.AddElement(connectLine);
                            pictureBox.Image = scheme.Draw(mainPen);
                            connectLine = null;
                        }
                    }

                }
                else if (connectionButton.Checked)
                {
                    var underMouseElement = scheme.GetElementAtLocation(e.Location) as IMoveableElement;
                    if (underMouseElement != null && !(underMouseElement is Output))
                    {
                        modified = true;
                        connectLine = new Line(underMouseElement, null);
                    }
                }

                if (deleteButton.Checked)
                {
                    var toDelete = scheme.GetElementAtLocation(e.Location);
                    if (toDelete != null)
                    {
                        modified = true;
                        scheme.RemoveElement(toDelete);
                        pictureBox.Image = scheme.Draw(mainPen);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (connectLine != null)
                    pictureBox.Image = scheme.Draw(mainPen);

                dragElement = null;
                connectLine = null;

                menuElement = scheme.GetElementAtLocation(e.Location) as IMoveableElement;
                pictureBox.ContextMenuStrip = (menuElement == null ? schemeMenuStrip : elementMenuStrip);
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragElement != null)
            {
                if (!scheme.Elements.Contains(dragElement))
                    scheme.AddElement(dragElement);
                else
                {
                    dragElement.X = e.X;
                    dragElement.Y = e.Y;
                }

                pictureBox.Image = scheme.Draw(mainPen);
            }
            else if (connectLine != null)
            {
                var image = scheme.Draw(mainPen);
                using (var graphics = Graphics.FromImage(image))
                    graphics.DrawLine(mainPen, connectLine.X, connectLine.Y, e.X, e.Y);
                pictureBox.Image = image;
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            modified = true;

            string item = listBox.SelectedItem as string;
            switch (item)
            {
                case "Input":
                    {
                        var max_input_number = (scheme.Inputs.Count > 0 ? scheme.Inputs
                            .Select(x => x.Name.Substring(1))
                            .Where(x => { int number; return int.TryParse(x, out number); })
                            .Select(x => int.Parse(x))
                            .Max() : 0);

                        dragElement = new DrawableInput(
                            $"x{max_input_number + 1}",
                            ElementWidth,
                            ElementHeight
                        );
                    }
                    break;
                case "Output":
                    {
                        var max_output_number = (scheme.Outputs.Count > 0 ? scheme.Outputs
                            .Select(x => x.Name.Substring(1))
                            .Where(x => { int number; return int.TryParse(x, out number); })
                            .Select(x => int.Parse(x))
                            .Max() : 0);

                        dragElement = new DrawableOutput(
                            $"y{max_output_number + 1}",
                            null,
                            ElementWidth,
                            ElementHeight
                        );
                    }
                    break;
                default:
                    {
                        ComponentType componentType;
                        if (Enum.TryParse(item, out componentType))
                        {
                            var max_component_number = (scheme.Components.Count > 0 ? scheme.Components
                                .Select(x => x.Name.Substring(1))
                                .Where(x => { int number; return int.TryParse(x, out number); })
                                .Select(x => int.Parse(x))
                                .Max() : 0);

                            dragElement = new DrawableComponent(
                                $"c{max_component_number + 1}",
                                componentType,
                                new string[] { },
                                ElementWidth,
                                ElementHeight
                            );
                        }
                    }
                    break;
            }
        }

        private void deleteElementMenuStripItem_Click(object sender, EventArgs e)
        {
            modified = true;

            scheme.RemoveElement(menuElement);
            pictureBox.Image = scheme.Draw(mainPen);
            menuElement = null;
        }

        private void clearElementMenuStripItem_Click(object sender, EventArgs e)
        {
            modified = true;

            scheme.Clear();
            pictureBox.Image = scheme.Draw(mainPen);
        }

        private void tableOfValuesElementMenuStripItem_Click(object sender, EventArgs e)
        {
            try
            {
                var table = scheme.Scheme.CalculateTable();

                var header = scheme.Inputs.Select(x => x.Name).Concat(
                    scheme.Outputs.Select(x => x.Name)
                ).ToArray();

                if (table.Length > 0 && table.GetLength(1) == header.Length)
                    using (var form = new TableDialog(table, header))
                        form.ShowDialog();
                else
                    ShowError("Nothing to show", null);
            }
            catch (SchemeException exception)
            {
                ShowError($"Scheme is invalid!", exception);
            }
        }

        private void exportAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (exportFileDialog.ShowDialog() == DialogResult.OK)
                using (var file = exportFileDialog.OpenFile())
                {
                    try
                    {
                        scheme.Scheme.CalculateAndWriteTable(file);
                    }
                    catch (SchemeException exception)
                    {
                        MessageBox.Show($"Error: {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
        }

        private void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            scheme.SetSize(pictureBox.Width, pictureBox.Height);
            pictureBox.Image = scheme.Draw(mainPen);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = new PrintDocument();
            doc.PrintPage += (s, args) => scheme.Draw(args.Graphics, mainPen);
            printDialog.Document = doc;
            if (printDialog.ShowDialog() == DialogResult.OK)
                doc.Print();
        }

        private void renameElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new NameDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    modified = true;

                    foreach (Output output in scheme.Elements.Where(x => x is Output && (x as Output).Input == menuElement.Name))
                        output.Input = dialog.NewName;
                    foreach (Component component in scheme.Elements.Where(x => x is Component && (x as Component).Input.Contains(menuElement.Name)))
                    {
                        component.RemoveInput(menuElement.Name);
                        component.AddInput(dialog.NewName);
                    }
                    menuElement.Name = dialog.NewName;

                    pictureBox.Image = scheme.Draw(mainPen);
                    menuElement = null;
                }
        }

        private void moveElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modified = true;

            toolStripButton_Click(moveButton, null);
            dragElement = menuElement;
            menuElement = null;
        }

        private void createConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Click(connectionButton, null);
            if (!(menuElement is Output))
            {
                modified = true;
                connectLine = new Line(menuElement, null);
            }
            menuElement = null;
        }

        private void fromEquationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
        }
    }
}
