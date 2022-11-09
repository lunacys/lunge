using System.Collections.Generic;

namespace lunge.Library.AI.Pathfinding.FlowFields.Old
{
    public interface IFlowFieldGraph<T>
    {
        IEnumerable<T> GetNeighbors(T node);
        int Cost(T from, T to);
        int Heuristic(T from, T to);
        void SetIntegrationCostAt(T pos, int cost);
        int GetIntegrationCostAt(T pos);
    }
}