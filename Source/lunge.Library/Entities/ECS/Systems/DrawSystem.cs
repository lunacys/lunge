using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Entities.ECS.Systems
{
    public abstract class DrawSystem : IDrawSystem
    {
        public SpriteBatch SpriteBatch { get; }

        protected DrawSystem(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
        }

        public virtual void Dispose() { }
        public virtual void Initialize(World world) { }
        public abstract void Draw(GameTime gameTime);
    }
}