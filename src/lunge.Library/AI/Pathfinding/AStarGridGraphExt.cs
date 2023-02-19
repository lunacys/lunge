using System;
using System.Collections.Generic;
using lunge.Library.Tiles;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;

namespace lunge.Library.AI.Pathfinding;

public class AStarGridGraphExt : IAstarGraph<Point>
{
    private static readonly Point[] CardinalDirs = {
        new Point(1, 0),
        new Point(0, -1),
        new Point(-1, 0),
        new Point(0, 1)
    };

    private static readonly Point[] CompassDirs = {
        new Point(1, 0),
        new Point(1, -1),
        new Point(0, -1),
        new Point(-1, -1),
        new Point(-1, 0),
        new Point(-1, 1),
        new Point(0, 1),
        new Point(1, 1),
    };

    private static readonly HashSet<Point> DiagonalDirs = new HashSet<Point>(){
        new Point(1, -1),
        new Point(-1, -1),
        new Point(-1, 1),
        new Point(1, 1),
    };
    
    public static readonly int CardinalCost = 10;
    public static readonly int DiagonalCost = 14;
    
    public HashSet<Point> Walls = new HashSet<Point>();
    public HashSet<Point> WeightedNodes = new HashSet<Point>();

    private Point[] _directions;
    private bool _usingDiagonals;
    private int _width;
    private int _height;
    
    private List<Point> _neighbors = new List<Point>(8);

    public readonly int[,] GridWeights;
        
    public AStarGridGraphExt(int width, int height, bool useDiagonal = false)
    {
        _width = width;
        _height = height;

        _directions = useDiagonal ? CompassDirs : CardinalDirs;
        _usingDiagonals = useDiagonal;

        GridWeights = new int[_height, _width];
    }

    public AStarGridGraphExt(TileLayer layer, bool useDiagonal = false)
        : this(layer.Width, layer.Height)
    {
        _width = layer.Width;
        _height = layer.Height;

        _directions = useDiagonal ? CompassDirs : CardinalDirs;
        _usingDiagonals = useDiagonal;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (layer.GetTileAt(x, y) != null)
                    Walls.Add(new Point(x, y));
            }
        }
    }
    
    public List<Point>? Search(Point start, Point goal) => AStarPathfinder.Search(this, start, goal);
    
    public IEnumerable<Point> GetNeighbors(Point node)
    {
        _neighbors.Clear();

        if (_usingDiagonals)
        {
            foreach (var dir in _directions)
            {
                var next = new Point(node.X + dir.X, node.Y + dir.Y);

                if (!IsNodeInBounds(next) || !IsNodePassable(next))
                    continue;

                if (IsDiagonalCorrect(dir, node, next)) // TODO: Check this
                    _neighbors.Add(next);
            }
        }
        else
        {
            foreach (var dir in _directions)
            {
                var next = new Point(node.X + dir.X, node.Y + dir.Y);
                if (IsNodeInBounds(next) && IsNodePassable(next))
                    _neighbors.Add(next);
            }
        }

        return _neighbors;
    }

    public int Cost(Point from, Point to)
    {
        // TODO: Fix costs
        if (IsDiagonal(from, to))
        {
            return DiagonalCost; //WeightedNodes.Contains(to) ? WeightedNodeWeight : DefaultWeight;
        }

        return CardinalCost; //_gridWeights[to.Y, to.X];
        //var kv = WeightedNodeCostable.FirstOrDefault(kv => kv.Node == to);

        //return kv?.Weight ?? DefaultWeight;

        // return WeightedNodeCostable.Any(kv => kv.Key == to) ? WeightedNodeWeight : DefaultWeight;
    }

    public int Heuristic(Point node, Point goal)
    {
        if (_usingDiagonals)
        {
            var dx = Math.Abs(node.X - goal.X);
            var dy = Math.Abs(node.Y - goal.Y);
            var res = (CardinalCost * (dx + dy) + (DiagonalCost - 2 * CardinalCost) * Math.Min(dx, dy));
            return (int)res;
        }

        return Math.Abs(node.X - goal.X) + Math.Abs(node.Y - goal.Y);
    }
    
    public Point[]? SearchPostSmooth(Point start, Point goal)
    {
        var res = AStarPathfinder.Search(this, start, goal);
        if (res == null)
            return Array.Empty<Point>();
        return PostSmooth(res.ToArray());
    }
    
    public Point[] PostSmooth(Point[]? baseNodes)
    {
        if (baseNodes == null)
            return Array.Empty<Point>();

        var start = baseNodes[0];
        var end = baseNodes[^1];
        
        var newNodes = new List<Point>();
        newNodes.Add(start);

        var oldDir = Point.Zero;
        
        for (int i = 0; i < baseNodes.Length - 1; i++)
        {
            var curr = baseNodes[i];
            var next = baseNodes[i + 1];

            var dir = next - curr;
            if (oldDir != dir)
            {
                newNodes.Add(curr);
                oldDir = dir;
            }
        }
        
        newNodes.Add(end);
        return newNodes.ToArray();
    }

    public Point[] PostSmoothLinecast(Point[]? baseNodes, Vector2 cellSize)
    {
        if (baseNodes == null)
            return Array.Empty<Point>();

        var nodes = new List<Point>();
        
        var start = baseNodes[0];
        var end = baseNodes[^1];
        var startNode = start;
        var prevNode = startNode;

        nodes.Add(prevNode);

        foreach (var node in baseNodes)
        {
            // TODO: May be wrong
            var hit = Physics.Linecast(startNode.ToVector2() * cellSize, node.ToVector2() * cellSize);
            
            if (hit.Collider != null)
            {
                startNode = prevNode;
                nodes.Add(prevNode);
            }

            prevNode = node;
        }

        nodes.Add(end);

        return nodes.ToArray();   
    }
    
    private bool IsNodeInBounds(Point node)
        => 0 <= node.X && node.X < _width && 0 <= node.Y && node.Y < _height;

    private bool IsNodePassable(Point node) => !Walls.Contains(node);
    
    private bool IsDiagonal(Point from, Point to)
    {
        var delta = new Point((to.X - from.X), (to.Y - from.Y));
        return DiagonalDirs.Contains(delta);
    }

    private bool IsDiagonal(Point direction)
    {
        return DiagonalDirs.Contains(direction);
    }
    
    private bool IsDiagonalCorrect(Point dir, Point node, Point next)
    {
        if (!IsDiagonal(dir))
            return true;
        // TODO: Maybe use dir instead of diagonal dir? Check it 
        foreach (var diagonalDir in DiagonalDirs)
        {
            if (!IsNodePassable(node + new Point(0, diagonalDir.Y)) ||
                !IsNodePassable(node + new Point(diagonalDir.X, 0)))
                return false;
        }

        return true;
    }
}