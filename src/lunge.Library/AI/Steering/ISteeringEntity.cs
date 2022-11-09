using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering
{
    public interface ISteeringEntity
    {
        event EventHandler ResetEvent;

        Vector2 Position { get; set; }
        Vector2 DesiredVelocity { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 Steering { get; set; }

        float MaxVelocity { get; set; }
        float MaxForce { get; set; }
        float Mass { get; set; }
        
        public float Rotation { get; set; }

        void Reset();
    }
}