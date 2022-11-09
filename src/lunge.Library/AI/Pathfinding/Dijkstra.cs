using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding
{
    public class Dijkstra : IPathfinder<Point>
    {
        public string Alias => "Dijkstra's Algorithm";
        public Point Start { get; set; }
        public Point End { get; set; }
        public IEnumerable<Point> Nodes { get; private set; }
        public CoordinatesType CoordinatesType => CoordinatesType.Tile;

        private TmxMap _tiledMap;
        private WeightedGridGraph _weightedGridGraph;

        public Dijkstra(TmxMap map)
        {
            _tiledMap = map;
        }

        public void Initialize()
        {
            _weightedGridGraph = new WeightedGridGraph(_tiledMap.Width, _tiledMap.Height);

            var layer = _tiledMap.GetLayer<TmxLayer>("collision");

            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var tile = layer.GetTile(x, y);
                    if (tile != null)
                    {
                        _weightedGridGraph.Walls.Add(new Point(x, y));
                    }
                }
            }
        }

        public void Find()
        {
            Nodes = _weightedGridGraph.Search(Start, End);
        }

        public void Render(Batcher batcher)
        {
            if (Nodes != null)
            {
                foreach (var node in Nodes)
                {
                    batcher.DrawPixel(node.X * 32 + 14, node.Y * 32 + 14, Color.Blue, 4);
                }
            }
        }
    }
}