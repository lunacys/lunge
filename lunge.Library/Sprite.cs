using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effect { get; set; } = SpriteEffects.None;
        public float Depth { get; set; }
    }
}