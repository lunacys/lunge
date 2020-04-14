using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Collisions
{
    public struct Obstacle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public static int GlobalId = 0;

        public static Obstacle NullObstacle => new Obstacle(Vector2.Zero, 0.0f);

        public Obstacle(Vector2 circleCenter, float circleRadius)
        {
            Center = circleCenter;
            Radius = circleRadius;
            GlobalId++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(Center, Radius, (int)Radius, Color.Red, 1);
        }

        public bool Equals(Obstacle other)
        {
            return Center.Equals(other.Center) && Radius.Equals(other.Radius);
        }

        public override bool Equals(object obj)
        {
            return obj is Obstacle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Center, Radius);
        }

        public static bool operator ==(Obstacle o1, Obstacle o2)
        {
            return o1.Center == o2.Center && o1.Radius == o2.Radius;
        }

        public static bool operator !=(Obstacle o1, Obstacle o2)
        {
            return !(o1 == o2);
        }
    }
}