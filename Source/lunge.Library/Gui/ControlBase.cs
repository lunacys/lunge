using System;
using System.Collections.Generic;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui
{
    public abstract class ControlBase : IControl
    {
        public string Name { get; }
        public float DrawDepth { get; set; } = 1.0f;
        public IControl ParentControl { get; set; }
        public Canvas UsedCanvas { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual Size2 Size { get; set; }

        private Vector2? _origPos;

        public ControlList ChildControls { get; set; }

        public event EventHandler Created;
        public event EventHandler Initialized;
        public event EventHandler Disposed;

        public event EventHandler MouseHover;
        public event EventHandler MouseIn;
        public event EventHandler MouseOut;

        public ControlBase(string name, IControl parentControl = null)
        {
            ChildControls = new ControlList();

            Name = name;
            ParentControl = parentControl;

            Created?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes the current control element.
        /// Note: call <code>base.Initialize(canvas);</code> at the end of the method
        /// as it invokes the 'Initialized' event
        /// </summary>
        /// <param name="canvas"><see cref="Canvas"/> on which the element is placed</param>
        public virtual void Initialize(Canvas canvas)
        {
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Update(GameTime gameTime, InputHandler inputHandler)
        {
            if (!_origPos.HasValue)
                _origPos = Position;

            var mousePosition = inputHandler.MousePositionScreenToWorld;

            if (ParentControl != null)
            {
                Position = Vector2.Add(ParentControl.Position, _origPos.Value);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        protected void MoveControlAndItsChildren(IControl controlToMove, Vector2 newPosition)
        {
            controlToMove.Position = newPosition;

            foreach (var childControl in controlToMove.ChildControls)
            {
                MoveControlAndItsChildren(childControl, newPosition);
            }
        }

        protected void MoveControls(Vector2 newPosition)
        {
            MoveControlAndItsChildren(this, newPosition);
        }

        protected void MoveChildrenControls(Vector2 newPosition)
        {
            foreach (var childControl in ChildControls)
            {
                MoveControlAndItsChildren(childControl, newPosition);
            }
        }

        public void Close()
        {
            foreach (var childControl in ChildControls)
            {
                childControl.Close();
            }

            UsedCanvas.RemoveControl(Name);
        }

        public virtual RectangleF GetBounds()
        {
            return new RectangleF();
        }
    }
}