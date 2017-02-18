using VinCAD.Main;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Drawing.Drawing2D;
using System.Linq;

namespace VinCAD.WindowsUI
{
    public class OnMoveEventArgs : EventArgs
    {
        public int Dx;
        public int Dy;
        public object Ignore;

        public OnMoveEventArgs(int dx, int dy, object ignore)
        {
            Dx = dx;
            Dy = dy;
        }
    }

    public class DrawableComponent : Component, IDrawableElement
    {
        [JsonIgnore]
        public int Height => DrawableScheme.ElementHeight;
        [JsonIgnore]
        public int Width => DrawableScheme.ElementWidth;
        [JsonRequired]
        public int X { get; set; }
        [JsonRequired]
        public int Y { get; set; }

        [JsonConstructor]
        public DrawableComponent(string name, ComponentType type, IEnumerable<string> input) : base(name, type, input) { }

        public DrawableComponent(string name, ComponentType type, IEnumerable<string> input, int x, int y) : this(name, type, input)
        {
            X = x;
            Y = y;
        }

        public event EventHandler<OnMoveEventArgs> OnMove;

        public void Draw(Graphics graphics, Pen pen)
        {
            using (var path = new GraphicsPath())
            {
                switch (Type)
                {
                    case ComponentType.And:
                        {
                            var tmp1 = X + 0.5f * Width;
                            var tmp2 = X + 1.2f * Width;
                            var tmp3 = Y + Height;

                            path.AddLine(X, tmp3, tmp1, tmp3);
                            path.AddLine(X, tmp3, X, Y);
                            path.AddLine(X, Y, tmp1, Y);
                            path.AddBezier(tmp1 -= 1f, Y, tmp2, Y, tmp2, tmp3, tmp1, tmp3);
                        }
                        break;
                    case ComponentType.AndNot:
                        {
                            var tmp1 = Y + Height;
                            var tmp2 = 0.42f * Height;
                            var tmp3 = (Height * 0.5f - tmp2) * 2f;
                            var tmp4 = X + 0.5f * Width - tmp3;
                            var tmp5 = X + 1.2f * Width - tmp3;

                            path.AddLine(tmp4, tmp1, X, tmp1);
                            path.AddLine(X, tmp1, X, Y);
                            path.AddLine(X, Y, tmp4, Y);
                            path.AddBezier(tmp4 -= 1f, Y, tmp5, Y, tmp5, tmp1, tmp4, tmp1);
                            path.AddEllipse((Width + X) - 0.9f * tmp3, tmp2 + Y, tmp3, tmp3);
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

                            path.AddBezier(X, Y, tmp1, Y, tmp1, tmp2, X, tmp2);
                            path.AddBezier(X, tmp2, tmp3, tmp2, tmp4, Y + 0.933f * Height, tmp6, tmp5);
                            path.AddBezier(tmp6, tmp5, tmp4, Y + 0.066f * Height, tmp3, Y, X, Y);
                        }
                        break;
                    case ComponentType.OrNot:
                        {
                            var tmp1 = 0.42f * Height;
                            var tmp2 = (Height * 0.5f - tmp1) * 2f;

                            path.AddEllipse((Width + X) - 0.9f * tmp2, tmp1 + Y, tmp2, tmp2);

                            tmp1 *= 0.4f;
                            var tmp3 = X + 0.4f * Width;
                            var tmp4 = Y + Height;
                            var tmp6 = X + 0.3f * Width - tmp1;
                            var tmp7 = X + 0.85f * Width - tmp1;
                            var tmp8 = Y + 0.5f * Height;
                            var tmp9 = (X + Width) - tmp1;

                            path.AddBezier(X, Y, tmp3, Y, tmp3, tmp4, X, tmp4);
                            path.AddBezier(X, tmp4, tmp6, tmp4, tmp7, Y + 0.933f * Height, tmp9, tmp8);
                            path.AddBezier(tmp9, tmp8, tmp7, Y + 0.066f * Height, tmp6, Y, X, Y);
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

                            path.AddBezier(tmp7, Y, tmp1, Y, tmp1, tmp2, tmp7, tmp2);
                            path.AddBezier(tmp7, tmp2, tmp3, tmp2, tmp4, Y + 0.933f * Height, tmp6, tmp5);
                            path.AddBezier(tmp6, tmp5, tmp4, Y + 0.066f * Height, tmp3, Y, tmp7, Y);

                            path.CloseFigure();

                            path.AddBezier(X, Y, tmp8, Y, tmp8, tmp2, X, tmp2);
                        }
                        break;
                    case ComponentType.XorNot:
                        {
                            var tmp7 = 0.42f * Height;
                            var tmp8 = (Height * 0.5f - tmp7) * 2;

                            path.AddEllipse(Width + X - 0.9f * tmp8, tmp7 + Y, tmp8, tmp8);

                            tmp7 *= 0.4f;
                            var tmp1 = X + 0.4f * Width;
                            var tmp2 = Y + Height;
                            var tmp3 = X + 0.3f * Width - tmp7;
                            var tmp4 = X + 0.85f * Width - tmp7;
                            var tmp5 = Y + 0.5f * Height;
                            var tmp6 = X + Width - tmp7;
                            var tmp9 = X + 0.125f * Width;
                            var tmp10 = tmp1 - 0.125f * Width;

                            path.AddBezier(tmp9, Y, tmp1, Y, tmp1, tmp2, tmp9, tmp2);
                            path.AddBezier(tmp9, tmp2, tmp3, tmp2, tmp4, Y + 0.933f * Height, tmp6, tmp5);
                            path.AddBezier(tmp6, tmp5, tmp4, Y + 0.066f * Height, tmp3, Y, tmp9, Y);

                            path.CloseFigure();

                            path.AddBezier(X, Y, tmp10, Y, tmp10, tmp2, X, tmp2);
                        }
                        break;
                    case ComponentType.Not:
                        {
                            var tmp1 = 0.42f * Height;
                            var tmp2 = (Height * 0.5f - tmp1) * 2;

                            path.AddEllipse(Width + X - 0.9f * tmp2, tmp1 + Y, tmp2, tmp2);

                            var tmp3 = X + Width - tmp1 * 0.4f;
                            var tmp4 = Y + 0.5f * Height;
                            var tmp5 = Y + Height;

                            path.AddLine(X, Y, X, tmp5);
                            path.AddLine(X, tmp5, tmp3, tmp4);
                            path.AddLine(tmp3, tmp4, X, Y);
                        }
                        break;
                    default:
                        break;
                }

                graphics.FillPath(Brushes.White, path);
                graphics.DrawPath(pen, path);
            }
        }

        public bool ContainsPoint(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height);

        public bool Move(int dx, int dy, object eventIgnore = null)
        {
            OnMove?.Invoke(this, new OnMoveEventArgs(dx, dy, eventIgnore));

            X += dx;
            Y += dy;

            return true;
        }
    }

    public class DrawableInput : Input, IDrawableElement
    {
        [JsonIgnore]
        public int Height => DrawableScheme.ElementHeight;
        [JsonIgnore]
        public int Width => DrawableScheme.ElementWidth;
        [JsonRequired]
        public int X { get; set; }
        [JsonRequired]
        public int Y { get; set; }

        [JsonConstructor]
        public DrawableInput(string name) : base(name) { }

        public DrawableInput(string name, int x, int y) : base(name)
        {
            X = x;
            Y = y;
        }

        public event EventHandler<OnMoveEventArgs> OnMove;

        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.FillRectangle(Brushes.White, X, Y, Width, Height);
            graphics.DrawRectangle(pen, X, Y, Width, Height);
            graphics.DrawString(Name, new Font("Arial", 8), new SolidBrush(pen.Color), X + Width * 0.3f, Y + Height * 0.3f);
        }

        public bool ContainsPoint(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height);

        public bool Move(int dx, int dy, object eventIgnore = null)
        {
            OnMove?.Invoke(this, new OnMoveEventArgs(dx, dy, eventIgnore));

            X += dx;
            Y += dy;

            return true;
        }
    }

    public class DrawableOutput : Output, IDrawableElement
    {
        [JsonIgnore]
        public int Height => DrawableScheme.ElementHeight;
        [JsonIgnore]
        public int Width => DrawableScheme.ElementWidth;
        [JsonRequired]
        public int X { get; set; }
        [JsonRequired]
        public int Y { get; set; }

        [JsonConstructor]
        public DrawableOutput(string name, string input) : base(name, input) { }

        public DrawableOutput(string name, string input, int x, int y) : base(name, input)
        {
            X = x;
            Y = y;
        }

        public event EventHandler<OnMoveEventArgs> OnMove;

        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.FillRectangle(Brushes.White, X, Y, Width, Height);
            graphics.DrawRectangle(pen, X, Y, Width, Height);
            graphics.DrawString(Name, new Font("Arial", 8), new SolidBrush(pen.Color), X + Width * 0.3f, Y + Height * 0.3f);
        }

        public bool ContainsPoint(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height);

        public bool Move(int dx, int dy, object eventIgnore = null)
        {
            OnMove?.Invoke(this, new OnMoveEventArgs(dx, dy, eventIgnore));

            X += dx;
            Y += dy;

            return true;
        }
    }

    public class LineSegment
    {
        [JsonRequired]
        public Direction Direction { get; set; }
        [JsonRequired]
        public int Length { get; set; }

        [JsonConstructor]
        public LineSegment(Direction direction, int length)
        {
            Direction = direction;
            Length = length;
        }
    }

    public class Line : IDrawable, ISelectable, IDisposable
    {
        [JsonRequired]
        public string StartName => Start.Name;
        [JsonRequired]
        public string EndName => End.Name;

        [JsonRequired]
        private List<LineSegment> _segments;

        [JsonIgnore]
        private IDrawableElement _start;
        [JsonIgnore]
        public IDrawableElement Start
        {
            get { return _start; }
            set
            {
                if (_start != null)
                    _start.OnMove -= Start_OnMove;

                if (value != null)
                    value.OnMove += Start_OnMove;

                _start = value;
            }
        }

        [JsonIgnore]
        private IDrawableElement _end;
        [JsonIgnore]
        public IDrawableElement End
        {
            get { return _end; }
            set
            {
                if (_end != null)
                    _end.OnMove -= End_OnMove;

                if (value != null)
                    value.OnMove += End_OnMove;

                _end = value;
            }
        }

        [JsonIgnore]
        public GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();

                int prevX = Start.X + Start.Width / 2,
                    prevY = Start.Y + Start.Height / 2;

                for (int i = 0; i < _segments.Count; ++i)
                    path.AddLine(
                        prevX, prevY,
                        _segments[i].Direction == Direction.X ? prevX += _segments[i].Length : prevX,
                        _segments[i].Direction == Direction.Y ? prevY += _segments[i].Length : prevY
                    );

                return path;
            }
        }

        [JsonConstructor]
        public Line(string startName, string endName, IEnumerable<LineSegment> segments)
        {
            _segments = segments.ToList();
            Start = new DrawableInput(startName);
            End = new DrawableInput(endName);
        }

        public Line(IDrawableElement start, IDrawableElement end)
        {
            _segments = new List<LineSegment>();
            Start = start;
            End = end;
        }

        public Line(IDrawableElement start, IDrawableElement end, IEnumerable<LineSegment> segments)
        {
            _segments = segments.ToList();
            Start = start;
            End = end;
        }

        public bool ContainsPoint(Point p)
        {
            using (var path = Path)
                return path.IsOutlineVisible(p, new Pen(Color.Black, 7));
        }

        public bool IsInRectangle(Rectangle bounds)
        {
            using (var path = Path)
                return path.PathPoints.Any(p => bounds.Contains(Point.Round(p)));
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            using (var path = Path)
                graphics.DrawPath(pen, path);
        }

        public void DrawWithTempPoint(Graphics graphics, Pen pen, Point point)
        {
            AddLineSegmentAtPoint(point, false, false);
            Draw(graphics, pen);
            _segments.RemoveAt(_segments.Count - 1);
        }

        public bool MoveSegment(int dx, int dy, Point segmentPoint)
        {
            int segmentIndex = -1;

            {
                int prevX = Start.X + Start.Width / 2,
                prevY = Start.Y + Start.Height / 2,
                delta;

                for (int i = 0; i < _segments.Count; ++i)
                    if (_segments[i].Direction == Direction.X)
                    {
                        delta = segmentPoint.X - prevX;
                        prevX += _segments[i].Length;

                        if (segmentPoint.Y == prevY && (_segments[i].Length >= 0 ? delta <= _segments[i].Length && delta > 0 : delta >= _segments[i].Length && delta < 0))
                        {
                            segmentIndex = i;
                            break;
                        }
                    }
                    else if (_segments[i].Direction == Direction.Y)
                    {
                        delta = segmentPoint.Y - prevY;
                        prevY += _segments[i].Length;

                        if (segmentPoint.X == prevX && (_segments[i].Length >= 0 ? delta <= _segments[i].Length && delta > 0 : delta >= _segments[i].Length && delta < 0))
                        {
                            segmentIndex = i;
                            break;
                        }
                    }
            }

            if (segmentIndex == -1)
                return false;

            if (segmentIndex - 1 >= 0 && segmentIndex + 1 < _segments.Count)
            {
                switch (_segments[segmentIndex].Direction)
                {
                    case Direction.X:
                        _segments[segmentIndex - 1].Length += dy;
                        _segments[segmentIndex + 1].Length -= dy;
                        break;
                    case Direction.Y:
                        _segments[segmentIndex - 1].Length += dx;
                        _segments[segmentIndex + 1].Length -= dx;
                        break;
                }
                return true;
            }
            else return false;
        }

        private void Start_OnMove(object sender, OnMoveEventArgs e)
        {
            if (e.Ignore == this && !_segments.Any())
                return;

            switch (_segments[0].Direction)
            {
                case Direction.X:
                    _segments[0].Length -= e.Dx;

                    if (_segments.Count > 1)
                        _segments[1].Length -= e.Dy;
                    else
                        End.Move(0, e.Dy, this);
                    break;
                case Direction.Y:
                    _segments[0].Length -= e.Dy;

                    if (_segments.Count > 1)
                        _segments[1].Length -= e.Dx;
                    else
                        End.Move(e.Dx, 0, this);
                    break;
                default:
                    break;
            }
        }

        private void End_OnMove(object sender, OnMoveEventArgs e)
        {
            if (e.Ignore == this && !_segments.Any())
                return;

            switch (_segments.Last().Direction)
            {
                case Direction.X:
                    _segments.Last().Length += e.Dx;

                    if (_segments.Count > 1)
                        _segments[_segments.Count - 2].Length += e.Dy;
                    else
                        Start.Move(0, e.Dy, this);
                    break;
                case Direction.Y:
                    _segments.Last().Length += e.Dy;

                    if (_segments.Count > 1)
                        _segments[_segments.Count - 2].Length += e.Dx;
                    else
                        Start.Move(e.Dx, 0, this);
                    break;
                default:
                    break;
            }
        }

        public void AddLineSegmentAtPoint(Point point, bool isLast, bool canAddToExisting = true)
        {
            Direction direction;
            int dx, dy, length;

            if (isLast)
            {
                point.X = End.X + End.Width / 2;
                point.Y = End.Y + End.Height / 2;
            }

            if (_segments.Any())
                using (var path = Path)
                {
                    dx = point.X - Point.Round(path.PathPoints.Last()).X;
                    dy = point.Y - Point.Round(path.PathPoints.Last()).Y;
                }
            else
            {
                dx = point.X - Start.X - Start.Width / 2;
                dy = point.Y - Start.Y - Start.Height / 2;
            }

            if (Math.Abs(dy) > Math.Abs(dx))
            {
                direction = Direction.Y;
                length = dy;
            }
            else
            {
                direction = Direction.X;
                length = dx;
            }


            if (canAddToExisting && _segments.Any() && _segments.Last().Direction == direction)
                _segments.Last().Length += length;
            else
                _segments.Add(new LineSegment(direction, length));

            if (isLast)
            {
                switch (direction)
                {
                    case Direction.X:
                        if (_segments.Count > 1)
                            _segments[_segments.Count - 2].Length += dy;
                        else
                            _segments.Add(new LineSegment(Direction.Y, dy));
                        break;
                    case Direction.Y:
                        if (_segments.Count > 1)
                            _segments[_segments.Count - 2].Length += dx;
                        else
                            _segments.Add(new LineSegment(Direction.X, dx));
                        break;
                }
            }
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Start = null;
                End = null;
            }
        }
    }
}
