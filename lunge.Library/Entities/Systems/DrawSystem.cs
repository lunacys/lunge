using lunge.Library.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Entities.Systems
{
    public abstract class DrawSystem : UpdateSystem, IDrawSystem
    {
        public virtual void Draw(GameTime gameTime) { }
    }
}
