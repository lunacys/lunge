using Microsoft.Xna.Framework;

namespace lunge.Library.Gui
{
    public class MovedEventHandler
    {
        public Vector2 NewPosition { get; set; }

        public MovedEventHandler(Vector2 newPosition)
        {
            NewPosition = newPosition;
        }
    }
}