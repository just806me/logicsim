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
        [JsonIgnore]
        public ReadOnlyCollection<Line> Lines =>
            _elements.Where(x => x is Line).Cast<Line>().ToList().AsReadOnly();

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
                _elements.AddRange(lines);

            SetSize(width, height);
        }

        public static DrawableScheme FromScheme(Scheme scheme, int width, int height)
        {
            var elementX = 0;
            var elementY = 0;

            var drawableInputs = scheme.Inputs.Select(i => {
                var input = new DrawableInput(i.Name, elementX, elementY, ElementWidth, ElementHeight);
                elementY += GapHeight + ElementHeight;
                return input;
            }).ToArray();

            elementX += GapWidth + ElementWidth;
            elementY = 0;


            var drawableComponents = new List<DrawableComponent>();
            foreach (var layer in scheme.ComponentLayers)
            {
                drawableComponents.AddRange(layer.Select(c => {
                    var component = new DrawableComponent(c.Name, c.Type, c.Input, elementX, elementY, ElementWidth, ElementHeight);
                    elementY += GapHeight + ElementHeight;
                    return component;
                }).ToArray());

                elementX += GapWidth + ElementWidth;
                elementY = 0;
            }

            var drawableOutputs = scheme.Outputs.Select(o => {
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
            _elements.RemoveAll(x => Lines.Contains(x));

            foreach (var component in Components)
                foreach (var input in component.Input)
                    _elements.Add(new Line(
                        _elements.FirstOrDefault(x => x is IMoveableElement && x.Name == input) as IMoveableElement,
                        component
                    ));

            foreach (var output in Outputs)
            {
                _elements.Add(new Line(
                    _elements.FirstOrDefault(x => x is IMoveableElement && x.Name == output.Input) as IMoveableElement,
                    output
                ));
            }

            foreach (var line in Lines)
                line.connection = new Tuple<IMoveableElement, IMoveableElement>(
                    _elements.FirstOrDefault(x => x is IMoveableElement && x.Name == line.connectionName.Item1) as IMoveableElement,
                    _elements.FirstOrDefault(x => x is IMoveableElement && x.Name == line.connectionName.Item2) as IMoveableElement
                );
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

        public void Move(int dx, int dy)
        {
            foreach (var item in _elements)
                if (item is IMoveableElement)
                {
                    ((IMoveableElement)item).X += dx;
                    ((IMoveableElement)item).Y += dy;
                }
        }

        public void Draw(Graphics graphics, Pen pen, bool clear = false, Color? fillColor = null)
        {
            if (clear)
                graphics.Clear(fillColor ?? Color.White);

            foreach (var element in _elements)
                element.Draw(graphics, pen);
        }

        public Image Draw(Pen pen)
        {
            Draw(_graphics, pen, true);
            return _bitmap;
        }

        public IEnumerable<IDrawableElement> GetElementsAtRectangle(Rectangle bounds) 
            => _elements.FindAll(e => e.IsInRectangle(bounds));

        public IEnumerable<IDrawableElement> GetElementsAtRectangle(int x, int y, int width, int height)
            => _elements.FindAll(e => e.IsInRectangle(x, y, width, height));

        public IDrawableElement GetElementAtLocation(Point p) => _elements.Find(e => e.ContainsLocation(p));

        public IDrawableElement GetElementAtLocation(int x, int y) => _elements.Find(e => e.ContainsLocation(x, y));

        public void AddElement(IDrawableElement element) => _elements.Add(element);

        public void RemoveElement(IDrawableElement element)
        {
            _elements.Remove(element);
            _elements.RemoveAll(x => x is Line && (x as Line).ConnectedTo(element));

            foreach (var item in _elements)
            {
                if (item is Output)
                {
                    if (((Output)item).Input == element.Name)
                        ((Output)item).Input = null;

                }
                else if (item is Component)
                {
                    if (((Component)item).Input.Any(c => c == element.Name))
                        ((Component)item).RemoveInput(element.Name);
                }
            }
        }

        public void Clear() => _elements.Clear();

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
