using System;
using Microsoft.Xna.Framework;
using Nez;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace lunge.Library.AI.Steering.Old
{
    public class SteeringEntity : Entity, ISteeringEntity, ISteeringTarget
    {
        Vector2 ISteeringEntity.Position
        {
            get => Position;
            set => Position = value;
        }

        Vector2 ISteeringTarget.Position
        {
            get => Position;
            set => Position = value;
        }

        public bool IsActual => true;

        public event EventHandler ResetEvent;
        
        public Vector2 DesiredVelocity { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Steering { get; set; }
        public float MaxVelocity { get; set; }
        public float MaxForce { get; set; }
        public float Mass { get; set; }

        public Entity BaseEntity => this;

        float ISteeringEntity.Rotation
        {
            get => Rotation;
            set => Rotation = value;
        }

        public SteeringEntity()
        {
            MaxVelocity = 4.0f;
            MaxForce = 3.8f;
            Mass = 1.0f;
        }

        public void Reset()
        {
            Steering = Vector2.Zero;
            ResetEvent?.Invoke(this, EventArgs.Empty);
        }

        public override void DebugRender(Batcher batcher)
        {
            base.DebugRender(batcher);
            
            // Steering Line (Blue)
            batcher.DrawLine(Position, Position + Steering * 30, Color.Blue, 2);
            // Velocity Line (Green)
            batcher.DrawLine(Position, Position + Velocity * 20, Color.Green, 2);
            // Desired Velocity Line (Red)
            batcher.DrawLine(Position, Position + DesiredVelocity * 30, Color.Red, 2);
        }

        
    }
}