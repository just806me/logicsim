using LogicSimulator.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace LogicSimulator.WindowsUI
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

        public DrawableScheme(int width, int height)
        {
            _elements = new List<IDrawableElement>();
            SetSize(width, height);
        }

        [JsonConstructor]
        public DrawableScheme(IEnumerable<DrawableInput> inputs, IEnumerable<DrawableOutput> outputs, IEnumerable<DrawableComponent> components, IEnumerable<Line> lines, int width, int height)
        {
            _elements = inputs.Cast<IDrawableElement>().Concat(components).Concat(outputs).Concat(lines).ToList();
            SetSize(width, height);
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
                    var output = item as Output;
                    if (output.Input == element.Name)
                        output.Input = null;

                }
                else if (item is Component)
                {
                    var component = item as Component;
                    if (component.Input.Any(c => c == element.Name))
                        component.RemoveInput(element.Name);
                }
            }
        }

        public void Clear() => _elements.Clear();

        public void Dispose()
        {
            _graphics.Dispose();
            _bitmap.Dispose();
        }
    }
}
