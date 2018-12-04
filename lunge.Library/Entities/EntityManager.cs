using System;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public class EntityManager : UpdateSystem
    {
        public int ActiveCount => throw new System.NotImplementedException();

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public Entity Create()
        {
            throw new NotImplementedException();
        }

        public Entity Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public void Destroy(int entityId)
        {
            throw new NotImplementedException();
        }

        internal void Destroy(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}