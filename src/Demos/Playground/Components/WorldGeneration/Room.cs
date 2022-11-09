using Microsoft.Xna.Framework;
using Nez;

namespace Playground.Components.WorldGeneration;

public class Room
{
    public RectangleF Rectangle { get; }
    public Vector2 Center { get; }

    public Room(RectangleF rect)
    {
        Rectangle = rect;
        Center = rect.Center;
    }
}