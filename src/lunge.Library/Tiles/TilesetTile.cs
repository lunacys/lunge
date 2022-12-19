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
}