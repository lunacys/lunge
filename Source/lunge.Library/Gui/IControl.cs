using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Gui
{
    public interface IControl
    {
        string Name { get; }

        void Initialize(Canvas canvas);
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws control using <see cref="SpriteBatch"/>. Note: do not call Begin() method
        /// as it is being called in the <see cref="Canvas"/>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);
    }
}