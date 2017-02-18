using VinCAD.Main;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VinCAD.WindowsUI
{
    public partial class MainForm : Form
    {
        private string filename;

        private Pen mainPen;

        private DrawableScheme scheme;

        private IMoveable dragElement;
        private Point dragPrevLocation;

        private Line connectLine;
        private IDrawableElement connectStartElement;

        private ISelectable menuElement;

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
                    var overflowElements = scheme.Elements.Where(x => x.IsInRectangle(new Rectangle(
                        dragElement.X,
                        dragElement.Y,
                        dragElement.Width,
                        dragElement.Height
                    )));
                    if (overflowElements.Count() == 0 || (overflowElements.Count() == 1 && overflowElements.Contains(dragElement)))
                        dragElement = null;
                }
                else if (moveButton.Checked)
                {
                    modified = true;
                    dragElement = scheme.Moveable.FirstOrDefault(x => x is ISelectable && ((ISelectable)x).ContainsPoint(e.Location));
                    dragPrevLocation = e.Location;
                }

                else if (connectLine != null)
                {
                    var underMouseElement = scheme.Elements.FirstOrDefault(
                        x => x.ContainsPoint(new Point(connectLine.X + connectLine.Width, connectLine.Y + connectLine.Height))
                    );
                    if (underMouseElement != null && underMouseElement != connectLine.Start)
                    {
                        var add = !scheme.Lines.Contains(connectLine);

                        if (underMouseElement is Component)
                            ((Component)underMouseElement).AddInput(connectStartElement.Name);
                        else if (underMouseElement is Output && ((Output)underMouseElement).Input == string.Empty)
                            ((Output)underMouseElement).Input = connectStartElement.Name;
                        else
                            add = false;

                        if (add)
                        {
                            connectLine.End = underMouseElement;

                            scheme.AddLine(connectLine);
                            DrawScheme();
                            connectLine = null;
                        }
                    }
                    else
                    {
                        if (connectLine.Start is Line && connectLine.Direction == ((Line)connectLine.Start).Direction)
                        {
                            ((Line)connectLine.Start).Length += connectLine.Length;
                            connectLine = ((Line)connectLine.Start);
                            connectLine.End = null;
                        }

                        Line line;

                        switch (connectLine.Direction)
                        {
                            case Direction.X:
                                line = new Line(connectLine, null, Direction.Y, 0);
                                break;
                            case Direction.Y:
                                line = new Line(connectLine, null, Direction.X, 0);
                                break;
                            default:
                                line = null;
                                break;
                        }

                        connectLine.End = line;
                        scheme.AddLine(connectLine);
                        connectLine = line;

                        DrawScheme();
                    }
                }
                else if (connectionButton.Checked)
                {
                    var underMouseElement = scheme.Elements.FirstOrDefault(x => x.ContainsPoint(e.Location));
                    if (underMouseElement != null && !(underMouseElement is Output))
                    {
                        modified = true;

                        connectStartElement = underMouseElement;
                        connectLine = new Line(underMouseElement, null, Direction.X, 0);
                    }
                }

                else if (deleteButton.Checked)
                {
                    modified = true;

                    var toDelete = scheme.Selectable.Where(x => x.ContainsPoint(e.Location)).ToArray();
                    foreach (var item in toDelete)
                        if (item is IDrawableElement)
                            scheme.RemoveElement((IDrawableElement)item);
                        else if (item is Line)
                            scheme.RemoveLine((Line)item);

                    DrawScheme();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (connectLine != null)
                    DrawScheme();

                dragElement = null;
                connectLine = null;

                menuElement = scheme.Selectable.FirstOrDefault(x => x.ContainsPoint(e.Location));
                pictureBox.ContextMenuStrip = (menuElement == null ? schemeMenuStrip : elementMenuStrip);
                dragPrevLocation = e.Location;
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
            pictureBox.Cursor = Cursors.Default;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
#if DEBUG
            Text = e.Location.ToString();
#endif

            if (move)
            {
                scheme.MoveElements(e.X - moveLastLocation.X, e.Y - moveLastLocation.Y);
                DrawScheme();
                moveLastLocation = e.Location;
            }
            else if (dragElement != null)
            {
                if (!scheme.Elements.Contains(dragElement) && dragElement is IDrawableElement)
                {
                    ((IDrawableElement)dragElement).X = e.X;
                    ((IDrawableElement)dragElement).Y = e.Y;
                    scheme.AddElement((IDrawableElement)dragElement);
                }
                else
                    dragElement.Move(e.X - dragPrevLocation.X, e.Y - dragPrevLocation.Y);

                dragPrevLocation = e.Location;

                DrawScheme();
            }
            else if (connectLine != null)
            {
                var dx = e.X - connectLine.X;
                var dy = e.Y - connectLine.Y;

                if (Math.Abs(dy) > Math.Abs(dx))
                {
                    connectLine.Direction = Direction.Y;
                    connectLine.Length = dy;
                }
                else
                {
                    connectLine.Direction = Direction.X;
                    connectLine.Length = dx;
                }

                using (var image = new Bitmap(pictureBox.Width, pictureBox.Height))
                using (var graphics = Graphics.FromImage(image))
                {
                    connectLine.Draw(graphics, mainPen);
                    scheme.Draw(graphics, mainPen);

                    pictureBox.Image = (Image)image.Clone();
                }
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

            if (menuElement is IDrawableElement)
                scheme.RemoveElement((IDrawableElement)menuElement);
            else if (menuElement is Line)
                scheme.RemoveLine((Line)menuElement);

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
            if (menuElement is IDrawableElement)
                using (var dialog = new NameDialog())
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        modified = true;

                        foreach (Output output in scheme.Elements.Where(x => x is Output && (x as Output).Input == ((IDrawableElement)menuElement).Name))
                            output.Input = dialog.NewName;
                        foreach (Component component in scheme.Elements.Where(x => x is Component && (x as Component).Input.Contains(((IDrawableElement)menuElement).Name)))
                        {
                            component.RemoveInput(((IDrawableElement)menuElement).Name);
                            component.AddInput(dialog.NewName);
                        }
                        ((IDrawableElement)menuElement).Name = dialog.NewName;

                        DrawScheme();
                    }

            menuElement = null;
        }

        private void moveElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modified = true;

            if (menuElement is IMoveable)
            {
                toolStripButton_Click(moveButton, null);
                dragElement = (IMoveable)menuElement;
            }

            menuElement = null;
        }

        private void createConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Click(connectionButton, null);
            if (menuElement is IDrawableElement && !(menuElement is Output))
            {
                modified = true;

                connectStartElement = (IDrawableElement)menuElement;
                connectLine = new Line((IDrawableElement)menuElement, null, Direction.X, 0);
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
}
