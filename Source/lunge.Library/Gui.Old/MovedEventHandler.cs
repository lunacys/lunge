using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.Gui.Old
{
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public class MovedEventHandler
    {
        public Vector2 NewPosition { get; set; }

        public MovedEventHandler(Vector2 newPosition)
        {
            NewPosition = newPosition;
        }
    }
}