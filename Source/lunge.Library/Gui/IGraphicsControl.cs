using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace lunge.Library.Gui
{
    public interface IGraphicsControl
    {
        Vector2 Position { get; set; }
        Size2 Size { get; set; }
    }
}