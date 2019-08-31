using System;
using System.Collections.Generic;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui
{
    public interface IControl
    {
        string Name { get; }
        float DrawDepth { get; set; }
        IControl ParentControl { get; set; }
        ControlList ChildControls { get; set; }
        Canvas UsedCanvas { get; set; }

        event EventHandler MouseHover;
        event EventHandler MouseIn;
        event EventHandler MouseOut;

        void Initialize(Canvas canvas);
        void Update(GameTime gameTime, InputHandler inputHandler);
        void Close();

        /// <summary>
        /// Draws control using <see cref="SpriteBatch"/>. Note: do not call Begin() method
        /// as it is being called in the <see cref="Canvas"/>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);

        RectangleF GetBounds();
    }
}