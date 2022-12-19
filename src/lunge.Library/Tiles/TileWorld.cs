using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Tiles;

public class TileWorld
{
    public List<TileLayer> Layers { get; }
    public Dictionary<string, TileLayer> LayerMap { get; }

    public int Width { get; }
    public int Height { get; }
    public int TileWidth { get; }
    public int TileHeight { get; }
    public TileSet TileSet { get; }

    public TileWorld(int width, int height, int tileWidth, int tileHeight, TileSet tileSet)
    {
        Layers = new List<TileLayer>();
        LayerMap = new Dictionary<string, TileLayer>();

        Width = width;
        Height = height;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        TileSet = tileSet;
    }

    public TileLayer GetLayer(int index) => Layers[index];
    public TileLayer GetLayer(string name) => LayerMap[name];

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

    public int TileToWorldX(int x)
    {
        return x * TileWidth;
    }

    public int TileToWorldY(int y)
    {
        return y * TileHeight;
    }

    public void SetTileOnLayer(int layerId, Point pos, LayerTile tile)
        => Layers[layerId][pos] = tile;

    public void SetTileOnLayer(int layerId, int x, int y, LayerTile tile)
        => Layers[layerId][x, y] = tile;

    public LayerTile GetTileOnLayer(int layerId, Point pos)
        => Layers[layerId][pos];

    public LayerTile GetTileOnLayer(int layerId, int x, int y)
        => Layers[layerId][x, y];
}