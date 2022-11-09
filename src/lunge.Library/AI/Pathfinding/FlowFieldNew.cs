using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using FlowFieldGraph = lunge.Library.AI.Pathfinding.FlowFields.Old.FlowFieldGraph;

namespace lunge.Library.AI.Pathfinding
{
    public class FlowFieldNew : IPathfinder<Point>
    {
        public string Alias => "Flow Field New";
        public Point Start { get; set; }
        public Point End { get; set; }
        public IEnumerable<Point> Nodes { get; }
        public CoordinatesType CoordinatesType => CoordinatesType.Tile;


        private FlowFieldGraph _flowFieldGraph;
        private TmxMap _tiledMap;

        public FlowFieldNew(TmxMap map)
        {
            _tiledMap = map;
        }

        public void Initialize()
        {
            _flowFieldGraph = new FlowFieldGraph(_tiledMap.GetLayer<TmxLayer>("collision"));
        }

        public void Find()
        {
            _flowFieldGraph.Search(Start, End);
        }

        public void Render(Batcher batcher)
        {
            foreach (var node in _flowFieldGraph.FlowField)
            {
                if (node.Velocity != Vector2.Zero)
                {
                    var pos = new Vector2(node.Position.X * 32 + 16, node.Position.Y * 32 + 16);

                    batcher.DrawLine(pos, pos + node.Velocity * 10, Color.Black);
                    batcher.DrawPixel(pos, Color.Black, 4);
                }
            }

            /*for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var cost = _flowFieldGraph.GetIntegrationCostAt(new Point(x, y));
                    
                    var pos = new Vector2(x * 32 + 8, y * 32 + 10);
                    if (cost == int.MaxValue)
                        batcher.DrawString(Graphics.Instance.BitmapFont, "Inf", pos, Color.Red);
                    else
                        batcher.DrawString(Graphics.Instance.BitmapFont, cost.ToString(), pos, Color.White);
                }
            }*/
        }
    }
}