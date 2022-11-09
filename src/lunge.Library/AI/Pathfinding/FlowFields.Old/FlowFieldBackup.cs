using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding.FlowFields.Old
{
    public class FlowFieldNodeBackup : PriorityQueueNode
    {
        public Point Position { get; set; }
        public Vector2 Velocity { get; set; }

        public FlowFieldNodeBackup(Point position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }

    public class FlowFieldBackup : IPathfinder<Point>
    {
        public string Alias => "Flow Field";
        public Point Start { get; set; } = new Point(2, 2);
        public Point End { get; set; } = new Point(2, 2);

        public CoordinatesType CoordinatesType => CoordinatesType.Tile;
        public IEnumerable<Point> Nodes => _nodes;
        private List<Point> _nodes = new List<Point>();

        public const int MinCost = 0;
        public const int MaxCost = int.MaxValue;
        public const int Impassable = 255;
        public const int DefaultCost = 1;

        public int[,] CostField;
        public int[,] IntegrationField;
        public FlowFieldNodeBackup[,] Field;
        public HashSet<Point> Walls;

        private TmxMap _tiledMap;

        private IFont _fontToUse;

        public bool ShowIntegrationField = false;
        public bool ShowCostField = false;
        public bool ShowFlowField = true;

        public static Point[] Dirs = new Point[]
        {
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1)
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

        public FlowFieldBackup(TmxMap map)
        {
            _tiledMap = map;
            CostField = new int[map.Height, map.Width];
            IntegrationField = new int[map.Height, map.Width];
            Walls = new HashSet<Point>();
            Field = new FlowFieldNodeBackup[map.Height, map.Width];
            Start = new Point(5, 5);
        }

        public void Initialize()
        {
            _fontToUse = Nez.Graphics.Instance.BitmapFont;

            // var layer = _tiledMap.GetLayer<TmxLayer>("collision");
            
            ResetField();
            CalculateIntegrationField(End, out var cameFrom);
            Console.WriteLine();
        }

        public void Find()
        {
            // throw new System.NotImplementedException();
            CalculateIntegrationField(End, out var cameFrom);
         
            foreach (var point in cameFrom)
            {
                var from = point.Key;
                var to = point.Value;
                if (from != End)
                {
                    var velo = to.ToVector2() - from.ToVector2();
                    velo.Normalize();
                    Field[from.Y, from.X] = new FlowFieldNodeBackup(from, velo);
                }
            }

            /*var path = new List<Point>();
            var current = End;
            _nodes.Add(End);

            while (!current.Equals(Start))
            {
                current = cameFrom[current];
                _nodes.Add(current);
            }*/

            //CalculateFlowField();
        }

        public void Render(Batcher batcher)
        {
            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    if (ShowCostField)
                    {
                        var cost = CostField[y, x];
                        var pos = new Vector2(x * 32 + 8, y * 32 + 10);
                        if (cost == MaxCost)
                            batcher.DrawString(_fontToUse, "Inf", pos, Color.Red);
                        else
                            batcher.DrawString(_fontToUse, cost.ToString(), pos, Color.White);
                    }

                    if (ShowIntegrationField)
                    {
                        var val = IntegrationField[y, x];
                        var pos = new Vector2(x * 32 + 8, y * 32 + 10);
                        if (val == MaxCost)
                            batcher.DrawString(_fontToUse, "Inf", pos, Color.Red);
                        else
                            batcher.DrawString(_fontToUse, val.ToString(), pos, Color.White);
                    }

                    if (ShowFlowField)
                    {
                        var field = Field[y, x];
                        if (field.Velocity != Vector2.Zero)
                        {
                            var pos2 = new Vector2(field.Position.X * 32 + 16, field.Position.Y * 32 + 16);
                            batcher.DrawPixel(pos2, Color.Black, 4);
                            batcher.DrawLine(pos2, pos2 + field.Velocity * 10, Color.Black);
                        }
                    }

                    foreach (var node in Nodes)
                    {
                        batcher.DrawPixel(node.X * 32 + 16, node.Y * 32 + 16, Color.Green, 8);
                    }
                }
            }
        }

        private void ResetField()
        {
            var layer = _tiledMap.GetLayer<TmxLayer>("collision");
            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    if (layer.GetTile(x, y) != null)
                        CostField[y, x] = Impassable;
                    else
                        CostField[y, x] = DefaultCost;

                    IntegrationField[y, x] = MaxCost;  //new FlowFieldNodeBackup(new Point(x, y), MaxCost);
                    Field[y, x] = new FlowFieldNodeBackup(new Point(x, y), Vector2.Zero);
                }
            }
        }

        private void CalculateIntegrationField(Point target, out Dictionary<Point, Point> cameFrom)
        {
            cameFrom = new Dictionary<Point, Point>();
            cameFrom.Add(target, target);

            var costSoFar = new Dictionary<Point, int>();
            var openList = new PriorityQueue<FlowFieldNodeBackup>(1024); //List<Point>();
            openList.Enqueue(new FlowFieldNodeBackup(target, Vector2.Zero), 0);

            costSoFar[target] = 0;
            SetValueAt(target, 0);

            while (openList.Count > 0)
            {
                var curr = openList.Dequeue();

                foreach (var neighbor in GetNeighbors(curr.Position))
                {
                    var endNodeCost = costSoFar[curr.Position] + //IntegrationField[curr.Position.Y, curr.Position.X] +
                                      CostField[neighbor.Y, neighbor.X];
                    if (!costSoFar.ContainsKey(neighbor) || endNodeCost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = endNodeCost;
                        var priority = endNodeCost + Heuristic(neighbor, target);
                        openList.Enqueue(new FlowFieldNodeBackup(neighbor, Vector2.Zero), priority);
                        cameFrom[neighbor] = curr.Position;
                        SetValueAt(neighbor, endNodeCost);
                    }
                }
            }
        }

        private int Heuristic(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }

        private void CalculateFlowField()
        {
            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var integrVal = IntegrationField[y, x];
                    if (integrVal < MaxCost)
                    {
                        var neighbors = GetIntegrationNeighbors(new Point(x, y));

                        foreach (var tuple in neighbors)
                        {
                            if (tuple.Item2 < integrVal)
                            {
                                var newVelo = tuple.Item1.ToVector2() - new Vector2(x, y);
                                newVelo.Normalize();
                                Field[y, x].Velocity = newVelo;
                            }
                        }
                    }
                }
            }
        }

        private void SetValueAt(Point pos, int cost)
        {
            IntegrationField[pos.Y, pos.X] = cost;
        }

        private int GetValue(Point pos)
        {
            return IntegrationField[pos.Y, pos.X];
        }

        public List<Point> GetNeighbors(Point pos)
        {
            var resultList = new List<Point>();
            
            foreach (var dir in FlowFieldDirs)
            {
                var newPos = new Point(pos.X + dir.X, pos.Y + dir.Y);
                if (newPos.X > 0 && newPos.Y > 0 && newPos.X < _tiledMap.Width && newPos.Y < _tiledMap.Height)
                {
                    if (CostField[newPos.Y, newPos.X] < Impassable)
                        resultList.Add(newPos);
                }
            }

            return resultList;
        }

        private List<(Point, int)> GetIntegrationNeighbors(Point pos)
        {
            var resultList = new List<(Point, int)>();

            foreach (var dir in FlowFieldDirs)
            {
                var newPos = new Point(pos.X + dir.X, pos.Y + dir.Y);
                if (newPos.X > 0 && newPos.Y > 0 && newPos.X < _tiledMap.Width && newPos.Y < _tiledMap.Height)
                {
                    if (CostField[newPos.Y, newPos.X] < MaxCost)
                        resultList.Add((newPos, IntegrationField[newPos.Y, newPos.X]));
                }
            }

            return resultList;
        }
    }
}