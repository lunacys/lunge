using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Systems
{
    public interface IDrawSystem : ISystem
    {
        void Draw(GameTime gameTime);
    }
}