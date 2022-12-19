using Microsoft.Xna.Framework;

namespace lunge.Library.Tiles;

public class LayerTile
{
    public float TravelCost { get; set; } = 1;
    public bool IsPassable { get; set; } = true;
    public TilesetTile Tile { get; }
    public Point Position { get; set; }

    public LayerTile(TilesetTile tile)
    {
        Tile = tile;
    }
}