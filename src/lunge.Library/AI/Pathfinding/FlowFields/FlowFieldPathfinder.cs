using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lunge.Library.AI.Pathfinding.FlowFields
{
    public static class FlowFieldPathfinder
    {
        public static Dictionary<T, FlowFieldNode> Search<T>(IFlowFieldGraph<T> graph, T target)
        {
            Dictionary<T, FlowFieldNode> nodes = new Dictionary<T, FlowFieldNode>();

            var openList = new Queue<T>(1024);
            openList.Enqueue(target);

            nodes.Add(target, new FlowFieldNode(0, Vector2.Zero));

            while (openList.Count > 0)
            {
                var curr = openList.Dequeue();

                foreach (var neighbor in graph.GetNeighbors(curr))
                {
                    var endNodeCost = nodes[curr].Value + graph.Cost(neighbor);

                    //If a shorter path has been found, add the node into the open list
                    if (endNodeCost < nodes[curr].Value)
                    {
                        //Check if the neighbor cell is already in the list.
                        //If it is not then add it to the end of the list.
                        if (!openList.Contains(neighbor))
                        {
                            openList.Enqueue(neighbor);
                        }

                        nodes[neighbor] = new FlowFieldNode(endNodeCost, Vector2.Zero);
                    }
                }

                var integrVal = nodes[curr].Value;

                foreach (var neighbor in graph.GetFlowNeighbors(curr))
                {
                    // У всех соседей меняем велосити, чтобы он указывал на таргет (curr)
                    // если стоимость таргета меньше стоимости соседя
                    // curr - neighbor
                    var neighborCost = nodes[neighbor].Value;

                    if (neighborCost < integrVal)
                    {
                        var velocity = graph.GetVelocityFor(neighbor, curr);
                        nodes[curr] = new FlowFieldNode(nodes[curr].Value, velocity);
                        integrVal = neighborCost;
                    }
                }
            }

            return nodes;
        }
    }
}