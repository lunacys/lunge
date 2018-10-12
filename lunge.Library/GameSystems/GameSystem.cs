using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.GameSystems
{
    public abstract class GameSystem : IDisposable
    {
        public event EventHandler OnReset;

        /// <summary>
        /// Gets <see cref="IGameSystemManager"/>
        /// </summary>
        public IGameSystemManager GameSystemManager { get; internal set; }

        /// <summary>
        /// Gets or sets whether the game system is working
        /// </summary>
        public bool IsActive { get; set; }

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }

        public virtual void Reset()
        {
            OnReset?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}