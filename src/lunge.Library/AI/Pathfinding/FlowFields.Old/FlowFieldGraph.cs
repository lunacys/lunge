using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding.FlowFields.Old
{
    public class FlowFieldGraph : IFlowFieldGraph<Point>
    {
        public static Point[] CostFieldDirs = new[]
        {
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1),
        };

        public static Point[] FlowFieldDirs = new[]
        {
            //new Point(0, 0),
            new Point(1, 0),
            new Point(1, -1),
            new Point(0, -1),
            new Point(-1, -1),
            new Point(-1, 0),
            new Point(-1, 1),
            new Point(0, 1),
            new Point(1, 1),
        };

        public HashSet<Point> Walls = new HashSet<Point>();
        public HashSet<Point> WeightedNodes = new HashSet<Point>();
        public HashSet<FlowFieldNode<Point>> FlowField;
        public int DefaultWeight = 1;
        public int WeightedNodeWeight = 5;


        public int Width { get; }
        public int Height { get; }

        private List<Point> _neighbors = new List<Point>(8);
        private Dictionary<Point, Point> _cameFrom = new Dictionary<Point, Point>();

        public Dictionary<Point, int> Costs = new Dictionary<Point, int>();

        public FlowFieldGraph(int width, int height)
        {
            Width = width;
            Height = height;

            FlowField = new HashSet<FlowFieldNode<Point>>(Width * Height);
        }

        public FlowFieldGraph(TmxLayer layer)
        {
            Width = layer.Width;
            Height = layer.Height;

            FlowField = new HashSet<FlowFieldNode<Point>>(Width * Height);

            for (int y = 0; y < layer.Map.Height; y++)
            {
                for (int x = 0; x < layer.Map.Width; x++)
                {
                    if (layer.GetTile(x, y) != null)
                        Walls.Add(new Point(x, y));
                }
            }
        }

        public IEnumerable<Point> GetNeighbors(Point node)
        {
            _neighbors.Clear();

            foreach (var dir in FlowFieldDirs)
            {
                var newPos = new Point(node.X + dir.X, node.Y + dir.Y);
                if (IsInBounds(newPos) && !IsWall(newPos))
                {
                    _neighbors.Add(newPos);
                }
            }

            return _neighbors;
        }

        public int Cost(Point from, Point to)
        {
            if (IsWall(to))
                return int.MaxValue;

            return WeightedNodes.Contains(to) ? WeightedNodeWeight : DefaultWeight;
        }

        public int Heuristic(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }

        public void SetIntegrationCostAt(Point pos, int cost)
        {
            // Do not implement if integration cost of all nodes is not needed
            Costs[pos] = cost;
        }

        public int GetIntegrationCostAt(Point pos)
        {
            if (Costs.ContainsKey(pos))
                return Costs[pos];

            return int.MaxValue;
        }

        public HashSet<FlowFieldNode<Point>> Search(Point start, Point end)
        {
            // _cameFrom.Clear();
            Costs.Clear();
            FlowField.Clear();
            FlowFieldPathfinder.Search(this, start, end, out var cameFrom);
            //CalculateFlowField();
            foreach (var point in cameFrom)
            {
                var from = point.Key;
                var to = point.Value;
                if (from != end)
                {
                    var velo = to.ToVector2() - from.ToVector2();
                    velo.Normalize();
                    FlowField.Add(new FlowFieldNode<Point>(from, velo));
                    //Field[from.Y, from.X] = new FlowFieldNodeBackup(from, velo);
                }
            }

            return FlowField;
        }

        private void CalculateFlowField()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var p = new Point(x, y);
                    if (!Costs.ContainsKey(p)) 
                        continue;
                    
                    var integrVal = Costs[p];
                    if (integrVal < int.MaxValue)
                    {
                        var neighbors = GetIntegrationNeighbors(p);

                        foreach (var tuple in neighbors)
                        {
                            if (tuple.Item2 < integrVal)
                            {
                                var newVelo = tuple.Item1.ToVector2() - new Vector2(x, y);
                                newVelo.Normalize();
                                if (FlowField.FirstOrDefault(node => node.Position == p) == null)
                                    FlowField.Add(new FlowFieldNode<Point>(p, newVelo));
                            }
                        }
                    }
                }
            }
        }

        private List<(Point, int)> GetIntegrationNeighbors(Point pos)
        {
            var resultList = new List<(Point, int)>();

            foreach (var dir in FlowFieldDirs)
            {
                var newPos = new Point(pos.X + dir.X, pos.Y + dir.Y);
                if (IsInBounds(newPos))
                {
                    if (!IsWall(newPos))
                        resultList.Add((newPos, GetIntegrationCostAt(newPos)));
                }
            }

            return resultList;
        }

        private bool IsInBounds(Point p) => p.X > 0 && p.Y > 0 && p.X < Width && p.Y < Height;
        private bool IsWall(Point p) => Walls.Contains(p);

    }
}