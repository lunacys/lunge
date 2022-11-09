using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using lunge.Library.AI.Pathfinding;
using lunge.Library.Debugging.Logging;
using lunge.Library.Utils;
using lunge.Library.Utils.DelaunayAlgorithm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.AI.Pathfinding;
using Nez.ImGuiTools;
using Nez.Sprites;
using Edge = lunge.Library.Edge;
using Random = Nez.Random;

namespace Playground.Components.WorldGeneration;

public class WorldGeneratorComponent : Component, IUpdatable
{
    public class Circle
    {
        public Vector2 Center;
        public float Radius;
    }

    public List<RectangleF> RoomRects = new List<RectangleF>();

    public int WorldWidth => Settings.WorldWidth * GridSize;
    public int WorldHeight => Settings.WorldHeight * GridSize;

    public int WorldWidthTiles { get; private set; }
    public int WorldHeightTiles { get; private set; }

    public int MaxArea { get; }

    public int GridSize => 32;

    private readonly ILogger _logger = LoggerFactory.GetLogger<WorldGeneratorComponent>();

    public Delaunay2D TriangulationGraph { get; private set; }

    public List<Prim.EdgeD> Edges { get; private set; } = new();
    public List<Prim.EdgeD> AdditionalEdges { get; private set; } = new();
    public List<Vector2>? AStarPath { get; private set; } = new();
    public List<Circle> Circles { get; private set; } = new();

    private List<Point> _hallwayTilesList = new List<Point>();

    private AStarExitFinderGraph _aStarGraph;
    private AStarGridGraph2 _aStarHallwayGraph;

    public CellType[,] Grid { get; private set; }

    private object _lockObj = new object();

    private int _isCompleted = 1;
    private int _currentStage = 0;

    public int CurrentStage => _currentStage;

    public bool[,]? BitMap;
    public int[,]? BitMask;

    public WorldGeneratorSettings Settings { get; }

    private Collider[] _colliders;

    public WorldGeneratorComponent(int width, int height)
    {
        Settings = new WorldGeneratorSettings(async () =>
        {
            if (_isCompleted == 1)
                await Generate();
        });
        MaxArea = WorldWidth * WorldHeight;
    }

    public override async void OnAddedToEntity()
    {
        Core.GetGlobalManager<ImGuiManager>()?.RegisterDrawCommand(Settings.ImGuiDraw);
        await Generate();
    }

    public async Task<WorldData> Generate()
    {
        // Algorithm:
        // 1) Place rooms
        // 2) Create Delaunay Triangulation graph (points -> mesh) (e.g. Bowyer-Watson Algorithm)
        // 3) Find a minimum spanning tree (MST) (e.g. Prim's Algorithm)
        // 4) Randomly choose from the potential hallways (edges). Add some random edges from the 2nd step (12.5%)
        // 5) For every hallway, use the A* algorithm to find a path between the two rooms and to create the path itself
        // 6) 
        _logger.Debug($"Started generation!");
        Reset();
        
        await Task.Run(() =>
        {
            var stages = new Action[]
            {
                Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Stage7,
                Stage8
            };

            var elapsedList = new List<TimeSpan>();

            for (int i = 0; i < stages.Length; i++)
            {
                var stage = stages[i];

                Interlocked.Increment(ref _currentStage);

                lock(_lockObj)
                    _logger.Info($"Starting stage #{i + 1}: {ParseStage(_currentStage)}");

                var elapsed = Debug.TimeAction(stage);

                lock (_lockObj)
                {
                    elapsedList.Add(elapsed);
                    _logger.Debug($"Stage #{i + 1} completed in {elapsed.TotalMilliseconds} ms");
                }
            }
            

            lock (_lockObj)
            {
                var total = TimeSpan.Zero;
                foreach (var elapsed in elapsedList)
                {
                    total += elapsed;
                }
                _logger.Debug(
                    $"Total Time: {total.TotalMilliseconds} ms\n\n");
            }

            Interlocked.Exchange(ref _currentStage, 0);
            Interlocked.Exchange(ref _isCompleted, 1);
        });

        return new WorldData(WorldWidthTiles, WorldHeightTiles, RoomRects, BitMask);
    }

    public async void Update()
    {
        if (Input.IsKeyPressed(Keys.Space) && _isCompleted == 1)
        {
            await Generate();
        }

        if (_isCompleted == 1)
        {
            if (Input.LeftMouseButtonDown)
            {
                var pos = Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
                var posPoint = new Point((int)pos.X / GridSize, (int)pos.Y / GridSize);

                AddTiles(posPoint, new Point(3, 3), CellType.Room);
            }
            else if (Input.RightMouseButtonDown)
            {
                var pos = Core.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
                var posPoint = new Point((int)pos.X / GridSize, (int)pos.Y / GridSize);

                RemoveTiles(posPoint, new Point(1, 1));
            }
        }
    }

    private void Reset()
    {
        WorldWidthTiles = WorldWidth / GridSize;
        WorldHeightTiles = WorldHeight / GridSize;

        Grid = new CellType[WorldHeightTiles, WorldWidthTiles];
        _isCompleted = 0;

        Circles.Clear();
        RoomRects.Clear();
        Edges.Clear();
        AdditionalEdges.Clear();
        _hallwayTilesList.Clear();
        CollisionRects?.Clear();
        Grid = new CellType[WorldHeightTiles, WorldWidthTiles];

        if (_colliders != null)
        {
            foreach (var collider in _colliders)
            {
                Physics.RemoveCollider(collider);
            }
        }
    }

    private void Stage1()
    {
        // Generate simple room rectangles randomly
        int iteration = 0;
        
        while (!MinAreaConstraint())
        {
            int minW = Settings.InitialMinRoomWidth * GridSize;
            int maxW = Settings.InitialMaxRoomWidth * GridSize;
            int minH = Settings.InitialMinRoomHeight * GridSize;
            int maxH = Settings.InitialMaxRoomHeight * GridSize;

            // Try to fill with big rooms firstly
            if (iteration > Settings.NextStepRoomGenerationIterations)
            {
                minW = Settings.NextMinRoomWidth * GridSize;
                minH = Settings.NextMinRoomHeight * GridSize;

                maxW = Settings.NextMaxRoomWidth * GridSize;
                maxH = Settings.NextMaxRoomHeight * GridSize;
            }

            var randomRect = GenerateRect(minW, minH, maxW, maxH);

            var offset = Settings.RoomSpacing * GridSize;
            if (!IntersectsWithAny(randomRect, offset))
                RoomRects.Add(randomRect);

            iteration++;

            if (iteration > Settings.MaxRoomGenIterations)
            {
                _logger.Warn($"Could not create needed amount of rooms. Total Rooms: {RoomRects.Count}");
                break;
            }
        }
    }

    private void Stage2()
    {
        // Generate triangulation graph 
        /*TriangulationGraph = Delaunay2D.Triangulate(
            RoomRects.Select(r => r.Center).ToList()
        );*/
        TriangulationGraph = Delaunay2D.TriangulateConstraint(RoomRects);
    }

    private static bool IntersectsCircle(Vector2 vector, Circle circle)
    {
        return
            MathF.Pow(vector.X - circle.Center.X, 2) +
            MathF.Pow(vector.Y - circle.Center.Y, 2) < circle.Radius * circle.Radius;
    }

    private static bool Intersects(Circle circle, RectangleF rect)
    {
        var circleDistance = new Vector2(
            MathF.Abs(circle.Center.X - rect.Center.X),
            MathF.Abs(circle.Center.Y - rect.Center.Y)
        );

        if (circleDistance.X > (rect.Width / 2 + circle.Radius)) { return false; }
        if (circleDistance.Y > (rect.Height / 2 + circle.Radius)) { return false; }

        if (circleDistance.X <= (rect.Width / 2)) { return true; }
        if (circleDistance.Y <= (rect.Height / 2)) { return true; }

        var cornerDistance_sq = MathF.Pow(circleDistance.X - rect.Width / 2, 2) +
            MathF.Pow(circleDistance.Y - rect.Height / 2, 2);

        return (cornerDistance_sq <= (circle.Radius * circle.Radius));
    }

    private void Stage3()
    {
        // Find a minimum spanning tree
        var start = TriangulationGraph.Edges.First().U;
        Edges = Prim.MinimumSpanningTree(
            TriangulationGraph.Edges.Select(e => new Prim.EdgeD(e.U, e.V)).ToList(),
            start
        );

        // Add some random edges
        foreach (var edge in TriangulationGraph.Edges)
        {
            if (Random.Chance(Settings.RandomEdgeInclusionChance))
                AdditionalEdges.Add(new Prim.EdgeD(edge.U, edge.V));
        }
    }

    private void Stage4()
    {
        // fill rooms to grid
        for (int y = 0; y < WorldHeightTiles; y++)
        {
            for (int x = 0; x < WorldWidthTiles; x++)
            {
                var vec = new Vector2(x * GridSize, y * GridSize);

                if (RoomRects.Any(r => r.Contains(vec)))
                    Grid[y, x] = CellType.Room;
            }
        }

        // and spice up rectangles by adding and removing tiles
        /*foreach (var rect in RoomRects)
        {
            Circles.Add(new Circle
            {
                Center = rect.Center,
                Radius = MathF.Max(rect.Width, rect.Height) / 2f
            });
        }*/

        /*foreach (var rect in RoomRects)
        {
            var circle = new Circle
            {
                Center = rect.Center,
                Radius = rect.Width
            };
            for (int y = (int)rect.Y / GridSize; y < rect.Height / GridSize; y++)
            {
                for (int x = (int)rect.X / GridSize; x < rect.Width / GridSize; x++)
                {
                    var minRect = new RectangleF(x * GridSize, y * GridSize, GridSize, GridSize);

                    if (!Intersects(circle, minRect))
                        Grid[y, x] = CellType.None;
                    else
                        Grid[y, x] = CellType.Room;
                }
            }
        }*/
        
        /*for (int y = 0; y < WorldHeightTiles; y++)
        {
            for (int x = 0; x < WorldWidthTiles; x++)
            {
                var rect = new RectangleF(x * GridSize, y * GridSize, GridSize, GridSize);
                foreach (var circle in circles)
                {
                    if (Intersects(circle, rect))
                        Grid[y, x] = CellType.Room;
                    else
                        Grid[y, x] = CellType.None;
                }
            }
        }*/

        GustavsonNoise = new Color[WorldHeightTiles, WorldWidthTiles];
        CarmodyNoise = new Color[WorldHeightTiles, WorldWidthTiles];
        for (int y = 0; y < WorldHeightTiles; y++)
        {
            for (int x = 0; x < WorldWidthTiles; x++)
            {
                var gust = NoiseHelper.GustavsonNoise(x, y, false, true);
                var carm = NoiseHelper.CarmodyNoise(x, y, 0, true, true);

                //if (gust < 0.9f) gust = 0;
                //if (carm < 0.5f) carm = 0;

                GustavsonNoise[y, x] = Color.Red * gust;
                CarmodyNoise[y, x] = Color.White * carm;
            }
        }
    }

    public Color[,] GustavsonNoise;
    public Color[,] CarmodyNoise;

    private void Stage5()
    {
        // Find a path between the two rooms using A*

        var nodes = new List<Edge>(Edges);
        nodes.AddRange(AdditionalEdges);

        var startPosition = RoomRects.First().Center;
        var endPosition = RoomRects.Last().Center;

        _aStarGraph = new AStarExitFinderGraph(nodes);
        AStarPath = AStarPathfinder.Search(_aStarGraph, startPosition, endPosition);

        
        _aStarHallwayGraph = new AStarGridGraph2(WorldWidthTiles, WorldHeightTiles, false)
        {
            WeightedNodeWeight = 10,
            WeightedNodeCostable = ConvertToNodes()
        };

        foreach (var edge in Edges)
        {
            var path = _aStarHallwayGraph.Search(
                new Point((int)edge.U.X / GridSize, (int)edge.U.Y / GridSize),
                new Point((int)edge.V.X / GridSize, (int)edge.V.Y / GridSize)
            );

            if (path == null)
                continue;

            _hallwayTilesList.AddRange(path);

            foreach (var node in path)
            {/*
                var g = Grid[node.Y, node.X];
                if (g != CellType.Room)
                    Grid[node.Y, node.X] = CellType.Hallway;*/
                AddTiles(node, new Point(3, 3), CellType.Hallway, true);
            }

            _aStarHallwayGraph.WeightedNodeCostable = ConvertToNodes();
        }
    }

    private void Stage6()
    {
        // Generate walls around the rooms and hallways
        bool IsNotOutOfBounds(Point pos)
        {
            return pos.X >= 0 && pos.Y >= 0 && pos.X < WorldWidthTiles && pos.Y < WorldHeightTiles;
        }

        foreach (var rect in RoomRects)
        {
            var tileX = (int)rect.X / GridSize - 1; //(int)(rect.X - GridSize) / GridSize;
            var tileY = (int)rect.Y / GridSize - 1;
            var tileW = (int)rect.Width / GridSize + 2;
            var tileH = (int)rect.Height / GridSize + 2;

            for (int y = tileY; y < tileY + tileH; y++)
            {
                for (int x = tileX; x < tileX + tileW; x++)
                {
                    if (!IsNotOutOfBounds(new Point(x, y)))
                        continue;

                    var tile = Grid[y, x];

                    if (tile == CellType.None && 
                        (x == tileX || 
                         y == tileY ||
                         x == tileX + (tileW - 1)||
                         y == tileY + (tileH - 1)))
                        Grid[y, x] = CellType.Wall;
                }
            }
        }

        var dirs = new[]
        {
            new Point(-1, -1), new Point(0, -1), new Point(1, -1),
            new Point(-1,  0),                   new Point(1,  0),
            new Point(-1,  1), new Point(0,  1), new Point(1,  1),
        };

        foreach (var point in _hallwayTilesList)
        {
            foreach (var dir in dirs)
            {
                var p = point + dir;
                if (IsNotOutOfBounds(p) && Grid[p.Y, p.X] == CellType.None)
                    Grid[p.Y, p.X] = CellType.Wall;
            }
        }
    }

    private void Stage7()
    {
        // Generate bitmaps and bitmasks 
        BitMap = BitMaskHelper.CreateBitMapFrom(Grid, CellType.Wall);
        BitMask = BitMaskHelper.CalculateBitMaskForBitMap(BitMap, false);
    }

    private void Stage8()
    {
        // Create Collisions and Player
        CreateCollisions();
        CreatePlayer();
    }

    public void AddTiles(Point position, Point size, CellType cellType, bool includeWalls = true)
    {
        var w = Grid.GetLength(1);
        var h = Grid.GetLength(0);

        if (!BitMaskHelper.IsNotOutOfBounds(position, w, h))
            return;

        if (Grid[position.Y, position.X] == cellType)
            return;

        if (cellType == CellType.Wall)
        {
            Grid[position.Y, position.X] = CellType.Wall;
            BitMap[position.Y, position.X] = true;
        }
        else if (cellType == CellType.None)
        {
            RemoveTiles(position, size);
            BitMask = BitMaskHelper.CalculateBitMaskForBitMap(BitMap, false);
        }
        else
        {
            if (includeWalls)
            {
                var rect = new Rectangle(
                    position.X - 1,
                    position.Y - 1,
                    size.X + 1,
                    size.Y + 1
                );

                var startX = rect.X; //position.X - size.X;
                var startY = rect.Y; // position.Y - size.Y;
                var endX = rect.X + rect.Width; //position.X + size.X;
                var endY = rect.Y + rect.Height; // position.Y + size.Y;

                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        if (!BitMaskHelper.IsNotOutOfBounds(new Point(x, y), w, h))
                            continue;

                        if (x == startX || y == startY || x == endX || y == endY)
                        {
                            if (Grid[y, x] == CellType.None)
                            {
                                if (BitMap != null)
                                    BitMap[y, x] = true;
                                Grid[y, x] = CellType.Wall;
                            }
                        }
                        else
                        {
                            Grid[y, x] = cellType;
                        }
                    }
                }
            }
            else
            {
                var startX = position.X;
                var startY = position.Y;
                var endX = position.X + size.X;
                var endY = position.Y + size.Y;

                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        if (!BitMaskHelper.IsNotOutOfBounds(new Point(x, y), w, h))
                            continue;

                        Grid[y, x] = cellType;
                    }
                }
            }

            if (BitMap != null)
                BitMask = BitMaskHelper.CalculateBitMaskForBitMap(BitMap, false);
        }
    }

    public void RemoveTiles(Point position, Point size)
    {
        BitMap[position.Y, position.X] = false;
        Grid[position.Y, position.X] = CellType.None;
    }

    public static string ParseStage(int stageIndex)
    {
        return stageIndex switch
        {
            1 => "Generating simple room rectangles randomly",
            2 => "Generating triangulation graph ",
            3 => "Finding a minimum spanning tree",
            4 => "Applying room rectangles to the grid",
            5 => "Finding paths between the rooms",
            6 => "Generating walls around the rooms and hallways",
            7 => "Generating bitmaps and bitmasks",
            8 => "Creating Collisions and Player",
            _ => "Unknown"
        };
    }

    private void CreateCollisions()
    {
        var rects = RectangleOptimizer.GetCollisionRectangles(BitMap!, GridSize, GridSize);
        CollisionRects = rects;

        _colliders = new Collider[rects.Count];
        for (int i = 0; i < rects.Count; i++)
        {
            var r = rects[i];
            var collider = new BoxCollider(r.X, r.Y, r.Width, r.Height);
            _colliders[i] = collider;

            //Physics.AddCollider(collider);
            Entity.AddComponent(collider);
        }
    }

    private void CreatePlayer()
    {
        var entity = new Entity("player");

        var playerSprite = Core.Scene.Content.LoadTexture(Nez.Content.Steering.RedSquare);

        entity
            .AddComponent(new Mover())
            .AddComponent(new SimplePlayerComponent())
            .AddComponent(new SpriteRenderer(playerSprite))
            .AddComponent(new BoxCollider());

        var firstRectCenter = RoomRects.First().Center;
        entity.Position = firstRectCenter;

        Core.Scene.AddEntity(entity);
    }

    public List<Rectangle> CollisionRects;

    private HashSet<NodeCostable> ConvertToNodes()
    {
        HashSet<NodeCostable> nodesCostable = new HashSet<NodeCostable>();

        for (int y = 0; y < WorldHeightTiles; y++)
        {
            for (int x = 0; x < WorldWidthTiles; x++)
            {
                var cell = Grid[y, x];
                int cost = cell switch
                {
                    CellType.None => Settings.CostEmptySpace,
                    CellType.Room => Settings.CostRoom,
                    CellType.Hallway => Settings.CostHallway,
                    CellType.Wall => 9999,
                    _ => throw new ArgumentOutOfRangeException()
                };

                nodesCostable.Add(
                    new NodeCostable
                    {
                        Node = new Point(x, y),
                        Weight = cost
                    }
                );
            }
        }

        return nodesCostable;
    }

    private bool MinAreaConstraint()
    {
        int cur = 0;

        if (RoomRects.Count >= Settings.MaxRooms)
            return true;

        foreach (var rect in RoomRects)
        {
            cur += (int)(rect.Width * rect.Height);
        }

        return cur >= Settings.MinUsedArea;
    }

    private bool IntersectsWithAny(RectangleF rect, Vector2 offset)
    {
        rect.Inflate(offset.X, offset.Y);
        return RoomRects.Any(r => r.Intersects(rect));
    }

    private RectangleF GenerateRect(int minW, int minH, int maxW, int maxH)
    {
        var size = NextPoint(minW, minH, maxW, maxH);

        var pos = NextPoint(
            GridSize,
            GridSize,
            WorldWidth - size.X - GridSize,
            WorldHeight - size.Y - GridSize
        );

        return new RectangleF(
            pos.X / GridSize * GridSize,
            pos.Y / GridSize * GridSize,
            size.X / GridSize * GridSize,
            size.Y / GridSize * GridSize
        );
    }

    private Point NextPoint(int minW, int minH, int maxW, int maxH)
    {
        return new Point(
            Random.RNG.Next(minW, maxW), 
            Random.RNG.Next(minH, maxH)
        );
    }
}