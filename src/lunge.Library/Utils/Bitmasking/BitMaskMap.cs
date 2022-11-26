using Microsoft.Xna.Framework;

namespace lunge.Library.Utils.Bitmasking;

public class BitMaskMap
{
    private readonly bool[,] _bitMap;
    private readonly int[,] _bitMask;
    
    public int Width { get; }
    public int Height { get; }
    public BitMaskDirections Directions { get; }

    public int this[int y, int x] => GetMask(x, y);
    public int this[Point pos] => GetMask(pos);

    public bool RecalculateOnUpdate { get; set; } = true;

    public BitMaskMap(int width, int height, BitMaskDirections directions)
        : this(new bool[height, width], directions, false)
    { }

    public BitMaskMap(bool[,] bitMap, BitMaskDirections directions, bool calcImmediately = true)
    {
        Width = bitMap.GetLength(1);
        Height = bitMap.GetLength(0);
        Directions = directions;

        _bitMap = bitMap;
        _bitMask = new int[Height, Width];
        
        if (calcImmediately)
            RecalculateBitMask();
    }

    public void RecalculateBitMask()
    {
        BitMaskCalculator.CalculateBitMaskForBitMap(_bitMask, _bitMap, Directions);
    }

    public bool GetBit(int x, int y)
        => _bitMap[y, x];

    public bool GetBit(Point pos)
        => GetBit(pos.X, pos.Y);

    public void SetBit(int x, int y, bool bit)
    {
        _bitMap[y, x] = bit;
        if (RecalculateOnUpdate)
            RecalculateBitMask();
    }

    public void SetBit(Point pos, bool bit)
        => SetBit(pos.X, pos.Y, bit);

    public int GetMask(Point pos)
        => GetMask(pos.X, pos.Y);
    public int GetMask(int x, int y)
        => _bitMask[y, x];
}