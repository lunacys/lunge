using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Old
{
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public interface IControl
    {
        string Name { get; }
        float DrawDepth { get; set; }
        IControl ParentControl { get; set; }
        ControlList ChildControls { get; set; }
        Canvas UsedCanvas { get; set; }
        Vector2 Position { get; set; }
        Size2 Size { get; set; }

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