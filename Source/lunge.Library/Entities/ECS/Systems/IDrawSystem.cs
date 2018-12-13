using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.ECS.Systems
{
    public interface IDrawSystem : ISystem
    {
        void Draw(GameTime gameTime);
    }
}