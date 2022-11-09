using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace lunge.Library.AI.Pathfinding
{
    public class AStar : IPathfinder<Point>
    {
        public string Alias => "A*";
        public Point Start { get; set; } = new Point(2, 2);
        public Point End { get; set; } = new Point(5, 5);

        public CoordinatesType CoordinatesType => CoordinatesType.Tile;

        public IEnumerable<Point> Nodes => _nodes;
        private List<Point> _nodes;

        private AStarGridGraph2 _astarGridGraph;

        private TmxMap _tiledMap;

        private List<Vector2> _edges = new List<Vector2>();

        private AABB _testAabb;

        private List<Point> _curvePoints;

        public AStar(TmxMap map)
        {
            _tiledMap = map;
            _testAabb = new AABB(new Vector2(64, 64), new Vector2(86, 128));
            _curvePoints = computeCurvePoints(128,
                new Point[]
                {
                    new Point(64, 64), 
                    new Point(86, 128), 
                    new Point(128, 256),
                    new Point(256, 64)
                });
        }

        public void Initialize()
        {
            _astarGridGraph = new AStarGridGraph2(_tiledMap.Width, _tiledMap.Height, true);

            var layer = _tiledMap.GetLayer<TmxLayer>("collision");

            for (int y = 0; y < _tiledMap.Height; y++)
            {
                for (int x = 0; x < _tiledMap.Width; x++)
                {
                    var tile = layer.GetTile(x , y);
                    if (tile != null)
                    {
                        var p = new Point(x, y);
                        _astarGridGraph.Walls.Add(p);
                    }
                }
            }

            CalculateEdges();
        }

        public void Find()
        {
            _nodes = _astarGridGraph.Search(new Point(Start.X, Start.Y), new Point(End.X, End.Y));
        }

        public void Render(Batcher batcher)
        {
            if (Nodes != null)
            {
                for (int i = 0; i < _nodes.Count - 1; i++)
                {
                    var p1 = _nodes[i];
                    var p2 = _nodes[i + 1];
                    var startPos = new Vector2(p1.X * 32 + 16, p1.Y * 32 + 16);
                    var endPos = new Vector2(p2.X * 32 + 16, p2.Y * 32 + 16);

                    DrawArrow(batcher, startPos, endPos, Color.White);
                }

                foreach (var node in _nodes)
                {
                    batcher.DrawPixel(node.X * 32 + 16, node.Y * 32 + 16, Color.Red, 4);
                }
            }

            for (int i = 0; i < _curvePoints.Count - 1; i++)
            {
                batcher.DrawLine(_curvePoints[i].ToVector2(), _curvePoints[i + 1].ToVector2(), Color.Black, 2);
            }

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

        private void CalculateEdges()
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