using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Systems
{
    public interface IUpdateSystem : ISystem
    {
        void Update(GameTime gameTime);
    }
}