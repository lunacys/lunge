using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;

namespace lunge.Library.Tiles;

public class TileSet
{
    public int TileWidth { get; }
    public int TileHeight { get; }
    
    public List<Sprite> Sprites { get; }

    private Dictionary<string, TilesetTile> _tiles;

    public TileSet(Texture2D atlas, int tileWidth, int tileHeight)
    {
        _tiles = new Dictionary<string, TilesetTile>();
        
        TileWidth = tileWidth;
        TileHeight = tileHeight;

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
}