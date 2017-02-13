using VinCAD.Main;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Collections.ObjectModel;
using System;

namespace VinCAD.WindowsUI
{
    public interface IDrawable
    {
        void Draw(Graphics graphics, Pen pen);
    }

    public interface IDrawableElement : IDrawable, IElement
    {
        [JsonRequired]
        int X { get; set; }
        [JsonRequired]
        int Y { get; set; }
        [JsonRequired]
        int Width { get; set; }
        [JsonRequired]
        int Height { get; set; }

        bool ContainsLocation(Point p);
        bool ContainsLocation(int x, int y);
        bool IsInRectangle(Rectangle bounds);
        bool IsInRectangle(int x, int y, int width, int height);
    }

    public class DrawableComponent : Component, IDrawableElement
    {
        [JsonRequired]
        public int Height { get; set; }

        [JsonRequired]
        public int Width { get; set; }

        [JsonRequired]
        public int X { get; set; }

        [JsonRequired]
        public int Y { get; set; }

        [JsonConstructor]
        public DrawableComponent(string name, ComponentType type, IEnumerable<string> input) : base(name, type, input) { }

        public DrawableComponent(string name, ComponentType type, IEnumerable<string> input, int x, int y, int width, int height) : base(name, type, input)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            switch (Type)
            {
                case ComponentType.And:
                    {
                        var tmp1 = X + 0.5f * Width;
                        var tmp2 = X + 1.2f * Width;
                        var tmp3 = Y + Height;

                        graphics.DrawLine(pen, X, Y, tmp1, Y);
                        graphics.DrawLine(pen, X, tmp3, tmp1, tmp3);
                        graphics.DrawLine(pen, X, Y, X, tmp3);
                        graphics.DrawBezier(pen, tmp1 -= 1f, Y, tmp2, Y, tmp2, tmp3, tmp1, tmp3);
                    }
                    break;
                case ComponentType.AndNot:
                    {
                        var tmp2 = 0.42f * Height;
                        var tmp3 = (Height * 0.5f - tmp2) * 2f;

                        graphics.DrawEllipse(pen, (Width + X) - 0.9f * tmp3, tmp2 + Y, tmp3, tmp3);

                        var tmp1 = Y + Height;
                        var tmp4 = X + 0.5f * Width - tmp3;
                        var tmp5 = X + 1.2f * Width - tmp3;

                        graphics.DrawLine(pen, X, Y, tmp4, Y);
                        graphics.DrawLine(pen, X, tmp1, tmp4, tmp1);
                        graphics.DrawLine(pen, X, Y, X, tmp1);
                        graphics.DrawBezier(pen, tmp4 -= 1f, Y, tmp5, Y, tmp5, tmp1, tmp4, tmp1);
                    }
                    break;
                case ComponentType.Or:
                    {
                        var tmp1 = X + 0.4f * Width;
                        var tmp2 = Y + Height;
                        var tmp3 = X + 0.3f * Width;
                        var tmp4 = X + 0.85f * Width;
                        var tmp5 = Y + 0.5f * Height;
                        var tmp6 = X + Width;

                        graphics.DrawBezier(pen, X, Y, tmp1, Y, tmp1, tmp2, X, tmp2);
                        graphics.DrawBezier(pen, X, Y, tmp3, Y, tmp4, Y + 0.066f * Height, tmp6, tmp5);
                        graphics.DrawBezier(pen, X, tmp2, tmp3, tmp2, tmp4, Y + 0.933f * Height, tmp6, tmp5);
                    }
                    break;
                case ComponentType.OrNot:
                    {
                        var tmp1 = 0.42f * Height;
                        var tmp2 = (Height * 0.5f - tmp1) * 2f;

                        graphics.DrawEllipse(pen, (Width + X) - 0.9f * tmp2, tmp1 + Y, tmp2, tmp2);

                        tmp1 *= 0.4f;
                        var tmp3 = X + 0.4f * Width;
                        var tmp4 = Y + Height;
                        var tmp6 = X + 0.3f * Width - tmp1;
                        var tmp7 = X + 0.85f * Width - tmp1;
                        var tmp8 = Y + 0.5f * Height;
                        var tmp9 = (X + Width) - tmp1;

                        graphics.DrawBezier(pen, X, Y, tmp3, Y, tmp3, tmp4, X, tmp4);
                        graphics.DrawBezier(pen, X, Y, tmp6, Y, tmp7, Y + 0.066f * Height, tmp9, tmp8);
                        graphics.DrawBezier(pen, X, tmp4, tmp6, tmp4, tmp7, Y + 0.933f * Height, tmp9, tmp8);
                    }
                    break;
                case ComponentType.Xor:
                    {
                        var tmp1 = X + 0.4f * Width;
                        var tmp2 = Y + Height;
                        var tmp3 = X + 0.3f * Width;
                        var tmp4 = X + 0.85f * Width;
                        var tmp5 = Y + 0.5f * Height;
                        var tmp6 = X + Width;
                        var tmp7 = X + 0.125f * Width;
                        var tmp8 = tmp1 - 0.125f * Width;

                        graphics.DrawBezier(pen, tmp7, Y, tmp1, Y, tmp1, tmp2, tmp7, tmp2);
                        graphics.DrawBezier(pen, X, Y, tmp8, Y, tmp8, tmp2, X, tmp2);
                        graphics.DrawBezier(pen, tmp7, Y, tmp3, Y, tmp4, Y + 0.066f * Height, tmp6, tmp5);
                        graphics.DrawBezier(pen, tmp7, tmp2, tmp3, tmp2, tmp4, Y + 0.933f * Height, tmp6, tmp5);
                    }
                    break;
                case ComponentType.XorNot:
                    {
                        var tmp7 = 0.42f * Height;
                        var tmp8 = (Height * 0.5f - tmp7) * 2;

                        graphics.DrawEllipse(pen, Width + X - 0.9f * tmp8, tmp7 + Y, tmp8, tmp8);

                        tmp7 *= 0.4f;
                        var tmp1 = X + 0.4f * Width;
                        var tmp2 = Y + Height;
                        var tmp3 = X + 0.3f * Width - tmp7;
                        var tmp4 = X + 0.85f * Width - tmp7;
                        var tmp5 = Y + 0.5f * Height;
                        var tmp6 = X + Width - tmp7;
                        var tmp9 = X + 0.125f * Width;
                        var tmp10 = tmp1 - 0.125f * Width;

                        graphics.DrawBezier(pen, tmp9, Y, tmp1, Y, tmp1, tmp2, tmp9, tmp2);
                        graphics.DrawBezier(pen, X, Y, tmp10, Y, tmp10, tmp2, X, tmp2);
                        graphics.DrawBezier(pen, tmp9, Y, tmp3, Y, tmp4, Y + 0.066f * Height, tmp6, tmp5);
                        graphics.DrawBezier(pen, tmp9, tmp2, tmp3, tmp2, tmp4, Y + 0.933f * Height, tmp6, tmp5);
                    }
                    break;
                case ComponentType.Not:
                    {
                        var tmp1 = 0.42f * Height;
                        var tmp2 = (Height * 0.5f - tmp1) * 2;

                        graphics.DrawEllipse(pen, Width + X - 0.9f * tmp2, tmp1 + Y, tmp2, tmp2);

                        var tmp3 = X + Width - tmp1 * 0.4f;
                        var tmp4 = Y + 0.5f * Height;
                        var tmp5 = Y + Height;

                        graphics.DrawLine(pen, X, Y, X, tmp5);
                        graphics.DrawLine(pen, X, Y, tmp3, tmp4);
                        graphics.DrawLine(pen, X, tmp5, tmp3, tmp4);
                    }
                    break;
                default:
                    break;
            }
        }

        public bool ContainsLocation(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool ContainsLocation(int x, int y) => new Rectangle(X, Y, Width, Height).Contains(x, y);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height)
            || rectangle.Contains(new Rectangle(X, Y, Width, Height));

        public bool IsInRectangle(int x, int y, int width, int height) => IsInRectangle(new Rectangle(x, y, width, height));
    }

    public class DrawableInput : Input, IDrawableElement
    {
        [JsonRequired]
        public int Height { get; set; }
        [JsonRequired]
        public int Width { get; set; }
        [JsonRequired]
        public int X { get; set; }
        [JsonRequired]
        public int Y { get; set; }

        [JsonConstructor]
        public DrawableInput(string name) : base(name) { }

        public DrawableInput(string name, int x, int y, int width, int height) : base(name)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.DrawRectangle(pen, X, Y, Width, Height);
            graphics.DrawString(Name, new Font("Arial", 8), new SolidBrush(pen.Color), X + Width * 0.3f, Y + Height * 0.3f);
        }

        public bool ContainsLocation(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool ContainsLocation(int x, int y) => new Rectangle(X, Y, Width, Height).Contains(x, y);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height)
            || rectangle.Contains(new Rectangle(X, Y, Width, Height));

        public bool IsInRectangle(int x, int y, int width, int height) => IsInRectangle(new Rectangle(x, y, width, height));
    }

    public class DrawableOutput : Output, IDrawableElement
    {
        [JsonRequired]
        public int Height { get; set; }
        [JsonRequired]
        public int Width { get; set; }
        [JsonRequired]
        public int X { get; set; }
        [JsonRequired]
        public int Y { get; set; }

        [JsonConstructor]
        public DrawableOutput(string name, string input) : base(name, input) { }

        public DrawableOutput(string name, string input, int x, int y, int width, int height) : base(name, input)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.DrawRectangle(pen, X, Y, Width, Height);
            graphics.DrawString(Name, new Font("Arial", 8), new SolidBrush(pen.Color), X + Width * 0.3f, Y + Height * 0.3f);
        }

        public bool ContainsLocation(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool ContainsLocation(int x, int y) => new Rectangle(X, Y, Width, Height).Contains(x, y);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height)
            || rectangle.Contains(new Rectangle(X, Y, Width, Height));

        public bool IsInRectangle(int x, int y, int width, int height) => IsInRectangle(new Rectangle(x, y, width, height));
    }

    public class Line : IDrawable
    {
        [JsonIgnore]
        public IDrawableElement Start { get; set; }
        [JsonRequired]
        public string StartName => Start.Name;

        [JsonIgnore]
        public IDrawableElement End { get; set; }
        [JsonRequired]
        public string EndName => End.Name;

        [JsonIgnore]
        public Point StartPoint => Start != null ? new Point(Start.X + Start.Width, Start.Y + Start.Height / 2) : new Point();
        [JsonIgnore]
        public Point EndPoint
        {
            get
            {
                if (End is Output)
                    return new Point(End.X, End.Y + End.Height / 2);
                else if (End is Component)
                {
                    var inputs_count = ((Component)End).Input.Count + 1f;
                    var index = ((Component)End).Input.IndexOf(Start.Name) + 1f;

                    return new Point(End.X, (int)Math.Round(End.Y + End.Height * index / inputs_count));
                }
                else return new Point();
            }
        }
        [JsonIgnore]
        public ReadOnlyCollection<Point> AllPoints
        {
            get
            {
                var result = new List<Point>(_points.Count + 2);

                result.Add(StartPoint);
                result.AddRange(_points);
                result.Add(EndPoint);

                return result.AsReadOnly();
            }
        }

        [JsonIgnore]
        private List<Point> _points;
        [JsonRequired]
        public ReadOnlyCollection<Point> Points => _points.AsReadOnly();

        [JsonConstructor]
        public Line(string startName, string endName, IEnumerable<Point> points)
            : this(new DrawableInput(startName), new DrawableInput(endName), points) { }

        public Line(IDrawableElement start, IDrawableElement end, IEnumerable<Point> points)
        {
            Start = start;
            End = end;
            _points = points.ToList();
        }

        public void Draw(Graphics graphics, Pen pen) => graphics.DrawLines(pen, AllPoints.ToArray());

        public bool IsConnectedTo(IDrawableElement element) => Start == element || End == element;

        public void AddPoint(Point point) => _points.Add(point);

        public void AddPoint(int x, int y) => _points.Add(new Point(x, y));

        public void Move(int dx, int dy)
        {
            for (int i = 0; i < _points.Count; i++)
                _points[i] = new Point(_points[i].X + dx, _points[i].Y + dy);
        }
    }
}
