using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Gui
{
    public abstract class ControlBase : IControl, IDisposable
    {
        public string Name { get; }

        public event EventHandler Created;
        public event EventHandler Initialized;
        public event EventHandler Disposed;

        public ControlBase(string name)
        {
            Name = name;

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
        public virtual void Update(GameTime gameTime, InputHandler inputHandler) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}