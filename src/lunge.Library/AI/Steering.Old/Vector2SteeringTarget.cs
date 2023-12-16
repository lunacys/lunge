using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Old
{
    public struct Vector2SteeringTarget : ISteeringTarget
    {
        public Vector2 Position { get; set; }
        public bool IsActual => true;

        public Vector2SteeringTarget(Vector2 value)
        {
            Position = value;
        }   

        public static explicit operator Vector2SteeringTarget(Vector2 value)
        {
            return new Vector2SteeringTarget(value);
        }

        public static explicit operator Vector2(Vector2SteeringTarget value)
        {
            return value.Position;
        }
    }
}