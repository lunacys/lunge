﻿using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public abstract class Entity : IDisposable
    {
        public int Id { get; internal set; }
        public EntityManager EntityManager { get; internal set; }

        public bool IsExpired { get; private set; }

        protected Entity() { }
                
        public void Destroy()
        {
            IsExpired = true;
        }

        public virtual void Initialize(World world) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }

        public virtual void Dispose()
        {
	        EntityManager?.Dispose();
        }
    }
}