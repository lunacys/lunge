using Microsoft.Xna.Framework;

namespace lunge.Library.Gui
{
    public class MouseEventHandler
    {
        public Point MousePosition { get; set; }

        public MouseEventHandler(Point mousePosition)
        {
            MousePosition = mousePosition;
        }
    }
}