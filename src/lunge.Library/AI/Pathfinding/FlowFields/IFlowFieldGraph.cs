using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Pathfinding.FlowFields
{
    public interface IFlowFieldGraph<T>
    {
        IEnumerable<T> GetNeighbors(T start);
        IEnumerable<T> GetFlowNeighbors(T start);
        int Cost(T target);
        Vector2 GetVelocityFor(T neighbor, T current);
    }
}