using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui
{
    public interface IControl : IDisposable
    {
        string Name { get; }
        float DrawDepth { get; set; }
        IControl ParentControl { get; set; }

        void Initialize(Canvas canvas);
        void Update(GameTime gameTime, InputHandler inputHandler);

        /// <summary>
        /// Draws control using <see cref="SpriteBatch"/>. Note: do not call Begin() method
        /// as it is being called in the <see cref="Canvas"/>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);

        RectangleF GetBounds();
    }
}