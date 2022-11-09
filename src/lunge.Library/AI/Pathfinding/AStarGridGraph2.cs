using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez.AI.Pathfinding;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding
{
    public class NodeCostable : IEquatable<NodeCostable>
    {
        public Point Node { get; set; }
		public int Weight { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Node, Weight);
        }

        public bool Equals(NodeCostable? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Node.Equals(other.Node) && Weight == other.Weight;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NodeCostable)obj);
        }
    }

    public class AStarGridGraph2 : IAstarGraph<Point>
    {
		public Point[] Dirs = new Point[]
		{
			new Point(1, 0),
			new Point(0, -1),
			new Point(-1, 0),
			new Point(0, 1),
			new Point(1, 1),
			new Point(-1, -1),
			new Point(-1, 1),
			new Point(1, -1)
		};

		public static readonly Point[] CARDINAL_DIRS = {
			new Point(1, 0),
			new Point(0, -1),
			new Point(-1, 0),
			new Point(0, 1)
		};

		static readonly Point[] COMPASS_DIRS = {
			new Point(1, 0),
			new Point(1, -1),
			new Point(0, -1),
			new Point(-1, -1),
			new Point(-1, 0),
			new Point(-1, 1),
			new Point(0, 1),
			new Point(1, 1),
		};

		static readonly HashSet<Point> DIAGONAL_DIRS = new HashSet<Point>(){
			new Point(1, -1),
			new Point(-1, -1),
			new Point(-1, 1),
			new Point(1, 1),
		};

		public HashSet<Point> Walls = new HashSet<Point>();
		public HashSet<Point> WeightedNodes = new HashSet<Point>();


        private int[,] _gridWeights;


        public HashSet<NodeCostable> WeightedNodeCostable
        {
			set {
                foreach (var node in value)
                {
                    _gridWeights[node.Node.Y, node.Node.X] = node.Weight;
                }
            }
        }
		public int DefaultWeight = 10;
		public int WeightedNodeWeight = 15;

		public static int CardinalCost => 10;
		public static int DiagonalCost => 14;

		int _width, _height;
		List<Point> _neighbors = new List<Point>(4);

		private bool _usingDiagonal;

		public AStarGridGraph2(int width, int height, bool useDiagonal = false)
		{
			_width = width;
			_height = height;

			Dirs = useDiagonal ? COMPASS_DIRS : CARDINAL_DIRS;
			_usingDiagonal = useDiagonal;

            _gridWeights = new int[_height, width];
        }

		/// <summary>
		/// creates a WeightedGridGraph from a TiledTileLayer. Present tile are walls and empty tiles are passable.
		/// </summary>
		/// <param name="tiledLayer">Tiled layer.</param>
		public AStarGridGraph2(TmxLayer tiledLayer, bool useDiagonal = false)
		{
			_width = tiledLayer.Width;
			_height = tiledLayer.Height;

            Dirs = useDiagonal ? COMPASS_DIRS : CARDINAL_DIRS;
            _usingDiagonal = useDiagonal;

			for (var y = 0; y < tiledLayer.Map.Height; y++)
			{
				for (var x = 0; x < tiledLayer.Map.Width; x++)
				{
					if (tiledLayer.GetTile(x, y) != null)
						Walls.Add(new Point(x, y));
				}
			}
		}

		/// <summary>
		/// ensures the node is in the bounds of the grid graph
		/// </summary>
		/// <returns><c>true</c>, if node in bounds was ised, <c>false</c> otherwise.</returns>
		bool IsNodeInBounds(Point node)
		{
			return 0 <= node.X && node.X < _width && 0 <= node.Y && node.Y < _height;
		}

		/// <summary>
		/// checks if the node is passable. Walls are impassable.
		/// </summary>
		/// <returns><c>true</c>, if node passable was ised, <c>false</c> otherwise.</returns>
		bool IsNodePassable(Point node) => !Walls.Contains(node);

		/// <summary>
		/// convenience shortcut for calling AStarPathfinder.search
		/// </summary>
		public List<Point>? Search(Point start, Point goal) => AStarPathfinder.Search(this, start, goal);

		#region IAstarGraph implementation

		IEnumerable<Point> IAstarGraph<Point>.GetNeighbors(Point node)
		{
			_neighbors.Clear();

			if (_usingDiagonal)
			{
				foreach (var dir in Dirs)
				{
					var next = new Point(node.X + dir.X, node.Y + dir.Y);

					if (!IsNodeInBounds(next) || !IsNodePassable(next))
						continue;

					if (IsDiagonalCorrect(dir, node, next))
						_neighbors.Add(next);
				}
			}
			else
			{
				foreach (var dir in Dirs)
				{
					var next = new Point(node.X + dir.X, node.Y + dir.Y);
					if (IsNodeInBounds(next) && IsNodePassable(next))
						_neighbors.Add(next);
				}
			}

			return _neighbors;
		}

		bool IsDiagonalCorrect(Point dir, Point node, Point next)
		{
			if (!IsDiagonal(dir))
				return true;

			foreach (var diagonalDir in DIAGONAL_DIRS)
			{
				if (!IsNodePassable(node + new Point(0, diagonalDir.Y)) ||
					!IsNodePassable(node + new Point(diagonalDir.X, 0)))
					return false;
			}

			return true;
		}


		int IAstarGraph<Point>.Cost(Point from, Point to)
		{
			/*if (LineOfSight(from.X, from.Y, to.X, to.Y))
			{
				return 1;
			}*/

			if (IsDiagonal(from, to))
			{
				return DiagonalCost; //WeightedNodes.Contains(to) ? WeightedNodeWeight : DefaultWeight;
			}

            return _gridWeights[to.Y, to.X];
            //var kv = WeightedNodeCostable.FirstOrDefault(kv => kv.Node == to);

            //return kv?.Weight ?? DefaultWeight;

            // return WeightedNodeCostable.Any(kv => kv.Key == to) ? WeightedNodeWeight : DefaultWeight;
        }

		int IAstarGraph<Point>.Heuristic(Point node, Point goal)
		{
			if (_usingDiagonal)
			{
				var dx = Math.Abs(node.X - goal.X);
				var dy = Math.Abs(node.Y - goal.Y);
				var res = (CardinalCost * (dx + dy) + (DiagonalCost - 2 * CardinalCost) * Math.Min(dx, dy));
				return (int)res;
			}

			return Math.Abs(node.X - goal.X) + Math.Abs(node.Y - goal.Y);
		}

		#endregion

		Point RelativePoint(Point from, Point to)
		{
			return new Point(to.X - from.X, to.Y - from.Y);
		}

		bool IsDiagonal(Point from, Point to)
		{
			var delta = new Point((to.X - from.X), (to.Y - from.Y));
			return DIAGONAL_DIRS.Contains(delta);
		}

		bool IsDiagonal(Point direction)
		{
			return DIAGONAL_DIRS.Contains(direction);
		}
	}
}
