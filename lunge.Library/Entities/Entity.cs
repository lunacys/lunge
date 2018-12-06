using lunge.Library.Entities.Components;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public abstract class Entity
    {
        public int Id { get; internal set; }
        public EntityManager EntityManager { get; internal set; }

        public bool IsExpired { get; private set; }

        protected Entity() { }
                
        public void Destroy()
        {
            IsExpired = true;
        }

        public virtual void Initialize(World world) { }

        public virtual void Deinitialize() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
    }
}