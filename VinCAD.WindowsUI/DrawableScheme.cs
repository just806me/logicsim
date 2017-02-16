using VinCAD.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace VinCAD.WindowsUI
{
    public class DrawableScheme : IDisposable
    {
        [JsonIgnore]
        public Scheme Scheme => new Scheme(Inputs, Outputs, Components);

        [JsonIgnore]
        public ReadOnlyCollection<IDrawableElement> Elements => _elements.AsReadOnly();
        private List<IDrawableElement> _elements;

        [JsonRequired]
        public ReadOnlyCollection<DrawableInput> Inputs =>
            _elements.Where(x => x is DrawableInput).Cast<DrawableInput>().ToList().AsReadOnly();
        [JsonRequired]
        public ReadOnlyCollection<DrawableOutput> Outputs =>
            _elements.Where(x => x is DrawableOutput).Cast<DrawableOutput>().ToList().AsReadOnly();
        [JsonRequired]
        public ReadOnlyCollection<DrawableComponent> Components =>
            _elements.Where(x => x is DrawableComponent).Cast<DrawableComponent>().ToList().AsReadOnly();

        [JsonRequired]
        public ReadOnlyCollection<Line> Lines => _lines.AsReadOnly();
        [JsonIgnore]
        private List<Line> _lines;

        [JsonIgnore]
        public IEnumerable<ISelectable> Selectable => _elements.Cast<ISelectable>().Union(_lines);
        [JsonIgnore]
        public IEnumerable<IMoveable> Moveable => _elements.Cast<IMoveable>().Union(_lines);
        [JsonIgnore]
        public IEnumerable<IDrawable> Drawable => _elements.Cast<IDrawable>().Union(_lines);

        [JsonRequired]
        public int Width { get; private set; }
        [JsonRequired]
        public int Height { get; private set; }
        [JsonIgnore]
        private Bitmap _bitmap;
        [JsonIgnore]
        private Graphics _graphics;

        [JsonIgnore]
        public const int ElementWidth = 40;
        [JsonIgnore]
        public const int ElementHeight = 30;
        [JsonIgnore]
        public const int GapWidth = 30;
        [JsonIgnore]
        public const int GapHeight = 25;

        public DrawableScheme(int width, int height)
        {
            _elements = new List<IDrawableElement>();
            _lines = new List<Line>();

            SetSize(width, height);
        }

        [JsonConstructor]
        public DrawableScheme(IEnumerable<DrawableInput> inputs, IEnumerable<DrawableOutput> outputs, IEnumerable<DrawableComponent> components, IEnumerable<Line> lines, int width, int height)
        {
            _elements = new List<IDrawableElement>();

            if (inputs != null)
                _elements.AddRange(inputs);
            if (components != null)
                _elements.AddRange(components);
            if (outputs != null)
                _elements.AddRange(outputs);

            if (lines != null)
                _lines = lines.ToList();
            else
                _lines = new List<Line>();

            SetSize(width, height);
        }

        public static DrawableScheme FromScheme(Scheme scheme, int width, int height)
        {
            var elementX = 0;
            var elementY = 0;

            var drawableInputs = scheme.Inputs.Select(i =>
            {
                var input = new DrawableInput(i.Name, elementX, elementY, ElementWidth, ElementHeight);
                elementY += GapHeight + ElementHeight;
                return input;
            }).ToArray();

            elementX += GapWidth + ElementWidth;
            elementY = 0;


            var drawableComponents = new List<DrawableComponent>();
            foreach (var layer in scheme.ComponentLayers)
            {
                drawableComponents.AddRange(layer.Select(c =>
                {
                    var component = new DrawableComponent(c.Name, c.Type, c.Input, elementX, elementY, ElementWidth, ElementHeight);
                    elementY += GapHeight + ElementHeight;
                    return component;
                }).ToArray());

                elementX += GapWidth + ElementWidth;
                elementY = 0;
            }

            var drawableOutputs = scheme.Outputs.Select(o =>
            {
                var output = new DrawableOutput(o.Name, o.Input, elementX, elementY, ElementWidth, ElementHeight);
                elementY += GapHeight + ElementHeight;
                return output;
            }).ToArray();

            var drawableScheme = new DrawableScheme(drawableInputs, drawableOutputs, drawableComponents, null, width, height);

            drawableScheme.RestoreLines();

            return drawableScheme;
        }

        public void RestoreLines()
        {
            throw new NotImplementedException();
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;

            if (_graphics != null)
                _graphics.Dispose();
            if (_bitmap != null)
                _bitmap.Dispose();

            _bitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        public void MoveElements(int dx, int dy)
        {
            foreach (var item in _elements)
                item.Move(new Point(item.X + dx, item.Y + dy));
        }

        public void Draw(Graphics graphics, Pen pen, bool clear = false, Color? fillColor = null)
        {
            if (clear)
                graphics.Clear(fillColor ?? Color.White);

            foreach (var item in Drawable)
                item.Draw(graphics, pen);
        }

        public Image Draw(Pen pen)
        {
            Draw(_graphics, pen, true);
            return _bitmap; // TODO : (Image)_bitmap.Clone()
        }

        public void AddElement(IDrawableElement element) => _elements.Add(element);

        public void AddLine(Line line) => _lines.Add(line);

        public void RemoveElement(IDrawableElement element)
        {
            _elements.Remove(element);

            foreach (var item in _elements)
            {
                if (item is Output)
                {
                    var output = (Output)item;
                    if (output.Input == element.Name)
                        output.Input = null;

                }
                else if (item is Component)
                {
                    var component = (Component)item;
                    if (component.Input.Any(c => c == element.Name))
                        component.RemoveInput(element.Name);
                }
            }
        }

        public void RemoveLine(Line line) => _lines.Remove(line);

        public void Clear()
        {
            _elements.Clear();
            _lines.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_graphics != null)
                    _graphics.Dispose();
                if (_bitmap != null)
                    _bitmap?.Dispose();
            }
        }
    }
}
