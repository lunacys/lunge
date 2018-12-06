using lunge.Library.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Entities.Systems
{
    public abstract class DrawSystem : UpdateSystem, IDrawSystem
    {
        /// Gets <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> which is currently in use
        public SpriteBatch SpriteBatch { get; }

        protected DrawSystem(World world)
            : base(world)
        {
            SpriteBatch = new SpriteBatch(world.GameRoot.GraphicsDevice);
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
