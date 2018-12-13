using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.ECS.Components
{
    public interface IComponent
    {
        bool IsActive { get; set; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}