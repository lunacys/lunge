using System;
using System.Collections.Generic;
using ImGuiNET;
using lunge.Library;
using lunge.Library.Utils;
using lunge.Library.Utils.DelaunayAlgorithm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.ImGuiTools;
using Nez.Textures;
using Edge = lunge.Library.Edge;

namespace Playground.Components.WorldGeneration;

public class WorldRendererComponent : RenderableComponent
{
    public override float Width => 10_000;
    public override float Height => 10_000;

    private WorldGeneratorComponent _worldGenerator;

    private RenderTarget2D _gridRenderTarget2D;

    private bool _renderGrid = false;
    private bool _renderRooms = false;
    private bool _renderAllEdges = false;
    private bool _renderHallways = false;
    private bool _renderAdditionalHallWays = false;
    private bool _renderGridedRooms = true;
    private bool _renderCircles = true;

    private bool _renderCarmodyNoise = false;
    private bool _renderGustavsonNoise = false;

    private float _gustavsonThreshold = .5f;

    private List<Sprite> _sprites = new List<Sprite>();

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnAddedToEntity()
    {
        var texture = Core.Scene.Content.LoadTexture(Nez.Content.Tiles.Wall1tileset);
        _sprites = Sprite.SpritesFromAtlas(texture, 32, 32);

        _worldGenerator = Entity.GetComponent<WorldGeneratorComponent>();

        _gridRenderTarget2D = RenderTarget.Create((int)Width, (int)Height);

        _gridRenderTarget2D.RenderFrom(batcher => DrawGrid(batcher, Color.DarkGray * 0.5f, 1f));

        Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(DrawAction);
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        var gridSize = _worldGenerator.GridSize;

        if (_renderGrid)
            batcher.Draw(_gridRenderTarget2D, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One,
                SpriteEffects.None, 0f);

        batcher.DrawRect(0, 0, _worldGenerator.WorldWidth, _worldGenerator.WorldHeight, Color.CornflowerBlue);

        if (_renderGridedRooms && _worldGenerator.BitMask != null)
        {
            for (int y = 0; y < _worldGenerator.Grid.GetLength(0); y++)
            {
                for (int x = 0; x < _worldGenerator.Grid.GetLength(1); x++)
                {
                    var cell = _worldGenerator.Grid[y, x];
                    var bit = _worldGenerator.BitMap[y, x];
                    var bitStr = bit ? "1" : "0";
                    var mask = _worldGenerator.BitMask[y, x];
                    var maskStr = Convert.ToString(mask, 2)
                        .PadLeft(8, '0')
                        .Insert(4, "\n");

                    if (cell == CellType.Room)
                    {
                        batcher.DrawRect(x * gridSize, y * gridSize, gridSize, gridSize, Color.DarkGray);
                        //batcher.DrawHollowRect(x * gridSize, y * gridSize, gridSize, gridSize, Color.Gray, 3f);
                    }
                    else if (cell == CellType.Hallway)
                    {
                        batcher.DrawRect(x * gridSize, y * gridSize, gridSize, gridSize, Color.DarkGray);
                        //batcher.DrawHollowRect(x * gridSize, y * gridSize, gridSize, gridSize, Color.Gray, 3f);
                    }
                    else if (cell == CellType.Wall)
                    {

                        //batcher.DrawRect(x * gridSize, y * gridSize, gridSize, gridSize, Color.Gray);
                        //batcher.Draw(_sprites[4].Texture2D, new Vector2(x * gridSize, y * gridSize), _sprites[4].SourceRect, Color.White);
                        batcher.DrawSprite(_sprites[mask], new Vector2(x * gridSize, y * gridSize));
                        //batcher.DrawHollowRect(x * gridSize, y * gridSize, gridSize, gridSize, new Color(66, 66, 66), 3f);
                    }

                    if (_worldGenerator.BitMap != null && _worldGenerator.BitMask != null)
                    {
                        if (bit)
                        {
                            /*batcher.DrawString(
                                Graphics.Instance.BitmapFont,
                                $"{mask}",
                                new Vector2(x * gridSize + 4, y * gridSize + 4),
                                Color.Red
                            );*/

                            /*batcher.DrawString(
                                Graphics.Instance.BitmapFont,
                                $"{maskStr}",
                                new Vector2(x * gridSize + 4, y * gridSize + 12),
                                Color.Purple
                            );*/
                        }
                    }
                }
            }
        }

        if (_renderRooms)
            foreach (var room in _worldGenerator.RoomRects)
            {
                batcher.DrawHollowRect(room.X, room.Y, room.Width, room.Height, Color.Red, 3f);
            }

        if (_worldGenerator.TriangulationGraph != null && _renderAllEdges)
            foreach (var edge in _worldGenerator.TriangulationGraph.Edges)
            {
                DrawEdge(batcher, edge, Color.Gray * 0.4f, 10);
            }

        /*foreach (var triangle in _worldGenerator.TriangulationGraph.Triangles)
        {
            DrawTriangle(batcher, triangle, Color.Green);
        }*/
        if (_worldGenerator.Edges != null && _renderHallways)
            foreach (var edge in _worldGenerator.Edges)
            {
                DrawEdge(batcher, edge, Color.LightGreen, 20);
            }

        if (_worldGenerator.AdditionalEdges != null && _renderAdditionalHallWays)
            foreach (var edge in _worldGenerator.AdditionalEdges)
            {
                DrawEdge(batcher, edge, Color.Green, 20);
            }

        if (_worldGenerator.AStarPath != null && _renderAdditionalHallWays)
            for (int i = 0; i < _worldGenerator.AStarPath.Count - 1; i++)
            {
                var a = _worldGenerator.AStarPath[i];
                var b = _worldGenerator.AStarPath[i + 1];

                DrawEdge(batcher, a, b, Color.LightBlue, 25f);
            }

        if (_renderCircles)
            foreach (var circle in _worldGenerator.Circles)
            {
                batcher.DrawCircle(circle.Center, circle.Radius, Color.Blue, 3f, 32);
            }

        if (_renderCarmodyNoise)
        {
            for (int y = 0; y < _worldGenerator.Grid.GetLength(0); y++)
            {
                for (int x = 0; x < _worldGenerator.Grid.GetLength(1); x++)
                {
                    var cell = _worldGenerator.CarmodyNoise[y, x];

                    batcher.DrawRect(x * gridSize, y * gridSize, gridSize, gridSize, cell);
                }
            }                                                                                                                     
        }

        if (_renderGustavsonNoise)
        {
            for (int y = 0; y < _worldGenerator.Grid.GetLength(0); y++)
            {
                for (int x = 0; x < _worldGenerator.Grid.GetLength(1); x++)
                {
                    var cell = _worldGenerator.GustavsonNoise[y, x];
                    if (cell.A <= _gustavsonThreshold * 255)
                        cell = Color.Transparent;

                    batcher.DrawRect(x * gridSize, y * gridSize, gridSize, gridSize, cell);
                }
            }
        }

        if (_worldGenerator.CollisionRects != null)
        {
            foreach (var rect in _worldGenerator.CollisionRects)
            {
                batcher.DrawHollowRect(rect, Color.Green, 5f);
            }
        }

        //var first = _worldGenerator.RoomRects.First().Center;
        //var last = _worldGenerator.RoomRects.Last().Center;

        //batcher.DrawCircle(first.X, first.Y, 32f, Color.Black, 10, 32);
        //batcher.DrawCircle(last.X, last.Y, 32f, Color.Yellow, 10, 32);

        // Draw bounds
        batcher.DrawHollowRect(0, 0, _worldGenerator.WorldWidth, _worldGenerator.WorldHeight, Color.Red, 10f);
    }

    private void DrawAction()
    {
        ImGui.Begin("World Generator Renderer Settings");

        ImGui.Checkbox("Render Grid", ref _renderGrid);
        ImGui.Checkbox("Render Rooms", ref _renderRooms);
        ImGui.Checkbox("Render All Edges", ref _renderAllEdges);
        ImGui.Checkbox("Render Hallways", ref _renderHallways);
        ImGui.Checkbox("Render Additional Hallways", ref _renderAdditionalHallWays);
        ImGui.Checkbox("Render Grided Room", ref _renderGridedRooms);
        ImGui.Checkbox("Render Carmody Noise", ref _renderCarmodyNoise);
        ImGui.Checkbox("Render Gustavson Noise", ref _renderGustavsonNoise);
        ImGui.SliderFloat("Gustavson Noise Threshold", ref _gustavsonThreshold, 0, 1.0f);
        ImGui.Checkbox("Render Circles", ref _renderCircles);

        ImGui.End();
    }

    private void DrawGrid(Batcher batcher, Color color, float thickness = 1f)
    {
        var gridSize = _worldGenerator.GridSize;
        for (int i = 0; i < Width; i += gridSize)
        {
            for (int j = 0; j < Height; j += gridSize)
            {
                batcher.DrawHollowRect(j, i, gridSize, gridSize, color, thickness);
            }
        }
    }

    private void DrawEdge(Batcher batcher, Edge edge, Color color, float thickness = 3f)
    {
        batcher.DrawLine(edge.U, edge.V, color, thickness);
    }

    private void DrawEdge(Batcher batcher, Vector2 p1, Vector2 p2, Color color, float thickness = 3f)
    {
        batcher.DrawLine(p1, p2, color, thickness);
    }

    private void DrawTriangle(Batcher batcher, Triangle triangle, Color color)
    {
        batcher.DrawLine(triangle.A, triangle.B, color, 3);
        batcher.DrawLine(triangle.B, triangle.C, color, 3);
        batcher.DrawLine(triangle.C, triangle.A, color, 3);
    }
}