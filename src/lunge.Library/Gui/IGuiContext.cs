using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Gui
{
    public interface IGuiContext
    {
        SpriteFont DefaultFont { get; }
        Vector2 CursorPosition { get; }
        Control FocusedControl { get; }

        void SetFocus(Control focusedControl);
    }
}