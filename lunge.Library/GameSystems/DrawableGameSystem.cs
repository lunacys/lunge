using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.GameSystems
{
    public abstract class DrawableGameSystem : GameSystem
    {
        public event EventHandler OnReset;

        protected DrawableGameSystem()
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
        public virtual void Draw(GameTime gameTime) { }

        public virtual void Reset()
        {
            OnReset?.Invoke(this, EventArgs.Empty);
        }
    }
}
