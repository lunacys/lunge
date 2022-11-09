using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Pathfinding.FlowFields.Old
{
    public class FlowFieldNode<T> : PriorityQueueNode
    {
        public T Position { get; set; }
        public Vector2 Velocity { get; set; }

        public FlowFieldNode(T position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }

    public static class FlowFieldPathfinder
    {
        public static void Search<T>(IFlowFieldGraph<T> graph, T start, T target, out Dictionary<T, T> cameFrom)
        {
            cameFrom = new Dictionary<T, T>();
            cameFrom.Add(target, target);

            var costSoFar = new Dictionary<T, int>();
            var openList = new PriorityQueue<FlowFieldNode<T>>(1024); 
            openList.Enqueue(new FlowFieldNode<T>(target, Vector2.Zero), 0);

            costSoFar[target] = 0;
            graph.SetIntegrationCostAt(target, 0);

            while (openList.Count > 0)
            {
                var curr = openList.Dequeue();

                foreach (var neighbor in graph.GetNeighbors(curr.Position))
                {
                    var endNodeCost = costSoFar[curr.Position] + graph.Cost(curr.Position, neighbor);

                    if (!costSoFar.ContainsKey(neighbor) || endNodeCost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = endNodeCost;
                        var priority = endNodeCost + graph.Heuristic(neighbor, target);
                        openList.Enqueue(new FlowFieldNode<T>(neighbor, Vector2.Zero), priority);
                        cameFrom[neighbor] = curr.Position;
                        graph.SetIntegrationCostAt(neighbor, endNodeCost);
                    }
                }
            }
        }
    }
}