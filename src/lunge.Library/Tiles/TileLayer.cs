using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace lunge.Library.Tiles;

public class TileLayer
{
    private readonly LayerTile?[,] _tileMap;
    
    public string Name { get; }
    
    public int Width { get; }
    public int Height { get; }

    public bool IsVisible { get; set; } = true;

    public LayerTile? this[int x, int y]
    {
        get => GetTileAt(x, y);
        set => SetTileAt(x, y, value);
    }
    
    public LayerTile? this[Point tilePos]
    {
        get => GetTileAt(tilePos);
        set => SetTileAt(tilePos, value);
    }
    
    public TileWorld World { get; }
    
    public Vector2 RenderOffset { get; set; } = Vector2.Zero;

    public TileLayer(TileWorld world, string name)
    {
	    World = world;
        Width = world.Width;
        Height = world.Height;
        Name = name;
        _tileMap = new LayerTile[world.Height, world.Width];
        Clear();
    }

    public LayerTile? GetTileAt(Point tilePos) => GetTileAt(tilePos.X, tilePos.Y);
    public LayerTile? GetTileAt(int x, int y) => _tileMap[y, x];
    public void SetTileAt(Point tilePos, LayerTile? layerTile)
    {
        _tileMap[tilePos.Y, tilePos.X] = layerTile;
        if (layerTile != null)
            layerTile!.Position = tilePos;
    }
    public void SetTileAt(int x, int y, LayerTile? layerTile)
    {
        SetTileAt(new Point(x, y), layerTile);
    }

    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            SetTileAt(x, y, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOutOfBounds(Point pos)
	    => World.IsOutOfBounds(pos);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOutOfBounds(int x, int y)
	    => World.IsOutOfBounds(x, y);

    public List<Rectangle> GetCollisionRectangles()
    {
	    var checkedIndexes = new bool?[Width * Height];
	    var rectangles = new List<Rectangle>();
	    var startCol = -1;
	    var index = -1;

	    for (var y = 0; y < Height; y++)
	    {
		    for (var x = 0; x < Width; x++)
		    {
			    index = y * Width + x;
			    var tile = GetTileAt(x, y);

			    if (tile != null && (checkedIndexes[index] == false || checkedIndexes[index] == null))
			    {
				    if (startCol < 0)
					    startCol = x;

				    checkedIndexes[index] = true;
			    }
			    else if (tile == null || checkedIndexes[index] == true)
			    {
				    if (startCol >= 0)
				    {
					    rectangles.Add(FindBoundsRect(startCol, x, y, checkedIndexes));
					    startCol = -1;
				    }
			    }
		    } // end for x

		    if (startCol >= 0)
		    {
			    rectangles.Add(FindBoundsRect(startCol, Width, y, checkedIndexes));
			    startCol = -1;
		    }
	    }

	    return rectangles;
    }

    /// <summary>
    /// Finds the largest bounding rect around tiles between startX and endX, starting at startY and going
    /// down as far as possible
    /// </summary>
    public Rectangle FindBoundsRect(int startX, int endX, int startY, bool?[] checkedIndexes)
    {
	    var index = -1;

	    for (var y = startY + 1; y < Height; y++)
	    {
		    for (var x = startX; x < endX; x++)
		    {
			    index = y * Width + x;
			    var tile = GetTileAt(x, y);

			    if (tile == null || checkedIndexes[index] == true)
			    {
				    // Set everything we've visited so far in this row to false again because it won't be included in the rectangle and should be checked again
				    for (var _x = startX; _x < x; _x++)
				    {
					    index = y * Width + _x;
					    checkedIndexes[index] = false;
				    }

				    return new Rectangle(startX * World.TileWidth, startY * World.TileHeight,
					    (endX - startX) * World.TileWidth, (y - startY) * World.TileHeight);
			    }

			    checkedIndexes[index] = true;
		    }
	    }

	    return new Rectangle(startX * World.TileWidth, startY * World.TileHeight,
		    (endX - startX) * World.TileWidth, (World.Height - startY) * World.TileHeight);
    }

    private void ForEach(Action<LayerTile?> action)
    {
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            action(GetTileAt(x, y));
    }

    private IEnumerable<LayerTile?> GetEach()
    {
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            yield return GetTileAt(x, y);
    }
}