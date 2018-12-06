using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace lunge.Library.Entities
{
    public class World : SimpleDrawableGameComponent
    {
        internal EntityManager EntityManager { get; }

        public int EntityCount => EntityManager.ActiveCount;

        internal World()
        {
            EntityManager = new EntityManager(this);
        }

        public void AddEntity(Entity entity)
        {
            EntityManager.Add(entity);
        }

        public Entity GetEntity(int entityId)
        {
            return EntityManager.Get(entityId);
        }

        public void DestroyEntity(int entityId)
        {
            EntityManager.Destroy(entityId);
        }

        public void DestroyEntity(Entity entity)
        {
            EntityManager.Destroy(entity);
        }

        public override void Update(GameTime gameTime)
        {
            EntityManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            EntityManager.Draw(gameTime);
        }
    }
}