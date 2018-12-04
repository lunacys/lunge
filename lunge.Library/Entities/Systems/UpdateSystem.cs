using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Systems
{
    public abstract class UpdateSystem : IUpdateSystem
    {
        public virtual void Dispose() { }
        public virtual void Initialize(World world) { }
        public abstract void Update(GameTime gameTime);
    }
}