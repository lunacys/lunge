using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using lunge.Library.AI.Pathfinding;
using lunge.Library.AI.Pathfinding.FlowFields.Old;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tiled;
using Random = Nez.Random;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Playground2.Components.Pathfinding2
{
    public class PathfinderComponent : RenderableComponent, IUpdatable
    {
        public override float Width => 10000;
        public override float Height => 10000;


        private Vector2? _startPos, _endPos;

        private RectangleF? _selectRect;

        private Vector2? _wayPoint;

        private TmxMap _tiledMap;

        private Point _pathStart = new Point(3, 3);
        private Point _pathEnd = new Point(32, 32);

        private bool _showThetaStar = false;
        private bool _showAStar = true;
        private bool _showBreadthFirst = false;
        private bool _showDijkstras = false;
        private bool _showFlowFieldBackup = false;
        private bool _showFlowFieldOld = false;
        private bool _showFlowFieldNew = false;
        private bool _showThetaStar2 = false;
        private bool _showAStarPostSmooth = true;

        //private AStar _aStar;
        private BreadthFirst _breadthFirst;
        private Dijkstra _dijkstra;
        private FlowFieldBackup _flowFieldBackup;
        private FlowFieldOld _flowFieldOld;
        private FlowFieldNew _flowFieldNew;
        //private AStarPostSmooth _aStarPostSmooth;

        private Entity _flowFieldEntity;

        private List<IPathfinder> _pathfinders = new List<IPathfinder>();

        private Dictionary<string, List<TimeSpan>> _times = new Dictionary<string, List<TimeSpan>>();

        private QuadTree<Vector2> _quadTree;

        public PathfinderComponent(TmxMap tiledMap, FlowFieldOld flowField)
        {
            _tiledMap = tiledMap;
            _flowFieldOld = flowField;

            Debug.DrawTextFromBottom = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            //_thetaStar = new ThetaStar(_tiledMap);
            //_aStar = new AStar(_tiledMap);
            _breadthFirst = new BreadthFirst(_tiledMap);
            _dijkstra = new Dijkstra(_tiledMap);
            _flowFieldBackup = new FlowFieldBackup(_tiledMap);
            _flowFieldOld = new FlowFieldOld(_tiledMap);
            _flowFieldNew = new FlowFieldNew(_tiledMap);
            //_thetaStar2 = new ThetaStar2(_tiledMap);
            //_aStarPostSmooth = new AStarPostSmooth(_tiledMap);

            //_pathfinders.Add(_aStar);
            _pathfinders.Add(_breadthFirst);
            _pathfinders.Add(_dijkstra);
            _pathfinders.Add(_flowFieldBackup);
            _pathfinders.Add(_flowFieldOld);
            _pathfinders.Add(_flowFieldNew);
            //_pathfinders.Add(_thetaStar);
            //_pathfinders.Add(_thetaStar2);
            //_pathfinders.Add(_aStarPostSmooth);

            foreach (var pathfinder in _pathfinders)
            {
                Debug.Log("Initializing " + pathfinder.Alias);
                var initTime = Debug.TimeAction(() => pathfinder.Initialize());
                Debug.Log($"Done Initializing {pathfinder.Alias}, finding path (Time spent: {initTime})");
                //var findTime = Debug.TimeAction(() => pathfinder.Find());
                //Debug.Log($"Done finding path for {pathfinder.Alias}, next one.. (Time spent: {findTime})");
            }

            _quadTree = new QuadTree<Vector2>(new AABB(Vector2.Zero, Width, Height), 1, 10);
        }

        public void Update()
        {
            if (Input.LeftMouseButtonPressed && !Input.IsKeyDown(Keys.LeftShift))
            {
                _startPos = Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
                var entities = Core.Scene.Entities.FindComponentsOfType<UnitComponent>();
                foreach (var entity in entities)
                {
                    entity.IsSelected = false;
                }
                ListPool<UnitComponent>.Free(entities);
            }
            else if (Input.LeftMouseButtonPressed && Input.IsKeyDown(Keys.LeftShift))
            {
                var pathEnd = Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
                var pathEndTile = _tiledMap.WorldToTilePosition(pathEnd);

                foreach (var pathfinder in _pathfinders)
                {
                    if (pathfinder is IPathfinder<Point> asPoint)
                        asPoint.End = pathEndTile;
                    else if (pathfinder is IPathfinder<Vector2> asVector)
                        asVector.End = pathEnd;

                    var time = Debug.TimeAction(() => pathfinder.Find());
                    Debug.Log($" -> {pathfinder.Alias}: {time:g}");

                    if (_times.ContainsKey(pathfinder.Alias))
                    {
                        _times[pathfinder.Alias].Add(time);
                    }
                    else
                    {
                        _times[pathfinder.Alias] = new List<TimeSpan> { time };
                    }
                }

                ShowAverageTimes();
            }

            if (Input.LeftMouseButtonDown && _startPos.HasValue)
            {
                _endPos = Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
                var start = _startPos.Value;
                var end = _endPos.Value;

                GetSelectRect(start, end, out _selectRect);
            }

            if (Input.LeftMouseButtonReleased)
            {
                /*if (_selectRect.HasValue)
                {
                    var colliders = Physics.BoxcastBroadphase(_selectRect.Value);
                    foreach (var collider in colliders)
                    {
                        var unit = collider.Entity.GetComponent<UnitComponent>();
                        if (unit != null)
                            unit.IsSelected = true;
                    }
                }*/

                _startPos = null;
                _endPos = null;
                _selectRect = null;
            }

            if (_selectRect.HasValue)
            {
                var units = Core.Scene.Entities.FindComponentsOfType<UnitComponent>();
                foreach (var unit in units)
                {
                    if (unit.Bounds.Intersects(_selectRect.Value))
                        unit.IsSelected = true;
                    else
                        unit.IsSelected = false;
                }
                ListPool<UnitComponent>.Free(units);
            }

            if (Input.IsKeyPressed(Keys.Space))
                Debug.DrawText("Space pressed", Color.Red, 10f, 2f);

            if (Input.RightMouseButtonPressed && !Input.IsKeyDown(Keys.LeftShift))
            {
                _wayPoint = Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);

                // TODO: Check if this method is faster than storing list of selected entities locally (Debug.TimeAction() method)
                var units = Core.Scene.Entities.FindComponentsOfType<UnitComponent>();
                foreach (var unit in units)
                {
                    unit.WorldToTileFunc = vector2 => _tiledMap.WorldToTilePosition(vector2);
                    if (unit.IsSelected)
                        unit.MoveTo(_wayPoint.Value);
                    unit.MoveAlongPath(_flowFieldOld);
                }
                ListPool<UnitComponent>.Free(units);
            }
            else if (Input.RightMouseButtonPressed && Input.IsKeyDown(Keys.LeftShift))
            {
                var pathStart = Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
                var pathStartTile = _tiledMap.WorldToTilePosition(pathStart);

                foreach (var pathfinder in _pathfinders)
                {
                    if (pathfinder is IPathfinder<Point> asPoint)
                        asPoint.Start = pathStartTile;
                    else if (pathfinder is IPathfinder<Vector2> asVector)
                        asVector.Start = pathStart;

                    pathfinder.Find();
                }
            }

            if (Input.LeftMouseButtonDown)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 rnd = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    rnd += Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
                    _quadTree.AddNode(new Element<Vector2>(rnd, new AABB(rnd, new Vector2(32, 32))));
                }
            }
        }

        private void ShowAverageTimes()
        {
            foreach (var time in _times)
            {
                Debug.Log($"(#{time.Value.Count}) Average for '{time.Key}': {time.Value.Average(span => span.TotalMilliseconds):F5} ms");
            }
        }

        private void ImGuiDebug()
        {
            ImGui.Begin("Options");

            ImGui.Checkbox("Show Theta*", ref _showThetaStar);
            ImGui.Checkbox("Show A*", ref _showAStar);
            ImGui.Checkbox("Show Breadth First", ref _showBreadthFirst);
            ImGui.Checkbox("Show Dijkstra's", ref _showDijkstras);
            ImGui.Checkbox("Show Flow Field Old", ref _showFlowFieldOld);
            ImGui.Checkbox("Show Flow Field Backup", ref _showFlowFieldBackup);
            ImGui.Checkbox("Show Flow Field New", ref _showFlowFieldNew);
            ImGui.Checkbox("Show Theta* 2", ref _showThetaStar2);
            ImGui.Checkbox("Show A* Post Smooth", ref _showAStarPostSmooth);

            ImGui.Separator();

            ImGui.Text("Active Pathfinders:");

            foreach (var pathfinder in _pathfinders)
            {
                ImGui.TextColored(new System.Numerics.Vector4(1, 0, 0, 1), $" -> {pathfinder.GetType().Name}");
                ImGui.SameLine();
                if (ImGui.Button("Delete"))
                {
                    // _pathfinders.Remove(pathfinder);
                }
            }

            /*ImGui.Separator();
            ImGui.Text("Flow Field Values:");
            ImGui.Checkbox("Show Cost Field", ref _flowFieldBackup.ShowCostField);
            ImGui.Checkbox("Show Integration Field", ref _flowFieldBackup.ShowIntegrationField);
            ImGui.Checkbox("Show Flow Field", ref _flowFieldBackup.ShowFlowField);

            ImGui.Separator();
            ImGui.Checkbox("Show Flow Field NEW", ref _showFlowFieldNew);

            ImGui.End();*/
        }

        private void GetSelectRect(Vector2 startPos, Vector2 endPos, out RectangleF? rect)
        {
            if (endPos.X > startPos.X && endPos.Y > startPos.Y)
                rect = new RectangleF(startPos.X, startPos.Y, endPos.X - startPos.X, endPos.Y - startPos.Y);
            else if (endPos.X > startPos.X && endPos.Y < startPos.Y)
                rect = new RectangleF(startPos.X, endPos.Y, endPos.X - startPos.X, startPos.Y - endPos.Y);
            else if (endPos.X < startPos.X && endPos.Y > startPos.Y)
                rect = new RectangleF(endPos.X, startPos.Y, startPos.X - endPos.X, endPos.Y - startPos.Y);
            else if (endPos.X < startPos.X && endPos.Y < startPos.Y)
                rect = new RectangleF(endPos.X, endPos.Y, startPos.X - endPos.X, startPos.Y - endPos.Y);
            else
                rect = null;
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            ImGuiDebug();

            if (_startPos != null && _endPos != null && _selectRect != null)
            {
                batcher.DrawHollowRect(_selectRect.Value, Color.Black);
            }

            
            if (_showAStar)
            {
                //_aStar?.Render(batcher);
            }
            if (_showBreadthFirst)
            {
                _breadthFirst?.Render(batcher);
            }
            if (_showDijkstras)
            {
                _dijkstra?.Render(batcher);
            }
            if (_showFlowFieldBackup)
            {
                _flowFieldBackup?.Render(batcher);
            }
            if (_showFlowFieldOld)
            {
                _flowFieldOld?.Render(batcher);
            }
            if (_showFlowFieldNew)
            {
                _flowFieldNew?.Render(batcher);
            }
            if (_showAStarPostSmooth)
            {
                //_aStarPostSmooth?.Render(batcher);
            }
        }

        public override void DebugRender(Batcher batcher)
        {
            /*foreach (var navRectangle in _navRectangles)
            {
                batcher.DrawRect(navRectangle, Color.Green * 0.3f);
                batcher.DrawHollowRect(navRectangle, Color.Black, 3f);
            }*/

            batcher.DrawRect(new Rectangle(0, 0, 1280, 128), Color.Green * 0.3f);
        }
    }
}