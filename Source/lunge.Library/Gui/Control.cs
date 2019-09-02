using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace lunge.Library.Gui
{
    public abstract class Control : IRectangular
    {
        /// <summary>
        /// Gets the name of the control.
        /// Name must be unique as it acts as an unique identifier.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets current position of the control.
        /// The position of a control is relative to its parent.
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set => MoveTo(value);
        }
        public Size2 Size
        {
            get => _size;
            set => Resize(value);
        }
        public virtual Rectangle BoundingRectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.Width, (int)Size.Height);

        public Control Parent { get; set; }

        private Vector2 _position;
        private Size2 _size;
        private Rectangle _boundingRectangle;

        protected Control(string name, Control parent = null)
        {
            Name = name;
            Parent = parent;
        }

        private void Resize(Size2 newSize)
        {

        }

        public void Move(Vector2 direction)
        {

        }

        public void MoveTo(Vector2 newPosition)
        {

        }
    }
}