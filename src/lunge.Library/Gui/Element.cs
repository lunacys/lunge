using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace lunge.Library.Gui
{
    public abstract class Element
    {
        public string Name { get; set; }
        public Point Position { get; set; } = Point.Zero;
        public Point Origin { get; set; } = Point.Zero;
        public Color BackgroundColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.White;
        public int BorderWidth { get; set; } = 0;

        private Size _size;

        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                OnSizeChanged();
            }
        }

        protected virtual void OnSizeChanged() { }

        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; } = int.MaxValue;
        public int MaxHeight { get; set; } = int.MaxValue;

        public int Width
        {
            get => Size.Width;
            set => Size = new Size(value, Size.Height);
        }

        public int Height
        {
            get => Size.Height;
            set => Size = new Size(Size.Width, value);
        }
        public Size ActualSize { get; internal set; }

        public abstract void Draw(IGuiContext context, IGuiRenderer renderer, float deltaSeconds);
    }

    public abstract class Element<TParent> : Element, IRectangular
        where TParent : IRectangular
    {
        public TParent Parent { get; internal set; }

        public Rectangle BoundingRectangle
        {
            get
            {
                var offset = Point.Zero;

                if (Parent != null)
                    offset = Parent.BoundingRectangle.Location;

                return new Rectangle(offset + Position - ActualSize * Origin, ActualSize);
            }
        }
    }
}