using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding
{
    public enum FlowFieldDirection
    {
        None,
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    public class FlowFieldNode
    {
        public int Value;
        public Vector2 Direction;

        public FlowFieldNode(int value, Vector2 direction)
        {
            Value = value;
            Direction = direction;
        }

        public static FlowFieldDirection PointToEnum(Point direction)
        {
            if (direction == new Point(0, -1))
                return FlowFieldDirection.Up;
            if (direction == new Point(1, -1))
                return FlowFieldDirection.UpRight;
            if (direction == new Point(1, 0))
                return FlowFieldDirection.Right;
            if (direction == new Point(1, 1))
                return FlowFieldDirection.DownRight;
            if (direction == new Point(0, 1))
                return FlowFieldDirection.Down;
            if (direction == new Point(-1, 1))
                return FlowFieldDirection.DownLeft;
            if (direction == new Point(-1, 0))
                return FlowFieldDirection.Left;
            if (direction == new Point(-1, -1))
                return FlowFieldDirection.UpLeft;

            return FlowFieldDirection.None;
        }

        public static Point EnumToPoint(FlowFieldDirection direction)
        {
            switch (direction)
            {
                case FlowFieldDirection.None: 
                    return Point.Zero;
                case FlowFieldDirection.Up:
                    return new Point(0, -1);
                case FlowFieldDirection.UpRight:
                    return new Point(1, -1);
                case FlowFieldDirection.Right:
                    return new Point(1, 0);
                case FlowFieldDirection.DownRight:
                    return new Point(1, 1);
                case FlowFieldDirection.Down:
                    return new Point(0, 1);
                case FlowFieldDirection.DownLeft:
                    return new Point(-1, 1);
                case FlowFieldDirection.Left:
                    return new Point(-1, 0);
                case FlowFieldDirection.UpLeft:
                    return new Point(-1, -1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }

    public class FlowFieldPathNode
    {
        public Point Position { get; set; }
        public FlowFieldNode Node { get; set; }

        public FlowFieldPathNode(Point position, FlowFieldNode node)
        {
            Position = position;
            Node = node;
        }
    }

    public class FlowFieldOld : Component, IPathfinder<Point>
    {
        public string Alias => "Flow Field Old";
        public Point Start { get; set; } = new Point(2, 2);
        public Point End { get; set; } = new Point(2, 2);

        public CoordinatesType CoordinatesType => CoordinatesType.Tile;
        public IEnumerable<Point> Nodes => _nodes;
        private List<Point> _nodes = new List<Point>();
        private List<FlowFieldPathNode> _pathNodes = new List<FlowFieldPathNode>();

        public const int MaxCost = int.MaxValue;
        public const int Impassable = 255;
        
        public Dictionary<Point, FlowFieldNode> _flowFieldNodes;

        public HashSet<Point> Walls;
        public HashSet<Point> WeightedNodes;

        private TmxMap _tiledMap;

        private IFont _fontToUse = null!;

        public bool ShowIntegrationField = true;
        public bool ShowCostField = false;
        public bool ShowFlowField = true;

        public static readonly Point[] Dirs = new Point[]
        {
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1)
        };

        public static readonly Point[] FlowFieldDirs = new[]
        {
            new Point(1, 0),
            new Point(1, -1),
            new Point(0, -1),
            new Point(-1, -1),
            new Point(-1, 0),
            new Point(-1, 1),
            new Point(0, 1),
            new Point(1, 1),
        };

        static readonly HashSet<Point> DiagonalDirs = new HashSet<Point>(){
            new Point(1, -1),
            new Point(-1, -1),
            new Point(-1, 1),
            new Point(1, 1),
        };

        public FlowFieldOld(TmxMap map)
        {
            _tiledMap = map;
            Walls = new HashSet<Point>();
            WeightedNodes = new HashSet<Point>();
            _flowFieldNodes = new Dictionary<Point, FlowFieldNode>(map.Width * map.Height * 2);
            Start = new Point(5, 5);
        }

        public override void Initialize()
        {
            _fontToUse = Nez.Graphics.Instance.BitmapFont;
            ResetField();
        }

        public void Find()
        {
            CalculateIntegrationField(new Point(End.X, End.Y));
        }

        public void Render(Batcher batcher)
        {
            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var pos = new Point(x, y);
                    var drawPos = new Vector2(x * 32 + 8, y * 32 + 10);
                    if (_flowFieldNodes.ContainsKey(pos))
                    {
                        var field = _flowFieldNodes[pos];

                        if (ShowFlowField)
                        {
                            if (field.Direction != Vector2.Zero)
                            {
                                var pos2 = new Vector2(x * 32 + 16, y * 32 + 16);
                                batcher.DrawPixel(pos2, Color.Black, 4);
                                batcher.DrawLine(pos2, pos2 + field.Direction * 10, Color.Black);
                            }
                        }

                        if (ShowIntegrationField)
                        {
                            batcher.DrawString(_fontToUse, field.Value.ToString(), drawPos, Color.White);
                        }
                    }
                    else
                    {
                        if (ShowFlowField)
                        {

                        }

                        if (ShowIntegrationField)
                        {
                            batcher.DrawString(_fontToUse, "Inf", drawPos, Color.White);
                        }
                    }

                    
                }
            }

            foreach (var node in _pathNodes)
            {
                batcher.DrawPixel(node.Position.X * 32 + 16, node.Position.Y * 32 + 16, Color.Red, 5);
            }

            foreach (var node in _nodes)
            {
                batcher.DrawPixel(node.X * 32 + 16, node.Y * 32 + 16, Color.Red, 5);
            }
        }

        private void ResetField()
        {
            var layer = _tiledMap.GetLayer<TmxLayer>("collision");
            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var tile = layer.GetTile((int) (x ) , (int) (y));
                    if (tile != null)
                        Walls.Add(new Point(x, y));
                }
            }
            _flowFieldNodes.Clear();
        }

        private void ResetIntegrationField()
        {
            _flowFieldNodes.Clear();
        }

        private void CalculateIntegrationField(Point target)
        {
            ResetIntegrationField();

            var openList = new Queue<Point>(1024);
            openList.Enqueue(target);

            SetValueAt(target, 0);

            var maxCount = 1;
            bool pathFound = false;

            var shortestPath = new List<FlowFieldPathNode>();


            while (openList.Count > 0)
            {
                var curr = openList.Dequeue();

                foreach (var neighbor in GetNeighbors(curr))
                {
                    var endNodeCost = GetValue(curr) + GetCostAt(neighbor);

                    if (neighbor == Start)
                    {
                        pathFound = true;
                    }

                    //If a shorter path has been found, add the node into the open list
                    var curVal = GetValue(neighbor);
                    if (endNodeCost < curVal)
                    {
                        //Check if the neighbor cell is already in the list.
                        //If it is not then add it to the end of the list.
                        if (!openList.Contains(neighbor))
                        {
                            openList.Enqueue(neighbor);
                            if (openList.Count > maxCount)
                                maxCount = openList.Count;
                        }

                        SetValueAt(neighbor, endNodeCost);
                    }
                }

                var integrVal = GetValue(curr);

                foreach (var neighbor in GetIntegrationNeighbors(curr))
                {
                    var neighborCost = GetValue(neighbor);

                    // float r = 1.0f / (float) Math.Sqrt(neighbor.X * neighbor.X + neighbor.Y + neighbor.Y);
                    var start = new Vector2(curr.X * 32 + 16, curr.Y * 32 + 16);
                    var end = new Vector2(target.X * 32 + 16, target.Y * 32 + 16);
                    bool straight = false;

                    //var hit = Physics.Linecast(start, end);
                    //straight = hit.Collider == null;

                    var targetPos = straight ? target : neighbor;

                    if (neighborCost < integrVal && _flowFieldNodes.ContainsKey(curr))
                    {
                        var direction = targetPos - curr;
                        if (IsDiagonalCorrect(direction, curr))
                        {
                            var velocity = (targetPos.ToVector2() - curr.ToVector2());
                            velocity.Normalize();
                            _flowFieldNodes[curr].Direction = velocity;
                            integrVal = neighborCost;
                        }
                    }
                }
            }

            if (pathFound && !IsWall(target))
            {
                var curPos = Start;
                var maxIterations = 100;
                var curIteration = 0;

                while (curPos != target && curIteration <= maxIterations)
                {
                    var node = _flowFieldNodes[curPos];
                    shortestPath.Add(new FlowFieldPathNode(curPos, node));

                    var veloX = (int) Math.Round(node.Direction.X);
                    var veloY = (int) Math.Round(node.Direction.Y);

                    var x = curPos.X + veloX;
                    var y = curPos.Y + veloY;
                    var next = new Point(x, y);
                    curPos = next;

                    curIteration++;
                }

                shortestPath.Add(new FlowFieldPathNode(target, _flowFieldNodes[target]));

                var a = shortestPath.Select(node => node.Position);
                _nodes = PostSmooth(a); //shortestPath;
            }

            Debug.Log($"Found Path from {Start.X},{Start.Y} to {End.X}, {End.Y}: {pathFound}");
        }

        private List<Point> PostSmooth(IEnumerable<Point> baseNodes)
        {
            var nodes = new List<Point>();
            if (baseNodes == null)
                return nodes;

            var startNode = Start;
            Point prevNode = startNode;

            nodes.Add(prevNode);

            foreach (var node in baseNodes)
            {
                var pos = new Vector2(node.X * 32 + 16, node.Y * 32 + 16);

                var hit = Physics.Linecast(new Vector2(startNode.X * 32 + 16, startNode.Y * 32 + 16), pos);
                if (hit.Collider != null)
                {
                    startNode = prevNode;
                    nodes.Add(prevNode);
                }

                prevNode = node;
            }

            nodes.Add(End);
            return nodes;
        }

        bool IsDiagonalCorrect(Point dir, Point node)
        {
            if (!IsDiagonal(dir))
                return true;

            foreach (var diagonalDir in DiagonalDirs)
            {
                if (IsWall(node + new Point(0, diagonalDir.Y)) ||
                    IsWall(node + new Point(diagonalDir.X, 0)))
                    return false;
            }

            return true;
        }

        bool IsDiagonal(Point from, Point to)
        {
            var delta = new Point((to.X - from.X), (to.Y - from.Y));
            return DiagonalDirs.Contains(delta);
        }

        bool IsDiagonal(Point direction)
        {
            return DiagonalDirs.Contains(direction);
        }

        public void RecalculateFlowField()
        {
            /*for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var pos = new Point(x, y);
                    if (Walls.Contains(pos))
                        continue;
                    if (pos == End)
                        _flowFieldNodes[pos].Direction = Vector2.Zero;;

                    var integrVal = GetValue(pos);
                    var neighbors = GetIntegrationNeighbors(new Point(x, y));

                    foreach (var neighbor in neighbors)
                    {
                        var neighborCost = GetValue(neighbor);

                        if (neighborCost < integrVal)
                        {
                            var velocity = neighbor - pos;
                            _flowFieldNodes[pos].Direction = velocity;
                            integrVal = neighborCost;
                        }
                    }
                }
            }*/
        }

        private int GetCostAt(Point pos)
        {
            return Walls.Contains(pos) ? Impassable : 1;
        }

        private void SetValueAt(Point pos, int cost)
        {
            if (Walls.Contains(pos))
                return;

            if (_flowFieldNodes.ContainsKey(pos))
                _flowFieldNodes[pos].Value = cost;
            else
                _flowFieldNodes[pos] = new FlowFieldNode(cost, Vector2.Zero);
        }

        private int GetValue(Point pos)
        {
            if (Walls.Contains(pos))
                return MaxCost;
            return _flowFieldNodes.ContainsKey(pos) ? _flowFieldNodes[pos].Value : MaxCost;
        }

        public List<Point> GetNeighbors(Point pos)
        {
            var resultList = new List<Point>();
            
            foreach (var dir in Dirs)
            {
                var newPos = new Point(pos.X + dir.X, pos.Y + dir.Y);
                if (IsInBounds(newPos) && !IsWall(newPos))
                {
                     resultList.Add(newPos);
                }
            }

            return resultList;
        }

        public bool IsWall(Point pos) => Walls.Contains(pos);

        public bool IsInBounds(Point pos) =>
            pos.X > 0 && pos.Y > 0 && pos.X < _tiledMap.Width && pos.Y < _tiledMap.Height;

        private List<Point> GetIntegrationNeighbors(Point pos)
        {
            var resultList = new List<Point>();

            foreach (var dir in FlowFieldDirs)
            {
                var newPos = new Point(pos.X + dir.X, pos.Y + dir.Y);
                if (IsInBounds(newPos) && !IsWall(newPos))
                {
                    resultList.Add(newPos);
                }
            }

            return resultList;
        }
    }
}