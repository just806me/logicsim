using System;
using System.Drawing;
using VinCAD.Main;

namespace VinCAD.WindowsUI
{
    public enum Direction { X, Y }

    public interface IDrawable
    {
        void Draw(Graphics graphics, Pen pen);
    }

    public interface IMoveable
    {
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }

        void Move(Point moveTo);
        event EventHandler<OnMoveEventArgs> OnMove;
    }

    public interface ISelectable
    {
        bool ContainsPoint(Point p);
        bool IsInRectangle(Rectangle bounds);
    }

    public interface IDrawableElement : IElement, IDrawable, IMoveable, ISelectable { }
}