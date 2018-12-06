using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.ECS.Systems
{
    public interface IUpdateSystem : ISystem
    {
        void Update(GameTime gameTime);
    }
}