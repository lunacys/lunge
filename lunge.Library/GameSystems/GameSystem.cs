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

        protected Game GameRoot { get; }

        protected GameSystem(Game game)
        {
            GameRoot = game;
        }

        /// <summary>
        /// Finds and returns <see cref="DrawableGameSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="DrawableGameSystem"/></returns>
        public T FindSystem<T>() where T : DrawableGameSystem
        {
            return GameSystemManager?.FindSystem<T>();
        }

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }

        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Reset()
        {
            OnReset?.Invoke(this, EventArgs.Empty);
        }
    }
}