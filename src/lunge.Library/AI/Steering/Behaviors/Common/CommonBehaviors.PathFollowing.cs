using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Steering.Behaviors.Common;

public static partial class CommonBehaviors
{
    public static Vector2 FollowPathPatrol(
        SteeringHost host, 
        FollowingPath path, 
        ref int currentNode,
        ref int pathDir,
        Func<Vector2, Vector2> steeringFunc)
    {
        var targetNode = path[currentNode];

        if (targetNode != null)
        {
            /*var distance = (SteeringEntity.Position - targetNode.Target).Length();

            if (distance <= targetNode.TargetRadius)*/
            if (IsWithinTarget(host, targetNode))
            {
                currentNode += pathDir;

                if (currentNode >= path.NodeCount || currentNode < 0)
                {
                    pathDir *= -1;
                    currentNode += pathDir;
                }

                targetNode = path[currentNode];
            }

            return steeringFunc(targetNode.Target);
        }

        return -host.Velocity;
    }

    public static Vector2 FollowPathOneWay(SteeringHost host, 
        FollowingPath path,
        Func<Vector2, Vector2> steeringFunc
        )
    {
        var targetNode = path.GetTargetNode();
        if (targetNode != null)
        {
            /*var distance = (target.Position - SteeringEntity.Position).Length();

            if (distance <= targetNode.TargetRadius)*/
            if (IsWithinTarget(host, targetNode))
                path.RemoveTargetNode();

            return steeringFunc(targetNode.Target);
        }

        return -host.Velocity;
    }

    public static Vector2 FollowPathCircular(
        SteeringHost host, 
        FollowingPath path, 
        ref int currentNode,
        Func<Vector2, Vector2> steeringFunc
        )
    {
        var targetNode = path[currentNode % path.NodeCount];

        if (targetNode != null)
        {
            /*var distance = (SteeringEntity.Position - targetNode.Target).Length();

            if (distance <= targetNode.TargetRadius)*/
            if (IsWithinTarget(host, targetNode))
            {
                currentNode++;
                targetNode = path[currentNode % path.NodeCount];
            }

            return steeringFunc(targetNode.Target);
        }

        return -host.Velocity;
    }
    
    
    private static bool IsWithinTarget(SteeringHost host, FollowingPathNode target)
    {
        var distance = (host.Entity.Position - target.Target).Length();
        return distance <= target.TargetRadius;
    }
}