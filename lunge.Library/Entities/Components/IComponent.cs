using Microsoft.Xna.Framework;

namespace lunge.Library.Entities.Components
{
    public interface IComponent
    {
        bool IsActive { get; set; }
        void Initialize(World world);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}