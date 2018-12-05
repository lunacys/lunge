using System;
using lunge.Library.Entities.Systems;
using Microsoft.Xna.Framework;

namespace lunge.Library.Entities
{
    public class ComponentManager : UpdateSystem
    {
        public event Action<int> ComponentsChanged;

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}