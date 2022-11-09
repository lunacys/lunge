using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;
using Nez.ImGuiTools.ObjectInspectors;
using Nez.Tiled;

namespace Playground2.Components.TestsTiledPathfinding
{
    public class Pathfinder : RenderableComponent, IUpdatable
    {
        public override float Width => 1000;
        public override float Height => 1000;

        UnweightedGridGraph _gridGraph;
        List<Point> _breadthSearchPath;

        WeightedGridGraph _weightedGraph;
        List<Point> _weightedSearchPath;

        private AstarGridGraph _astarGraph;
        private List<Point> _astarSearchPath;

        private TmxMap _tilemap;
        private Point _start, _end;
        private Vector2 _position;

        private Path _path;

        public Vector2 Velocity { get; set; }
        protected Vector2 Steering;
        protected Vector2 DesiredVelocity;

        private List<Vector2> _excludedNodes = new List<Vector2>();
        private List<Point> _nonTransformedPath = new List<Point>();

        public float MaxVelocity { get; set; } = 2.0f;
        public float MaxForce { get; set; } = 3.8f;
        public float Mass { get; set; } = 1.0f;

        public Pathfinder(TmxMap tilemap)
        {
            _tilemap = tilemap;
            var layer = tilemap.GetLayer<TmxLayer>("main");

            _start = new Point(5, 5);
            _end = new Point(10, 10);

            _position = new Vector2(
                _start.X * _tilemap.TileWidth + _tilemap.TileWidth * 0.5f,
                _start.Y * _tilemap.TileHeight + _tilemap.TileHeight * 0.5f
                );

            _gridGraph = new UnweightedGridGraph(layer);
            _breadthSearchPath = _gridGraph.Search(_start, _end);

            _weightedGraph = new WeightedGridGraph(layer);
            _weightedSearchPath = _weightedGraph.Search(_start, _end);

            _astarGraph = new AstarGridGraph(layer);
            _astarSearchPath = _astarGraph.Search(_start, _end);

            _path = new Path();
            RebuildPath();
            OptimizePath();

            Debug.DrawTextFromBottom = true;
        }

        private void RebuildPath()
        {
            _path.Clear();
            _excludedNodes.Clear();
            _nonTransformedPath.Clear();

            var res = new List<Vector2>();

            if (_astarSearchPath != null)
            {
                if (_astarSearchPath.Count > 0)
                {
                    var directions = GetNodeDirections();
                    for (int i = 0; i < directions.Count - 1; i++)
                    {
                        if (directions[i] != directions[i + 1])
                        {
                            res.Add(TileToScreen(_astarSearchPath[i + 1]));
                            _nonTransformedPath.Add(_astarSearchPath[i + 1]);
                        }
                        else
                        {
                            _excludedNodes.Add(TileToScreen(_astarSearchPath[i + 1]));
                        }
                    }

                    res.Add(TileToScreen(_astarSearchPath.LastItem()));


                }
            }
        }

        public Vector2 Seek(Vector2 target)
        {
            var tmp = target - _position;
            tmp.Normalize();
            DesiredVelocity = tmp * MaxVelocity;

            return DesiredVelocity - Velocity;
        }

        private Vector2 TileToScreen(Point tile, bool origin = true)
        {
            return new Vector2(
                tile.X * _tilemap.TileWidth + _tilemap.TileWidth * (origin ? 0.5f : 0),
                tile.Y * _tilemap.TileHeight + _tilemap.TileHeight * (origin ? 0.5f : 0)
            );
        }

        private Point ScreenToTile(Vector2 position)
        {
            return new Point(
                (int)(position.X / _tilemap.TileWidth - _tilemap.TileWidth * 0.5),
                (int)(position.Y / _tilemap.TileWidth - _tilemap.TileWidth * 0.5)
            );
        }

        private List<Vector2> GetNodeDirections()
        {
            var list = new List<Vector2>();

            for (int i = 0; i < _astarSearchPath.Count - 1; i++)
            {
                var node = _astarSearchPath[i];
                var nextNode = _astarSearchPath[i + 1];

                var dir = nextNode - node;
                list.Add(dir.ToVector2());
            }

            return list;
        }

        private void OptimizePath()
        {

        }

        private void FollowPath()
        {
            var targetNode = _path.GetTargetNode();

            if (targetNode != null)
            {
                var target = targetNode.Position;

                Steering = Arrival(target, targetNode.Radius);

                var distance = (target - _position).Length();

                if (distance <= targetNode.Radius * 1)
                    _path.RemoveTargetNode();
            }
            else
            {
                Velocity = Vector2.Zero;
            }
        }

        private Vector2 Arrival(Vector2 target, float slowingRadius = 50f)
        {
            DesiredVelocity = target - _position;
            var distance = DesiredVelocity.Length();

            var dvNorm = DesiredVelocity;
            dvNorm.Normalize();

            if (distance < slowingRadius)
                DesiredVelocity = dvNorm * MaxVelocity * (distance / slowingRadius);
            else
                DesiredVelocity = dvNorm * MaxVelocity;

            return DesiredVelocity - Velocity;
        }

        void IUpdatable.Update()
        {
            // on left click set our path end time
            if (Input.LeftMouseButtonPressed)
                _end = _tilemap.WorldToTilePosition(Input.MousePosition);

            // on right click set our path start time
            if (Input.RightMouseButtonPressed)
                _start = _tilemap.WorldToTilePosition(Input.MousePosition);

            // regenerate the path on either click
            if (Input.LeftMouseButtonPressed || Input.RightMouseButtonPressed)
            {
                var first = Debug.TimeAction(() => { _breadthSearchPath = _gridGraph.Search(_start, _end); });

                var second = Debug.TimeAction(() => { _weightedSearchPath = _weightedGraph.Search(_start, _end); });

                var third = Debug.TimeAction(() =>
                {
                    _astarSearchPath = _astarGraph.Search(_start, _end);
                    RebuildPath();
                    OptimizePath();
                });

                // debug draw the times
                Debug.DrawText("Breadth First: {0}\nDijkstra: {0}\nAstar: {0}", third);
                Debug.Log("\nBreadth First: {0}\nDijkstra: {0}\nAstar: {0}", third);
            }

            FollowPath();

            Steering = MathUtils.Truncate(Steering, MaxForce);
            Steering /= Mass;

            Velocity += Steering;
            Velocity = MathUtils.Truncate(Velocity, MaxVelocity);
            _position += Velocity;

            var oldStart = _start;

            //_start = new Point((int)_position.X / _tilemap.TileWidth, (int)_position.Y / _tilemap.TileHeight);

            //if (oldStart != _start)
            //{
            //    _astarSearchPath = _astarGraph.Search(_start, _end);
            //}

            // Rotation = MathUtil.TurnToFace(Position, Position + Velocity, Rotation, 0.1f);
            //Position = Vector2.Clamp(Position,
            //    Vector2.Zero + Vector2.One * 80,
            //    new Vector2(800 - 16 - 64, 600 - 16 - 64));
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            batcher.DrawCircle(_position.X, _position.Y, 6, Color.Red);

            if (_astarSearchPath != null)
            {
                foreach (var node in _astarSearchPath)
                {
                    var x = node.X * _tilemap.TileWidth + _tilemap.TileWidth * 0.5f;
                    var y = node.Y * _tilemap.TileHeight + _tilemap.TileHeight * 0.5f;

                    batcher.DrawPixel(x, y, Color.Orange, 3);
                }
            }

            if (_breadthSearchPath != null)
            {
                foreach (var node in _breadthSearchPath)
                {
                    var x = node.X * _tilemap.TileWidth + _tilemap.TileWidth * 0.5f;
                    var y = node.Y * _tilemap.TileHeight + _tilemap.TileHeight * 0.5f;

                    batcher.DrawPixel(x + 2, y + 2, Color.Red, 3);
                }
            }

            if (_weightedSearchPath != null)
            {
                foreach (var node in _weightedSearchPath)
                {
                    var x = node.X * _tilemap.TileWidth + _tilemap.TileWidth * 0.5f;
                    var y = node.Y * _tilemap.TileHeight + _tilemap.TileHeight * 0.5f;

                    batcher.DrawPixel(x - 2, y - 2, Color.Green, 3);
                }
            }

            foreach (var node in _path)
            {
                batcher.DrawCircle(node.Position, 4, Color.Green, 1, 24);
            }

            for (int i = 0; i < _path.Count(); i++)
            {
                var node = _path[i];
                batcher.DrawCircle(node.Position, 4, Color.Green, 1, 24);
            }

            foreach (var node in _excludedNodes)
            {
                batcher.DrawCircle(node, 4, Color.Red, 1, 24);
            }

            //batcher.DrawLine(_start.ToVector2() * _tilemap.TileWidth, _end.ToVector2() * _tilemap.TileWidth, Color.Green, 2);
        }

        private int _privateInt;

        [InspectorDelegate]
        public void TestImGuiMethod()
        {
            ImGui.TextColored(new System.Numerics.Vector4(0, 1, 0, 1), "Colored text, private int is: " + _privateInt);
            ImGui.Combo("Combo", ref _privateInt, "First\0Second\0Third");
        }

        public void Line(Batcher batcher, int x, int y, int x2, int y2, Color color)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                var tts = TileToScreen(new Point(x, y));
                batcher.DrawPixel(tts, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
    }
}