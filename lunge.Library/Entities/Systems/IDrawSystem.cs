using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Systems
{
    public interface IDrawSystem : IUpdateSystem
    {
        void Draw(GameTime gameTime);
    }
}