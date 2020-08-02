using System;
using lunge.Library.Screens;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Systems
{
    public abstract class UpdateSystem : IUpdateSystem, IDisposable
    {
        /// <summary>
        /// Gets <see cref="ISystemManager"/>
        /// </summary>
        public ISystemManager SystemManager { get; set; }

        /// <summary>
        /// Gets or sets whether the game system is working
        /// </summary>
        public bool IsActive { get; set; }
        
        protected UpdateSystem()
        {
            IsActive = true;
        }

        /// <summary>
        /// Finds and returns <see cref="DrawSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="DrawSystem"/></returns>
        public T FindSystem<T>() where T : DrawSystem
        {
            return SystemManager?.FindSystem<T>();
        }

        public virtual void Initialize(World world) { }

        public virtual void Update(GameTime gameTime) { }

        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}