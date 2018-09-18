using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.GameSystems
{
    public abstract class GameSystem : IDisposable
    {
        public event EventHandler OnReset;

        protected GameSystem()
        { }

        /// <summary>
        /// Gets <see cref="IGameSystemManager"/>
        /// </summary>
        public IGameSystemManager GameSystemManager { get; internal set; }

        /// <summary>
        /// Gets or sets whether the game system is working
        /// </summary>
        public bool IsWorking { get; set; }

        /// <summary>
        /// Finds and returns <see cref="GameSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="GameSystem"/></returns>
        public T FindSystem<T>() where T : GameSystem
        {
            return GameSystemManager?.FindSystem<T>();
        }
        
        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        public virtual void Reset()
        {
            OnReset?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Dispose() { }
    }
}
