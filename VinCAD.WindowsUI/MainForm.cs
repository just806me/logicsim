using VinCAD.Main;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace VinCAD.WindowsUI
{
    public partial class MainForm : Form
    {
        private string filename;

        private Pen mainPen;

        private DrawableScheme scheme;

        private IDrawableElement dragElement;

        private Line connectLine;
        private Direction connectDirection;

        private IDrawableElement menuElement;

        private bool move;
        private Point moveLastLocation;

        private bool modified;

        public MainForm(string path)
        {
            InitializeComponent();

            mainPen = new Pen(Color.Black) { Width = 1 };

            listBox.Items.AddRange(Enum.GetNames(typeof(ComponentType)));
            listBox.Items.Add(nameof(Input));
            listBox.Items.Add(nameof(Output));

            LoadScheme(path);
        }

        private void LoadScheme(string path)
        {
            filename = path;
            modified = false;
            dragElement = null;
            connectLine = null;
            menuElement = null;
            move = false;

            if (string.IsNullOrEmpty(path))
            {
                scheme = new DrawableScheme(pictureBox.Width, pictureBox.Height);
                filename = null;
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
                    ShowError(String.Format(Resource.Localization.Error_PathOpen, path), exception);

                    filename = null;
                    scheme = new DrawableScheme(pictureBox.Width, pictureBox.Height);
                }
            }

            DrawScheme();
        }

        private void LoadScheme(Scheme source)
        {
            filename = null;
            modified = false;
            dragElement = null;
            connectLine = null;
            menuElement = null;
            move = false;

            scheme = DrawableScheme.FromScheme(source, pictureBox.Width, pictureBox.Height);

            DrawScheme();
        }

        private void DrawScheme() => pictureBox.Image = scheme.Draw(mainPen);

        private static void ShowError(string message, Exception exception)
        {
            MessageBox.Show(
                $"{Resource.Localization.Error}: {message}{Environment.NewLine}{(exception == null ? "" : exception.Message)}",
                Resource.Localization.MainForm_Name,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
                switch (MessageBox.Show(
                    Resource.Localization.MainForm_SaveMessage,
                   Resource.Localization.MainForm_Name,
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

            LoadScheme(string.Empty);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
                switch (MessageBox.Show(
                    Resource.Localization.MainForm_SaveMessage,
                    Resource.Localization.MainForm_Name,
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
            if (!string.IsNullOrEmpty(filename))
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
                    Resource.Localization.MainForm_SaveMessage,
                    Resource.Localization.MainForm_Name,
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
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
            {
                move = true;
                moveLastLocation = e.Location;
                pictureBox.Cursor = Cursors.NoMove2D;
                modified = true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (dragElement != null)
                {
                    var overflowElements = scheme.GetElementsAtRectangle(
                        dragElement.X,
                        dragElement.Y,
                        dragElement.Width,
                        dragElement.Height
                    );
                    if (overflowElements.Count() == 0 || (overflowElements.Count() == 1 && overflowElements.Contains(dragElement)))
                        dragElement = null;
                }
                else if (moveButton.Checked)
                {
                    modified = true;
                    dragElement = scheme.GetElementAtLocation(e.Location);
                }

                else if (connectLine != null)
                {
                    var underMouseElement = scheme.GetElementAtLocation(e.Location);
                    if (underMouseElement != null && underMouseElement != connectLine.Start)
                    {
                        connectLine.End = underMouseElement;

                        var add = !scheme.Lines.Contains(connectLine);

                        if (connectLine.End is Component)
                            ((Component)connectLine.End).AddInput(connectLine.StartName);
                        else if (connectLine.End is Output && ((Output)connectLine.End).Input == string.Empty)
                            ((Output)connectLine.End).Input = connectLine.StartName;
                        else
                            add = false;

                        if (add)
                        {
                            scheme.AddLine(connectLine);
                            DrawScheme();
                            connectLine = null;
                        }
                    }
                    else
                    {
                        if (connectDirection == Direction.X)
                        {
                            if (connectLine.Points.Any())
                                connectLine.AddPoint(new Point(e.X, connectLine.Points.Last().Y));
                            else
                                connectLine.AddPoint(new Point(e.X, connectLine.StartPoint.Y));

                            connectDirection = Direction.Y;
                        }
                        else if (connectDirection == Direction.Y)
                        {
                            if (connectLine.Points.Any())
                                connectLine.AddPoint(new Point(connectLine.Points.Last().X, e.Y));
                            else
                                connectLine.AddPoint(new Point(connectLine.StartPoint.X, e.Y));

                            connectDirection = Direction.X;
                        }
                    }
                }
                else if (connectionButton.Checked)
                {
                    var underMouseElement = scheme.GetElementAtLocation(e.Location);
                    if (underMouseElement != null && !(underMouseElement is Output))
                    {
                        modified = true;
                        connectLine = new Line(underMouseElement, null, new Point[0]);
                        connectDirection = Direction.X;
                    }
                }

                else if (deleteButton.Checked)
                {
                    var toDelete = scheme.GetElementAtLocation(e.Location);
                    if (toDelete != null)
                    {
                        modified = true;
                        scheme.RemoveElement(toDelete);
                        DrawScheme();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (connectLine != null)
                    DrawScheme();

                dragElement = null;
                connectLine = null;

                menuElement = scheme.GetElementAtLocation(e.Location);
                pictureBox.ContextMenuStrip = (menuElement == null ? schemeMenuStrip : elementMenuStrip);
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
            pictureBox.Cursor = Cursors.Default;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                scheme.Move(e.X - moveLastLocation.X, e.Y - moveLastLocation.Y);
                DrawScheme();
                moveLastLocation = e.Location;
            }
            else if (dragElement != null)
            {
                if (!scheme.Elements.Contains(dragElement))
                    scheme.AddElement(dragElement);
                else
                {
                    dragElement.X = e.X;
                    dragElement.Y = e.Y;
                }

                DrawScheme();
            }
            else if (connectLine != null)
            {
                var image = scheme.Draw(mainPen);

                using (var graphics = Graphics.FromImage(image))
                {
                    var points = new List<Point>();
                    points.Add(connectLine.StartPoint);

                    if (connectDirection == Direction.X)
                    {
                        if (connectLine.Points.Any())
                        {
                            points.AddRange(connectLine.Points);
                            points.Add(new Point(e.X, connectLine.Points.Last().Y));
                        }
                        else
                            points.Add(new Point(e.X, connectLine.StartPoint.Y));
                    }
                    else if (connectDirection == Direction.Y)
                    {
                        if (connectLine.Points.Any())
                        {
                            points.AddRange(connectLine.Points);
                            points.Add(new Point(connectLine.Points.Last().X, e.Y));
                        }
                        else
                            points.Add(new Point(connectLine.StartPoint.X, e.Y));
                    }

                    graphics.DrawLines(mainPen, points.ToArray());
                }

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
                            .Select(x =>
                            {
                                int number;
                                if (int.TryParse(x.Name.Substring(1), out number))
                                    return number;
                                else
                                    return 0;
                            })
                            .Max() : 0);

                        dragElement = new DrawableInput(
                            $"x{max_input_number + 1}",
                            0, 0,
                            DrawableScheme.ElementWidth,
                            DrawableScheme.ElementHeight
                        );
                    }
                    break;
                case "Output":
                    {
                        var max_output_number = (scheme.Outputs.Count > 0 ? scheme.Outputs
                            .Select(x =>
                            {
                                int number;
                                if (int.TryParse(x.Name.Substring(1), out number))
                                    return number;
                                else
                                    return 0;
                            })
                            .Max() : 0);

                        dragElement = new DrawableOutput(
                            $"y{max_output_number + 1}",
                            string.Empty,
                            0, 0,
                            DrawableScheme.ElementWidth,
                            DrawableScheme.ElementHeight
                        );
                    }
                    break;
                default:
                    {
                        ComponentType componentType;
                        if (Enum.TryParse(item, out componentType))
                        {
                            var max_component_number = (scheme.Components.Count > 0 ? scheme.Components
                                .Select(x =>
                                {
                                    int number;
                                    if (int.TryParse(x.Name.Substring(1), out number))
                                        return number;
                                    else
                                        return 0;
                                })
                                .Max() : 0);

                            dragElement = new DrawableComponent(
                                $"c{max_component_number + 1}",
                                componentType,
                                new string[] { },
                                0, 0,
                                DrawableScheme.ElementWidth,
                                DrawableScheme.ElementHeight
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
            DrawScheme();
            menuElement = null;
        }

        private void clearElementMenuStripItem_Click(object sender, EventArgs e)
        {
            modified = true;

            scheme.Clear();
            DrawScheme();
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
                    using (var form = new TableDialog(table, header, scheme.Inputs.Count))
                        form.ShowDialog();
                else
                    ShowError(Resource.Localization.Error_EmptyScheme, null);
            }
            catch (SchemeException exception)
            {
                ShowError(Resource.Localization.Error_InvalidScheme, exception);
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
                        ShowError(Resource.Localization.Error_HtmlExport, exception);
                    }
                }
        }

        private void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            // mono fix
            if (scheme != null)
            {
                scheme.SetSize(pictureBox.Width, pictureBox.Height);
                DrawScheme();
            }
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

                    DrawScheme();
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
                connectLine = new Line(menuElement, null, null);
            }
            menuElement = null;
        }

        private void timeDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (scheme.Inputs.Any())
                    using (var form = new TimeDiagramDialog(scheme.Scheme))
                        form.ShowDialog();
                else
                    ShowError(Resource.Localization.Error_EmptyScheme, null);
            }
            catch (SchemeException exception)
            {
                ShowError(Resource.Localization.Error_InvalidScheme, exception);
            }
        }

        private void fromEquationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modified)
                switch (MessageBox.Show(
                    Resource.Localization.MainForm_SaveMessage,
                    Resource.Localization.MainForm_Name,
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

            using (var form = new EquationDialog())
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadScheme(Scheme.FromEquation(form.Equation));
                    modified = true;
                }
        }

#if DEBUG
        private void sendLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Logger.Log.Upload());
        }
#endif
    }

    enum Direction
    {
        X,
        Y
    }
}
