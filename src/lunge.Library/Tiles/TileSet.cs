using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;

namespace lunge.Library.Tiles;

public class TileSet
{
    public int TileWidth { get; }
    public int TileHeight { get; }
    
    public List<Sprite> Sprites { get; }

    private Dictionary<string, TilesetTile> _tiles;
    
    public int AtlasWidth { get; }
    public int AtlasHeight { get; }

    public TileSet(Texture2D atlas, int tileWidth, int tileHeight)
    {
        _tiles = new Dictionary<string, TilesetTile>();
        
        TileWidth = tileWidth;
        TileHeight = tileHeight;

        AtlasWidth = atlas.Width;
        AtlasHeight = atlas.Height;

        Sprites = Sprite.SpritesFromAtlas(atlas, tileWidth, tileHeight);
    }

    public TilesetTile Get(string name)
    {
        return _tiles[name];
    }

    public TilesetTile Add(string name, int index)
    {
        var tile = new TilesetTile(this, index, name);
        _tiles[name] = tile;
        return tile;
    }

    public int IndexFromPosition(Point point)
        => IndexFromPosition(point.X, point.Y);

    public int IndexFromPosition(int x, int y)
        => y * (AtlasWidth / TileWidth) + x;
}