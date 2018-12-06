using System;
using lunge.Library.Entities.ECS.Systems;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.ECS
{
    public class ComponentManager : UpdateSystem
    {
        public event Action<int> ComponentsChanged;

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}