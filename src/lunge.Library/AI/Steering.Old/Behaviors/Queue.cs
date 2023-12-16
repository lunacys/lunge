using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Old.Behaviors
{
    public class Queue : SteeringComponentBase
    {
        private Func<ISteeringEntity, ISteeringEntity> _getNeighborAheadFunc;
        private float _maxQueueRadius;

        public Queue(Func<ISteeringEntity, ISteeringEntity> getNeighborAheadFunc, float maxQueueRadius)
        {
            _getNeighborAheadFunc = getNeighborAheadFunc;
            _maxQueueRadius = maxQueueRadius;
        }

        public override Vector2 Steer(ISteeringTarget target)
        {
            var v = SteeringEntity.Velocity;
            var brake = Vector2.Zero;
            var neighbor = _getNeighborAheadFunc.Invoke(SteeringEntity);

            if (neighbor != null)
            {
                brake = -SteeringEntity.Steering * 0.8f;
                v *= -1;
                brake += v;

                if (Vector2.Distance(SteeringEntity.Position, neighbor.Position) <= _maxQueueRadius)
                    SteeringEntity.Velocity *= 0.3f;
            }

            return brake;
        }
    }
}