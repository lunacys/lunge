using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Components
{
    public class TransformComponent : IComponent
    {
        public bool IsActive { get; set; } = true;

        public Vector2 Position { get; set; }

        public void Initialize(World world)
        { }

        public void Update(GameTime gameTime)
        { }

        public void Draw(GameTime gameTime)
        { }
    }
}