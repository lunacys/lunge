using System.Collections.Generic;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Nez;

namespace Playground.Components.WorldGeneration;

public class WorldData
{
    public List<RectangleF> RoomRectangles { get; }

    private CellType[,] _grid = null!;

    public int WorldWidth { get; }
    public int WorldHeight { get; }

    private bool[,] _bitMap = null!;

    public int[,] DefaultBitMask { get; }

    public WorldData(int width, int height, List<RectangleF> roomRects, int[,]? defaultBitMask = null)
    {
        WorldWidth = width;
        WorldHeight = height;

        ResetGrid();

        RoomRectangles = roomRects;

        if (defaultBitMask == null)
            defaultBitMask = CalculateBitMaskForCell(CellType.Wall, false);

        DefaultBitMask = defaultBitMask;
    }

    public void ResetGrid()
    {
        _grid = new CellType[WorldHeight, WorldWidth];
        _bitMap = new bool[WorldHeight, WorldWidth];
    }

    public int[,] CalculateBitMaskForCell(CellType cell, bool includeDiagonals)
    {
        return BitMaskHelper.CalculateBitMaskForBitMap(_bitMap, includeDiagonals);
    }

    public CellType GetCellAt(int x, int y) 
        => _grid[y, x];
    public CellType GetCellAt(Point point)
        => GetCellAt(point.X, point.Y);

    public void SetCellAt(int x, int y, CellType cell)
        => _grid[y, x] = cell;
    public void SetCellAt(Point point, CellType cell)
        => SetCellAt(point.X, point.Y, cell);

    public bool GetBitAt(int x, int y)
        => _bitMap[y, x];
    public bool GetBitAt(Point point)
        => GetBitAt(point.X, point.Y);

    public void SetBitAt(int x, int y, bool bit)
        => _bitMap[y, x] = bit;
    public void SetBitAt(Point point, bool bit)
        => SetBitAt(point.X, point.Y, bit);

    public bool IsNotOutOfBounds(Point point) 
        => BitMaskHelper.IsNotOutOfBounds(point, WorldWidth, WorldHeight);

}