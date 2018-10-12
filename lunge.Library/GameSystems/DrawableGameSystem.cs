using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.GameSystems
{
    public abstract class DrawableGameSystem : GameSystem
    {
        /// Gets <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> which is currently in use
        public SpriteBatch SpriteBatch { get; }

        protected DrawableGameSystem(Game game)
            : base(game)
        {
            SpriteBatch = new SpriteBatch(GameRoot.GraphicsDevice);
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
