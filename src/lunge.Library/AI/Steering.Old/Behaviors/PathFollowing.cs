using System;
using lunge.Library.AI.Steering.Old.Pathing;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Old.Behaviors
{
    public enum PathFollowingMode
    {
        OneWay,
        Circular,
        Patrol
    }

    public class PathFollowing : SteeringComponentBase
    {
        private readonly ISteeringBehavior _pathFollowingBehavior;

        [Inspectable]
        public PathFollowingMode PathFollowingMode { get; set; }

        private int _currentNode = 0;
        private int _pathDir = -1;

        public PathFollowing(ISteeringBehavior pathFollowingBehavior, PathFollowingMode mode = PathFollowingMode.OneWay)
        {
            _pathFollowingBehavior = pathFollowingBehavior;
            PathFollowingMode = mode;
        }

        public override void Initialize()
        {
            base.Initialize();

            _pathFollowingBehavior.SteeringEntity = SteeringEntity;
        }

        public override Vector2 Steer(ISteeringTarget target)
        {
            if (!target.IsActual) // We've completed the path, need to stop now
                return -SteeringEntity.Velocity;

            var pathComp = target as PathComponent;
            if (pathComp == null)
                throw new Exception("Incorrect pathing target (use PathComponent)");

            var path = pathComp.Path;

            if (path.NodeCount == 0)
                return -SteeringEntity.Velocity;

            if (PathFollowingMode == PathFollowingMode.Patrol)
            {
                var targetNode = path[_currentNode];

                if (targetNode != null)
                {
                    /*var distance = (SteeringEntity.Position - targetNode.Target).Length();

                    if (distance <= targetNode.TargetRadius)*/
                    if (IsWithinTarget(targetNode))
                    {
                        _currentNode += _pathDir;

                        if (_currentNode >= path.NodeCount || _currentNode < 0)
                        {
                            _pathDir *= -1;
                            _currentNode += _pathDir;
                        }

                        targetNode = path[_currentNode];
                    }

                    return _pathFollowingBehavior.Steer((Vector2SteeringTarget) targetNode.Target);
                }
            }
            else if (PathFollowingMode == PathFollowingMode.Circular)
            {
                var targetNode = path[_currentNode % path.NodeCount];

                if (targetNode != null)
                {
                    /*var distance = (SteeringEntity.Position - targetNode.Target).Length();

                    if (distance <= targetNode.TargetRadius)*/
                    if (IsWithinTarget(targetNode))
                    {
                        _currentNode++;
                        targetNode = path[_currentNode % path.NodeCount];
                    }

                    return _pathFollowingBehavior.Steer((Vector2SteeringTarget)targetNode.Target);
                }
            }
            else if (PathFollowingMode == PathFollowingMode.OneWay)
            {
                var targetNode = path.GetTargetNode();
                if (targetNode != null)
                {
                    /*var distance = (target.Position - SteeringEntity.Position).Length();

                    if (distance <= targetNode.TargetRadius)*/
                    if (IsWithinTarget(targetNode))
                        path.RemoveTargetNode();

                    return _pathFollowingBehavior.Steer((Vector2SteeringTarget)targetNode.Target);
                }
            }

            return -SteeringEntity.Velocity;
        }

        private bool IsWithinTarget(PathNode target)
        {
            var distance = (SteeringEntity.Position - target.Target).Length();
            return distance <= target.TargetRadius;
        }
    }
}