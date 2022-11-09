using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding.FlowFields
{
    public class FlowFieldGraph
    {
        public Dictionary<Point, FlowFieldNode> FlowField { get; }

        private TmxLayer _tiledLayer;

        public FlowFieldGraph(int width, int height)
        {
            FlowField = new Dictionary<Point, FlowFieldNode>(width * height);
        }

        public FlowFieldGraph(TmxLayer tiledLayer)
        {
            _tiledLayer = tiledLayer;
        }


    }
}