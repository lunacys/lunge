using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Old.Behaviors
{
    public class LeaderFollowing : SteeringComponentBase
    {
        public ISteeringEntity Leader { get; set; }
        [Inspectable]
        public float LeaderBehindDist { get; set; }
        [Inspectable]
        public float LeaderSightRadius { get; set; }

        private Arrival _arrival;

        private Vector2 _behind, _ahead;

        public LeaderFollowing(ISteeringEntity leader, float leaderBehindDist, float leaderSightRadius)
        {
            Leader = leader;
            LeaderBehindDist = leaderBehindDist;
            LeaderSightRadius = leaderSightRadius;
        }

        public override void Initialize()
        {
            base.Initialize();

            _arrival = new Arrival(16f); // TODO: Add as Nested Behavior
            _arrival.SteeringEntity = SteeringEntity;
        }

        public override Vector2 Steer(ISteeringTarget target)
        {
            var dv = Leader.Velocity;
            var force = Vector2.Zero;

            dv.Normalize();
            dv *= LeaderBehindDist;
            _ahead = Leader.Position + dv;

            dv *= -1;
            _behind = Leader.Position + dv;

            //if (IsOnLeaderSight(_ahead))
            //    force += _evade.Steer(Leader as ISteeringTarget);

            ISteeringBehavior nestedBehavior = NestedBehavior ?? _arrival;

            return force + nestedBehavior.Steer((Vector2SteeringTarget) _behind);
        }

        private bool IsOnLeaderSight(Vector2 leaderAhead)
        {
            return Vector2.Distance(leaderAhead, SteeringEntity.Position) <= LeaderSightRadius ||
                   Vector2.Distance(Leader.Position, SteeringEntity.Position) <= LeaderSightRadius;
        }

        public override void DebugRender(Batcher batcher)
        {
            batcher.DrawCircle(_behind, 8f, Color.Green);
        }
    }
}