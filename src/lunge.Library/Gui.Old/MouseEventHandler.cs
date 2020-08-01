using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.Gui.Old
{
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public class MouseEventHandler
    {
        public Point MousePosition { get; set; }

        public MouseEventHandler(Point mousePosition)
        {
            MousePosition = mousePosition;
        }
    }
}