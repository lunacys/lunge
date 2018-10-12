using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.GameSystems
{
    public abstract class DrawableGameSystem : GameSystem
    {
        public SpriteBatch SpriteBatch { get; }

        protected DrawableGameSystem(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
