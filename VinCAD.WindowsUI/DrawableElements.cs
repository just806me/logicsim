using VinCAD.Main;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace VinCAD.WindowsUI
{
    public class OnMoveEventArgs : EventArgs
    {
        public int Dx;
        public int Dy;

        public OnMoveEventArgs(int dx, int dy)
        {
            Dx = dx;
            Dy = dy;
        }
    }

    public class DrawableComponent : Component, IDrawableElement
    {
        [JsonRequired]
        public int Height { get; private set; }

        [JsonRequired]
        public int Width { get; private set; }

        [JsonRequired]
        public int X { get; private set; }

        [JsonRequired]
        public int Y { get; private set; }

        [JsonConstructor]
        public DrawableComponent(string name, ComponentType type, IEnumerable<string> input) : base(name, type, input) { }

        public DrawableComponent(string name, ComponentType type, IEnumerable<string> input, int x, int y, int width, int height) : base(name, type, input)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public event EventHandler<OnMoveEventArgs> OnMove;

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

        public bool ContainsPoint(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height);

        public void Move(Point moveTo)
        {
            OnMove?.Invoke(this, new OnMoveEventArgs(X - moveTo.X, Y - moveTo.Y));

            X = moveTo.X;
            Y = moveTo.Y;
        }
    }

    public class DrawableInput : Input, IDrawableElement
    {
        [JsonRequired]
        public int Height { get; private set; }
        [JsonRequired]
        public int Width { get; private set; }
        [JsonRequired]
        public int X { get; private set; }
        [JsonRequired]
        public int Y { get; private set; }

        [JsonConstructor]
        public DrawableInput(string name) : base(name) { }

        public DrawableInput(string name, int x, int y, int width, int height) : base(name)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public event EventHandler<OnMoveEventArgs> OnMove;

        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.DrawRectangle(pen, X, Y, Width, Height);
            graphics.DrawString(Name, new Font("Arial", 8), new SolidBrush(pen.Color), X + Width * 0.3f, Y + Height * 0.3f);
        }

        public bool ContainsPoint(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height);

        public void Move(Point moveTo)
        {
            OnMove?.Invoke(this, new OnMoveEventArgs(X - moveTo.X, Y - moveTo.Y));

            X = moveTo.X;
            Y = moveTo.Y;
        }
    }

    public class DrawableOutput : Output, IDrawableElement
    {
        [JsonRequired]
        public int Height { get; private set; }
        [JsonRequired]
        public int Width { get; private set; }
        [JsonRequired]
        public int X { get; private set; }
        [JsonRequired]
        public int Y { get; private set; }

        [JsonConstructor]
        public DrawableOutput(string name, string input) : base(name, input) { }

        public DrawableOutput(string name, string input, int x, int y, int width, int height) : base(name, input)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        public event EventHandler<OnMoveEventArgs> OnMove;

        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.DrawRectangle(pen, X, Y, Width, Height);
            graphics.DrawString(Name, new Font("Arial", 8), new SolidBrush(pen.Color), X + Width * 0.3f, Y + Height * 0.3f);
        }

        public bool ContainsPoint(Point p) => new Rectangle(X, Y, Width, Height).Contains(p);

        public bool IsInRectangle(Rectangle rectangle)
            => rectangle.Contains(X, Y)
            || rectangle.Contains(X, Y + Height)
            || rectangle.Contains(X + Width, Y)
            || rectangle.Contains(X + Width, Y + Height);

        public void Move(Point moveTo)
        {
            OnMove?.Invoke(this, new OnMoveEventArgs(X - moveTo.X, Y - moveTo.Y));

            X = moveTo.X;
            Y = moveTo.Y;
        }
    }

    public class Line : IDrawable, IMoveable, ISelectable
    {
        private IMoveable _start;
        private IMoveable _end;

        public IMoveable Start
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
        public IMoveable End
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

        public Direction Direction { get; set; }
        public int Length { get; set; }

        public int X => _start.X + _start.Width;
        public int Y => _start.Y + _start.Height;
        public int Height
        {
            get
            {
                switch (Direction)
                {
                    case Direction.Y:
                        return Length;
                    case Direction.X:
                    default:
                        return 0;
                }
            }
        }
        public int Width
        {
            get
            {
                switch (Direction)
                {
                    case Direction.X:
                        return Length;
                    case Direction.Y:
                    default:
                        return 0;
                }
            }
        }

        public event EventHandler<OnMoveEventArgs> OnMove;

        public Line(IMoveable start, IMoveable end, Direction direction, int length)
        {
            Start = start;
            End = end;
            Direction = direction;
            Length = length;
        }

        private void Start_OnMove(object sender, OnMoveEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void End_OnMove(object sender, OnMoveEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Draw(Graphics graphics, Pen pen)
            => graphics.DrawLine(pen, X, Y, X + Width, Y + Height);

        public bool ContainsPoint(Point p)
        {
            switch (Direction)
            {
                case Direction.X:
                    return p.Y == Y && (p.X - X <= Length);
                case Direction.Y:
                    return p.X == X && (p.Y - Y <= Length);
                default:
                    return false;
            }
        }

        public bool IsInRectangle(Rectangle bounds)
            => bounds.Contains(X, Y)
            || bounds.Contains(X + Width, Y + Height);

        public void Move(Point moveTo)
        {
            OnMove?.Invoke(this, new OnMoveEventArgs(moveTo.X - X, moveTo.Y - Y));

            if (_start is IDrawableElement)
                switch (Direction)
                {
                    case Direction.X:
                        ((IDrawableElement)_start).Move(new Point(moveTo.X, _start.Y));
                        break;
                    case Direction.Y:
                        ((IDrawableElement)_start).Move(new Point(_start.X, moveTo.Y));
                        break;
                }

            if (_end is IDrawableElement)
                switch (Direction)
                {
                    case Direction.X:
                        ((IDrawableElement)_end).Move(new Point(moveTo.X, _end.Y));
                        break;
                    case Direction.Y:
                        ((IDrawableElement)_end).Move(new Point(_end.X, moveTo.Y));
                        break;
                }
        }
    }
}
