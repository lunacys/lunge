using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Tiles;

public class TileWorld
{
    public List<TileLayer> Layers { get; }

    public int Width { get; }
    public int Height { get; }
    public int TileWidth { get; }
    public int TileHeight { get; }
    public TileSet TileSet { get; }

    public TileWorld(int width, int height, int tileWidth, int tileHeight, TileSet tileSet)
    {
        Layers = new List<TileLayer>();

        Width = width;
        Height = height;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        TileSet = tileSet;
    }

    public TileLayer? GetLayer(string? name) => Layers.FirstOrDefault(l => l.Name == name);
    public TileLayer? GetLayer(int index) => index >= 0 && index < Layers.Count ? Layers[index] : null;

    public Point WorldToTile(Vector2 position, bool clamp = true)
    {
        return new Point(
            WorldToTileX(position.X, clamp),
            WorldToTileY(position.Y, clamp)
        );
    }

    public int WorldToTileX(float x, bool clampToTilemapBounds = true)
    {
        var tileX = Mathf.FastFloorToInt(x / TileWidth);
        if (!clampToTilemapBounds)
            return tileX;
        return Mathf.Clamp(tileX, 0, Width - 1);
    }

    public int WorldToTileY(float y, bool clampToTilemapBounds = true)
    {
        var tileY = Mathf.FloorToInt(y / TileHeight);
        if (!clampToTilemapBounds)
            return tileY;
        return Mathf.Clamp(tileY, 0, Height - 1);
    }

    public Vector2 TileToWorld(Point position)
    {
        return new Vector2(
            TileToWorldX(position.X),
            TileToWorldY(position.Y)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int TileToWorldX(int x)
        => x * TileWidth;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int TileToWorldY(int y)
        => y * TileHeight;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOutOfBounds(Point pos)
        => pos.X < 0 || pos.Y < 0 || pos.X >= Width || pos.Y >= Height;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOutOfBounds(int x, int y)
        => x < 0 || y < 0 || x >= Width || y >= Height;
    
    
}