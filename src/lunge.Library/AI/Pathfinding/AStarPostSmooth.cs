using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding
{
    public class AStarPostSmooth : IPathfinder<Vector2>
    {
        public string Alias => "A* Post Smooth";
        public Vector2 Start { get; set; } = new Vector2(64, 64);
        public Vector2 End { get; set; } = new Vector2(128, 128);

        public CoordinatesType CoordinatesType => CoordinatesType.Absolute;

        public IEnumerable<Vector2> Nodes => _nodes;
        private List<Vector2> _nodes = new List<Vector2>();
        private List<Point> _bezierPoints = new List<Point>();

        private AStarGridGraph2 _astarGridGraph;

        private TmxMap _tiledMap;

        public static readonly Point[] Dirs = new[]
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

        public AStarPostSmooth(TmxMap map)
        {
            _tiledMap = map;
        }

        public void Initialize()
        {
            _astarGridGraph = new AStarGridGraph2(_tiledMap.Width, _tiledMap.Height, true);

            var layer = _tiledMap.GetLayer<TmxLayer>("collision");

            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var tile = layer.GetTile(x, y);
                    if (tile != null)
                    {
                        var p = new Point(x, y);
                        _astarGridGraph.Walls.Add(p);
                    }
                }
            }
        }

        public void Find()
        {
            var startPoint = _tiledMap.WorldToTilePosition(Start);
            var endPoint = _tiledMap.WorldToTilePosition(End);

            var baseNodes = _astarGridGraph.Search(startPoint, endPoint);
            
            PostSmooth(baseNodes);

            //_bezierPoints = computeCurvePoints(128, baseNodes.Select(v2 => new Point(v2.X * 32 + 16, v2.Y * 32 + 16)).ToArray());
        }

        public void Render(Batcher batcher)
        {
            for (int i = 0; i < _nodes.Count - 1; i++)
            {
                var p1 = _nodes[i];
                var p2 = _nodes[i + 1];

                DrawArrow(batcher, p1, p2, Color.White);
            }

            foreach (var node in _nodes)
            {
                batcher.DrawPixel(node.X, node.Y, Color.Brown, 4);
            }

            //for (int i = 0; i < _bezierPoints.Count - 1; i++)
            //{
                //batcher.DrawLine(_bezierPoints[i].ToVector2(), _bezierPoints[i + 1].ToVector2(), Color.Brown);
            //}
        }

        private void DrawArrow(Batcher batcher, Vector2 start, Vector2 end, Color color)
        {
            var angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            var firstQ = Quaternion.CreateFromYawPitchRoll(0, 0, angle);
            var offset1 = Vector2.Transform(new Vector2(-6, -6), firstQ);
            var offset2 = Vector2.Transform(new Vector2(-6, 6), firstQ);

            batcher.DrawLine(start, end, color);
            batcher.DrawLine(end, end + offset1, color);
            batcher.DrawLine(end, end + offset2, color);
        }

        /*private void CalculateEdges()
        {
            var layer = _tiledMap.GetLayer<TmxLayer>("collision");
            var tW = _tiledMap.TileWidth;
            var tH = _tiledMap.TileHeight;

            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var tile = layer.GetTile(x, y);
                    if (tile != null)
                    {
                        //var rect = new Rectangle(x * 32, y * 32, 32, 32);
                        var baseVec = new Vector2(x * tW, y * tH);
                        var topRight = new Vector2(baseVec.X + tW, baseVec.Y);
                        var bottomRight = new Vector2(baseVec.X + tW, baseVec.Y + tH);
                        var bottomLeft = new Vector2(baseVec.X, baseVec.Y + tH);

                        _edges.Add(baseVec);
                        _edges.Add(topRight);
                        _edges.Add(bottomRight);
                        _edges.Add(bottomLeft);
                    }
                }
            }
        }*/

        private void PostSmooth(IEnumerable<Point> baseNodes)
        {
            _nodes.Clear();
            if (baseNodes == null)
                return;

            var startNode = Start;
            Vector2 prevNode = startNode;

            _nodes.Add(prevNode);

            foreach (var node in baseNodes)
            {
                var pos = new Vector2(node.X * 32 + 16, node.Y * 32 + 16);

                var hit = Physics.Linecast(startNode, pos);
                if (hit.Collider != null)
                {
                    startNode = prevNode;
                    _nodes.Add(prevNode);
                }

                prevNode = pos;
            }

            var lastNode = _nodes.Last();
            _nodes.Add(End);
        }

        //This is what I call to get all points between which to draw.
        public static List<Point> computeCurvePoints(int steps, Point[] points)
        {
            List<Point> curvePoints = new List<Point>();
            for (float x = 0; x < 1; x += 1 / (float)steps)
            {
                curvePoints.Add(getBezierPointRecursive(x, points));
            }
            return curvePoints;
        }

        //Calculates a point on the bezier curve based on the timeStep.
        private static Point getBezierPointRecursive(float timeStep, Point[] ps)
        {
            if (ps.Length > 2)
            {
                List<Point> newPoints = new List<Point>();
                for (int x = 0; x < ps.Length - 1; x++)
                {
                    newPoints.Add(interpolatedPoint(ps[x], ps[x + 1], timeStep));
                }
                return getBezierPointRecursive(timeStep, newPoints.ToArray());
            }
            else
            {
                return interpolatedPoint(ps[0], ps[1], timeStep);
            }
        }

        //Gets the linearly interpolated point at t between two given points (without manual rounding).
        //Bad results!
        private static Point interpolatedPoint(Point p1, Point p2, float t)
        {
            Vector2 roundedVector = (Vector2.Multiply(p2.ToVector2() - p1.ToVector2(), t) + p1.ToVector2());
            return new Point((int)Math.Round(roundedVector.X), (int)Math.Round(roundedVector.Y));
        }
    }
}