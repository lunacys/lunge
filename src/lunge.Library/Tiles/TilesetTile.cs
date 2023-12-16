using Microsoft.Xna.Framework;

namespace lunge.Library.Tiles;

public class TilesetTile
{
    public TileSet TileSet { get; }
    public string Name { get; }
    /// <summary>
    /// Gets the index of the tile in the tile array
    /// </summary>
    public int Index { get; }

    public TilesetTile(TileSet tileSet, int index, string name)
    {
        TileSet = tileSet;
        Index = index;
        Name = name;
    }

    public TilesetTile(TileSet tileSet, Point position, string name)
        : this(tileSet, position.X, position.Y, name)
    { }

    public TilesetTile(TileSet tileSet, int x, int y, string name)
        : this(tileSet, IndexFromPos(tileSet, x, y), name)
    { }

    public int IndexFromPosition(int x, int y) => TileSet.IndexFromPosition(x, y);
    public int IndexFromPosition(Point point) => TileSet.IndexFromPosition(point);

    private static int IndexFromPos(TileSet tileSet, int x, int y) => tileSet.IndexFromPosition(x, y);
}