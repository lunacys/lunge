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
            var mousePosition = inputHandler.MousePositionScreenToWorld;

        }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        protected void MoveControlAndItsChildren(IControl controlToMove, Vector2 velocity)
        {
            if (controlToMove is IGraphicsControl graphicsControl)
            {
                graphicsControl.Position += velocity;
            }

            foreach (var childControl in controlToMove.ChildControls)
            {
                MoveControlAndItsChildren(childControl, velocity);
            }
        }

        protected void MoveControls(Vector2 velocity)
        {
            MoveControlAndItsChildren(this, velocity);
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