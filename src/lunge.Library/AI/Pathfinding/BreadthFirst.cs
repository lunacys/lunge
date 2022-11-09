using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding
{
    public class BreadthFirst : IPathfinder<Point>
    {
        public string Alias => "Breadth First";
        public Point Start { get; set; }
        public Point End { get; set; }
        public IEnumerable<Point> Nodes { get; private set; }
        public CoordinatesType CoordinatesType => CoordinatesType.Tile;

        private TmxMap _tiledMap;
        private UnweightedGridGraph _breadthFirstGraph;

        public BreadthFirst(TmxMap map)
        {
            _tiledMap = map;
        }

        public void Initialize()
        {
            _breadthFirstGraph = new UnweightedGridGraph(_tiledMap.Width, _tiledMap.Height);

            var layer = _tiledMap.GetLayer<TmxLayer>("collision");

            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var tile = layer.GetTile(x, y);
                    if (tile != null)
                    {
                        _breadthFirstGraph.Walls.Add(new Point(x, y));
                    }
                }
            }
        }

        public void Find()
        {
            Nodes = _breadthFirstGraph.Search(Start, End);
        }

        public void Render(Batcher batcher)
        {
            if (Nodes != null)
            {
                foreach (var node in Nodes)
                {
                    batcher.DrawPixel(node.X * 32 + 18, node.Y * 32 + 18, Color.Yellow, 4);
                }
            }
        }
    }
}